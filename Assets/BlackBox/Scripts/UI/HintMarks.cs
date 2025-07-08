using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackBox
{
    public class HintMarks : MonoBehaviour
    {
        public enum XO {None, X, Oq, O};
        private float[] markAlpha = {0f, 0.25f, 0.25f, 0.25f};

        [SerializeField] private Sprite[] markImage; //correlates with XO enum
        [SerializeField] public GameObject[] gridGO;
        private List<XO> gridMark = new List<XO>();

        private int size;
        private int[] gridSizeValues = new int[4] { 5, 6, 7, 4 };
        private float[] gridSizeGO = new float[4] { 200f, 166.66f, 142.85f, 250f };



        public void ResetGrid()
        {
            GridSize gs = BBEvents.GetLevel.Invoke().gridSize;
            size = gridSizeValues[(int)gs];
            this.GetComponent<GridLayoutGroup>().cellSize = new Vector2(gridSizeGO[(int)gs], gridSizeGO[(int)gs]);

            gridMark.Clear();
            for (int i=0; i<gridGO.Length; i++)
            {
                if (i<size*size) {
                    gridMark.Add(XO.None);
                    gridGO[i].SetActive(true);
                    SetMarkGraphic(gridGO[i], gridMark[i]);
                } else {
                    gridGO[i].SetActive(false);
                }
            }
        }

        public void UpdateHintGrid(Vector3Int orig, Vector3Int dest, Marker pathType)
        {            
            // Debug.Log($"Orig: ({orig.x}, {orig.y}), Dest: ({dest.x}, {dest.y})");
            switch(pathType)
            {
                case Marker.Miss:                    
                    if (orig.x == dest.x)
                    {
                        int xi = orig.x;
                        for (int i=0; i<size; i++) {
                            gridMark[xyTOi(new Vector3Int(xi, i, 0))] = XO.X;

                            if (xi == 0) {
                                gridMark[xyTOi(new Vector3Int(xi+1, i, 0))] = XO.X;
                            } else if (xi == size-1) {
                                gridMark[xyTOi(new Vector3Int(xi-1, i, 0))] = XO.X;
                            }
                        }
                    }
                    else if (orig.y == dest.y)
                    {
                        int yi = orig.y;
                        for (int i=0; i<size; i++) {
                            gridMark[xyTOi(new Vector3Int(i, yi, 0))] = XO.X;

                            if (yi == 0) {
                                gridMark[xyTOi(new Vector3Int(i, yi+1, 0))] = XO.X;
                            } else if (yi == size-1) {
                                gridMark[xyTOi(new Vector3Int(i, yi-1, 0))] = XO.X;
                            }
                        }
                    }
                    break;

                case Marker.Hit:
                    // orig and dest are same coordates
                    if (orig.x == -1 || orig.x == size) { // Horizontal
                        for (int i=0; i<size; i++) {
                            Vector3Int temp = new Vector3Int(i, orig.y, 0);
                            if (gridMark[xyTOi(temp)] == XO.None) {
                                gridMark[xyTOi(temp)] = XO.Oq;
                            }
                        }
                    } 
                    else if (orig.y == -1 || orig.y == size) { // Vertical
                        for (int i=0; i<size; i++) {
                            Vector3Int temp = new Vector3Int(orig.x, i, 0);
                            if (gridMark[xyTOi(temp)] == XO.None) {
                                gridMark[xyTOi(temp)] = XO.Oq;
                            }
                        }
                    }
                    break;

                case Marker.Detour:
                    // Only set up for 1 turn
                    if (orig.z == dest.z) break;

                    // Set up turn coordinates
                    Vector3Int turn = new Vector3Int(orig.x, dest.y);
                    if (orig.x == -1 || orig.x == size) { // Invert corner location
                        turn.x = dest.x; turn.y = orig.y;
                    }
                    Vector3Int nodeCoor = turn;

                    // Set first half of turn
                    if (orig.x == -1 || orig.x == size)
                    {
                        int start=0; int end=size-1;
                        if (orig.x == -1) { 
                            end = turn.x; 
                            nodeCoor.x = turn.x+1;
                        }
                        else if (orig.x == size) {
                            start = turn.x;
                            nodeCoor.x = turn.x-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(i, orig.y, 0))] = XO.X;
                            if (orig.y > 0) {
                                gridMark[xyTOi(new Vector3Int(i, orig.y-1, 0))] = XO.X;
                            } if (orig.y < size-1) {
                                gridMark[xyTOi(new Vector3Int(i, orig.y+1, 0))] = XO.X;
                            }
                        }
                    }
                    else if (orig.y == -1 || orig.y == size)
                    {
                        int start=0; int end=size-1;
                        if (orig.y == -1) { 
                            end = turn.y; 
                            nodeCoor.y = turn.y+1;
                        }
                        else if (orig.y == size) {
                            start = turn.y;
                            nodeCoor.y = turn.y-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(orig.x, i, 0))] = XO.X;
                            if (orig.x > 0) {
                                gridMark[xyTOi(new Vector3Int(orig.x-1, i, 0))] = XO.X;
                            } if (orig.x < size-1) {
                                gridMark[xyTOi(new Vector3Int(orig.x+1, i, 0))] = XO.X;
                            }
                        }
                    }
                    // Set second half of turn
                    if (dest.x == -1 || dest.x == size)
                    {
                        int start=0; int end=size-1;
                        if (dest.x == -1) { 
                            end = turn.x; 
                            nodeCoor.x = turn.x+1;
                        }
                        else if (dest.x == size) {
                            start = turn.x;
                            nodeCoor.x = turn.x-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(i, dest.y, 0))] = XO.X;
                            if (dest.y > 0) {
                                gridMark[xyTOi(new Vector3Int(i, dest.y-1, 0))] = XO.X;
                            } if (dest.y < size-1) {
                                gridMark[xyTOi(new Vector3Int(i, dest.y+1, 0))] = XO.X;
                            }
                        }
                    }
                    else if (dest.y == -1 || dest.y == size)
                    {
                        int start=0; int end=size-1;
                        if (dest.y == -1) { 
                            end = turn.y; 
                            nodeCoor.y = turn.y+1;
                        }
                        else if (dest.y == size) {
                            start = turn.y;
                            nodeCoor.y = turn.y-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(dest.x, i, 0))] = XO.X;
                            if (dest.x > 0) {
                                gridMark[xyTOi(new Vector3Int(dest.x-1, i, 0))] = XO.X;
                            } if (dest.x < size-1) {
                                gridMark[xyTOi(new Vector3Int(dest.x+1, i, 0))] = XO.X;
                            }
                        }
                    }
                    gridMark[xyTOi(nodeCoor)] = XO.O;
                    break;

                case Marker.Reflect:
                // Not implemented
                    break;
            }
        }

        /* Functions for the graphics of the grid */
        
        public void SetMarkGraphic(GameObject spot, XO type)
        {
            Image spotTemp = spot.GetComponent<Image>();
            spotTemp.sprite = markImage[(int)type];
            spotTemp.color = new Color(1,1,1,markAlpha[(int)type]);
        }

        public void UpdateGridGraphics() 
        {
            // PrintGrid();
            for (int i=0; i<gridMark.Count; i++)
            {
                SetMarkGraphic(gridGO[i], gridMark[i]);
            }
        }

        /* Utilities */

        private int xyTOi(Vector3Int coor) {
            return (coor.x + size*coor.y);
        }
        private Vector3Int iTOxy(int i) {
            return new Vector3Int(i%size, i/size, 0);
        }
        
        public void PrintGrid()
        {
            string temp = "";
            for (int i=size-1; i>=0; i--) {
                for (int j=0; j<size; j++) {
                    temp += $"{gridMark[xyTOi(new Vector3Int(j, i, 0))].ToString()}\t";
                }
                temp += "\n";
            }
            Debug.Log(temp);
        }

    }
}