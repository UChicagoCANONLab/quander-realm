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
        private XO[] gridMark = {
            XO.None, XO.None, XO.None, XO.None,
            XO.None, XO.None, XO.None, XO.None,
            XO.None, XO.None, XO.None, XO.None,
            XO.None, XO.None, XO.None, XO.None,
        };
        // 4x4 grid, x+4y=i


        public void UpdateHintGrid(Vector3Int orig, Vector3Int dest, Marker pathType)
        {            
            // Debug.Log($"Orig: ({orig.x}, {orig.y}), Dest: ({dest.x}, {dest.y})");
            switch(pathType)
            {
                case Marker.Miss:                    
                    if (orig.x == dest.x)
                    {
                        int xi = orig.x;
                        for (int i=0; i<4; i++) {
                            gridMark[xyTOi(new Vector3Int(xi, i, 0))] = XO.X;

                            if (xi == 0) {
                                gridMark[xyTOi(new Vector3Int(xi+1, i, 0))] = XO.X;
                            } else if (xi == 3) {
                                gridMark[xyTOi(new Vector3Int(xi-1, i, 0))] = XO.X;
                            }
                        }
                    }
                    else if (orig.y == dest.y)
                    {
                        int yi = orig.y;
                        for (int i=0; i<4; i++) {
                            gridMark[xyTOi(new Vector3Int(i, yi, 0))] = XO.X;

                            if (yi == 0) {
                                gridMark[xyTOi(new Vector3Int(i, yi+1, 0))] = XO.X;
                            } else if (yi == 3) {
                                gridMark[xyTOi(new Vector3Int(i, yi-1, 0))] = XO.X;
                            }
                        }
                    }
                    break;

                case Marker.Hit:
                    // orig and dest are same coordates
                    if (orig.x == -1 || orig.x == 4) { // Horizontal
                        for (int i=0; i<4; i++) {
                            Vector3Int temp = new Vector3Int(i, orig.y, 0);
                            if (gridMark[xyTOi(temp)] == XO.None) {
                                gridMark[xyTOi(temp)] = XO.Oq;
                            }
                        }
                    } 
                    else if (orig.y == -1 || orig.y == 4) { // Vertical
                        for (int i=0; i<4; i++) {
                            Vector3Int temp = new Vector3Int(orig.x, i, 0);
                            if (gridMark[xyTOi(temp)] == XO.None) {
                                gridMark[xyTOi(temp)] = XO.Oq;
                            }
                        }
                    }
                    break;

                case Marker.Detour:
                // Only set up for 1 turn
                    Vector3Int turn = new Vector3Int(orig.x, dest.y);
                    if (orig.x == -1 || orig.x == 4) { // Invert corner location
                        turn.x = dest.x; turn.y = orig.y;
                    }
                    Vector3Int nodeCoor = turn;

                    // Set first half of turn
                    if (orig.x == -1 || orig.x == 4)
                    {
                        int start=0; int end=3;
                        if (orig.x == -1) { 
                            end = turn.x; 
                            nodeCoor.x = turn.x+1;
                        }
                        else if (orig.x == 4) {
                            start = turn.x;
                            nodeCoor.x = turn.x-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(i, orig.y, 0))] = XO.X;
                            if (orig.y > 0) {
                                gridMark[xyTOi(new Vector3Int(i, orig.y-1, 0))] = XO.X;
                            } if (orig.y < 3) {
                                gridMark[xyTOi(new Vector3Int(i, orig.y+1, 0))] = XO.X;
                            }
                        }
                    }
                    else if (orig.y == -1 || orig.y == 4)
                    {
                        int start=0; int end=3;
                        if (orig.y == -1) { 
                            end = turn.y; 
                            nodeCoor.y = turn.y+1;
                        }
                        else if (orig.y == 4) {
                            start = turn.y;
                            nodeCoor.y = turn.y-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(orig.x, i, 0))] = XO.X;
                            if (orig.x > 0) {
                                gridMark[xyTOi(new Vector3Int(orig.x-1, i, 0))] = XO.X;
                            } if (orig.x < 3) {
                                gridMark[xyTOi(new Vector3Int(orig.x+1, i, 0))] = XO.X;
                            }
                        }
                    }
                    // Set second half of turn
                    if (dest.x == -1 || dest.x == 4)
                    {
                        int start=0; int end=3;
                        if (dest.x == -1) { 
                            end = turn.x; 
                            nodeCoor.x = turn.x+1;
                        }
                        else if (dest.x == 4) {
                            start = turn.x;
                            nodeCoor.x = turn.x-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(i, dest.y, 0))] = XO.X;
                            if (dest.y > 0) {
                                gridMark[xyTOi(new Vector3Int(i, dest.y-1, 0))] = XO.X;
                            } if (dest.y < 3) {
                                gridMark[xyTOi(new Vector3Int(i, dest.y+1, 0))] = XO.X;
                            }
                        }
                    }
                    else if (dest.y == -1 || dest.y == 4)
                    {
                        int start=0; int end=3;
                        if (dest.y == -1) { 
                            end = turn.y; 
                            nodeCoor.y = turn.y+1;
                        }
                        else if (dest.y == 4) {
                            start = turn.y;
                            nodeCoor.y = turn.y-1;
                        }
                        for (int i=start; i<=end; i++) {
                            gridMark[xyTOi(new Vector3Int(dest.x, i, 0))] = XO.X;
                            if (dest.x > 0) {
                                gridMark[xyTOi(new Vector3Int(dest.x-1, i, 0))] = XO.X;
                            } if (dest.x < 3) {
                                gridMark[xyTOi(new Vector3Int(dest.x+1, i, 0))] = XO.X;
                            }
                        }
                    }
                    gridMark[xyTOi(nodeCoor)] = XO.O;
                    break;
            }
            // UpdateGridGraphics();
        }

        /* Functions for the graphics of the grid */
        
        public void SetMarkGraphic(GameObject spot, XO type)
        {
            Image spotTemp = spot.GetComponent<Image>();
            spotTemp.sprite = markImage[(int)type];
            spotTemp.color = new Color(1,1,1,markAlpha[(int)type]);
        }
        
        public void ResetGrid()
        {
            for (int i=0; i<gridMark.Length; i++)
            {
                gridMark[i] = XO.None;
                SetMarkGraphic(gridGO[i], gridMark[i]);
            }
        }

        public void UpdateGridGraphics() 
        {
            // PrintGrid();
            for (int i=0; i<gridMark.Length; i++)
            {
                SetMarkGraphic(gridGO[i], gridMark[i]);
            }
        }

        /* Utilities */

        private int xyTOi(Vector3Int coor) {
            return (coor.x + 4*coor.y);
        }
        private Vector3Int iTOxy(int i) {
            return new Vector3Int(i%4, i/4, 0);
        }
        
        public void PrintGrid()
        {
            string temp = "";
            for (int i=3; i>=0; i--) {
                for (int j=0; j<4; j++) {
                    temp += $"{gridMark[xyTOi(new Vector3Int(j, i, 0))].ToString()}\t";
                }
                temp += "\n";
            }
            Debug.Log(temp);
        }

    }
}