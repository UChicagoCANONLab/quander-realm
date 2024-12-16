using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackBox 
{
    public class HintPopup : MonoBehaviour
    {
        [SerializeField] private Animator WolfieAnimator;
        /* [SerializeField] private string[] hintTexts = {
            "You need to play more before I can give you a hint!",
            "Remember, if you use too many hints, you'll lose a star..."
        }; */

        [SerializeField] private GameObject hintButton;
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject lineContainer;
    
        [SerializeField] private List<Vector3Int[]> hintPairs = new List<Vector3Int[]>();
        [SerializeField] private List<Marker> hintType = new List<Marker>();
        
        private GridSize size;
        private int[] gridSizeValues = new int[4] { 5, 6, 7, 4 };  // Copied from BBGameManager

        private int maxSize;
        private int hintCounter = 0;
        
        private int offsetPt = 30;
        private Vector3[] offsetType = new Vector3[5] {
            new Vector3(0, 0, 0),   // default no offset
            new Vector3(1, 1, 0),   // 1st quadrant
            new Vector3(-1, 1, 0),  // 2nd
            new Vector3(-1, -1, 0), // 3rd
            new Vector3(1, -1, 0)   // 4th
        };

        private Level currLevel;
    

        private void OnEnable() 
        {
            BBEvents.ShowHint += GiveHint;
            BBEvents.AppendHint += AppendHintCoor;
            BBEvents.ClearHints += ClearHintLines;
        }

        private void OnDisable() 
        {
            BBEvents.ShowHint -= GiveHint;
            BBEvents.AppendHint -= AppendHintCoor;
            BBEvents.ClearHints -= ClearHintLines;
        }

        public void GiveHint() 
        {   
            // Don't give hints if not hints to give
            if (hintPairs.Count <= hintCounter) { 
                WolfieAnimator.SetBool("IsOn", true);
                return;    
            }

            // If not tutorial, penalize hints
            if (currLevel.number != 1) {
                BBEvents.LoseLife.Invoke();
                
                if (BBEvents.GetLivesRemaining.Invoke() == 1) {
                    hintButton.GetComponent<Button>().interactable = false;
                }
                // BBEvents.DecrementEnergy.Invoke();
            }

            Vector3 start = (Vector3)hintPairs[hintCounter][0];
            Vector3 end = (Vector3)hintPairs[hintCounter][1];
            Marker currType = hintType[hintCounter];

            Vector3 turn = new Vector3(start.x, end.y, 0); 
            Vector3 turn2 = new Vector3(end.x, end.y, 0);
            
            // Change line shape by type of hit
            bool cornerOn = true;
            bool cornerOn2 = false;
            Vector3 cornerOffset = offsetType[0]; // default no offset
            Vector3 cornerOffset2 = offsetType[0];


            switch(currType) {
                case Marker.Detour:
                    cornerOffset = offsetType[1]; // default to quad 1 offset
                    cornerOffset2 = offsetType[1];
                    
                    // There are no levels with multiple detours where start.z != end.z
                    if (start.z != end.z) {
                        if (start.x==-1 || start.x==maxSize) { // Invert line shape
                            turn.x = end.x; turn.y = start.y; 
                        }
                        // Change turn icon location by type of turn
                        if ((start.z==1 && end.z==2) || (start.z==2 && end.z==1)) {
                            // No need to change direction of icon offset
                        } else if ((start.z==2 && end.z==3) || (start.z==3 && end.z==2)) {
                            cornerOffset = offsetType[2]; 
                        } else if ((start.z==3 && end.z==4) || (start.z==4 && end.z==3)) {
                            cornerOffset = offsetType[3];
                        } else if ((start.z==4 && end.z==1) || (start.z==1 && end.z==4)) {
                            cornerOffset = offsetType[4];
                        }
                    }
                    else { // Two detours, start.z == end.z
                        cornerOn2 = true;
                        turn.y = start.y; // so turn1==start and turn2==end

                        switch(start.z) {
                            case 1:     // Left
                                turn.x = maxSize;
                                foreach (Vector2Int node in currLevel.nodePositions) {
                                    if ((node.y == start.y + 1 || node.y == start.y - 1) // First detour
                                    && (node.x - 1 < turn.x)) {  // Closest node
                                        turn.x = node.x - 1;
                                        turn2.x = node.x - 1;
                                    }
                                } if (turn.y > turn2.y) {
                                    cornerOffset2 = offsetType[4];
                                } else {
                                    cornerOffset = offsetType[4];
                                }
                                break;
                            case 2:     // Bottom
                                turn.y = maxSize;
                                foreach (Vector2Int node in currLevel.nodePositions) {
                                    if ((node.x == start.x + 1 || node.x == start.x - 1) 
                                    && (node.y - 1 < turn.y)) {  
                                        turn.y = node.y - 1;
                                        turn2.y = node.y - 1;
                                    }
                                } if (turn.x > turn2.x) {
                                    cornerOffset2 = offsetType[2];
                                } else {
                                    cornerOffset = offsetType[2];
                                }
                                break;
                            case 3:     // Right
                                turn.x = 0;
                                foreach (Vector2Int node in currLevel.nodePositions) {
                                    if ((node.y == start.y + 1 || node.y == start.y - 1) 
                                    && (node.x + 1 > turn.x)) {  
                                        turn.x = node.x + 1;
                                        turn2.x = node.x + 1;
                                    }
                                } if (turn.y > turn2.y) {
                                    cornerOffset = offsetType[2];
                                    cornerOffset2 = offsetType[3];
                                } else {
                                    cornerOffset = offsetType[3];
                                    cornerOffset2 = offsetType[2];
                                }
                                break;
                            case 4:     // Top
                                turn.y = 0;
                                foreach (Vector2Int node in currLevel.nodePositions) {
                                    if ((node.x == start.x + 1 || node.x == start.x - 1) 
                                    && (node.y + 1 > turn.y)) {  
                                        turn.y = node.y + 1;
                                        turn2.y = node.y + 1;
                                    }
                                } if (turn.x > turn2.x) {
                                    cornerOffset = offsetType[4];
                                    cornerOffset2 = offsetType[3];
                                } else {
                                    cornerOffset = offsetType[3];
                                    cornerOffset2 = offsetType[4];
                                }
                                break;
                        }
                    }
                    break;

                case Marker.Miss:
                    // (start.x==end.x || start.y==end.y) && (start.z != end.z)
                    turn.x = (start.x+end.x)/2;
                    turn.y = (start.y+end.y)/2;
                    cornerOn = false;    
                    break;

                case Marker.Hit:
                    // (start.x==end.x && start.y==end.y) && (node.y == start.y)
                    // NEED TO CHECK FOR TURNS
                    switch(start.z) {
                        case 1:     // Left
                            turn.x = maxSize;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                if ((node.y == start.y) && (node.x < turn.x)) {  // Closest node
                                    turn.x = node.x - 0.5f;
                                }
                            } 
                            if (turn.x == maxSize) { // hint too complicated -- try next
                                TryNextHint(); return;
                            } break;
                        case 2:     // Bottom
                            turn.y = maxSize;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                if ((node.x == start.x) && (node.y < turn.y)) {
                                    turn.y = node.y - 0.5f;
                                }
                            } 
                            if (turn.y == maxSize) { 
                                TryNextHint(); return;
                            } break;
                        case 3:     // Right
                            turn.x = 0;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                if ((node.y == start.y) && (node.x > turn.x)) {
                                    turn.x = node.x + 0.5f;
                                }
                            } 
                            if (turn.x == 0) {
                                TryNextHint(); return;
                            } break;
                        case 4:     // Top
                            turn.y = 0;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                if ((node.x == start.x) && (node.y > turn.y)) {
                                    turn.y = node.y + 0.5f;
                                }
                            } 
                            if (turn.y == 0) {
                                TryNextHint(); return;
                            } break;
                    }
                    break;
                    
                case Marker.Reflect:
                    // (start.x==end.x && start.y==end.y) && (node.y == start.y +1 (or -1))
                    // NEED TO CHECK FOR TURNS
                    Vector2Int node1 = new Vector2Int(-1,-1);
                    Vector2Int node2 = new Vector2Int(-1,-1);

                    switch(start.z) {
                        case 1:     // Left
                            turn.x = maxSize;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                if ((node.y == start.y + 1) && (node.x - 0.5f <= turn.x)) { // closest node
                                    turn.x = node.x - 0.5f;
                                    node1 = node;
                                } else if ((node.y == start.y - 1) && (node.x - 0.5f <= turn.x)) {
                                    turn.x = node.x - 0.5f;
                                    node2 = node;
                                }
                            } if (node1.x != node2.x) {
                                TryNextHint(); return;
                            } break;
                        case 2:     // Bottom
                            turn.y = maxSize;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                // if ((node.x == start.x + 1 || node.x == start.x - 1) 
                                // && (node.y < turn.y)) {
                                //     turn.y = node.y - 0.5f;
                                // }
                                if ((node.x == start.x + 1) && (node.y - 0.5f <= turn.y)) { // closest node
                                    turn.y = node.y - 0.5f;
                                    node1 = node;
                                } else if ((node.x == start.x - 1) && (node.y - 0.5f <= turn.y)) {
                                    turn.y = node.y - 0.5f;
                                    node2 = node;
                                }
                            } if (node1.y != node2.y) {
                                TryNextHint(); return;
                            } break;
                        case 3:     // Right
                            turn.x = 0;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                // if ((node.y == start.y + 1 || node.y == start.y - 1) 
                                // && (node.x > turn.x)) {
                                //     turn.x = node.x + 0.5f;
                                // }
                                if ((node.y == start.y + 1) && (node.x + 0.5f >= turn.x)) { // closest node
                                    turn.x = node.x + 0.5f;
                                    node1 = node;
                                } else if ((node.y == start.y - 1) && (node.x + 0.5f >= turn.x)) {
                                    turn.x = node.x + 0.5f;
                                    node2 = node;
                                }
                            } if (node1.x != node2.x) {
                                TryNextHint(); return;
                            } break;
                        case 4:     // Top
                            turn.y = 0;
                            foreach (Vector2Int node in currLevel.nodePositions) {
                                /* if ((node.x == start.x + 1 || node.x == start.x - 1) 
                                && (node.y > turn.y)) {
                                    turn.y = node.y + 0.5f;
                                } */
                                if ((node.x == start.x + 1) && (node.y + 0.5f >= turn.y)) { // closest node
                                    turn.y = node.y + 0.5f;
                                    node1 = node;
                                } else if ((node.x == start.x - 1) && (node.y + 0.5f >= turn.y)) {
                                    turn.y = node.y + 0.5f;
                                    node2 = node;
                                }
                            } if (node1.y != node2.y) {
                                TryNextHint(); return;
                            } break;
                    }
                    // Check if both nodes are present; otherwise, too complicated -- try next hint
                    if (node1 == new Vector2Int(-1,-1) || node2 == new Vector2Int(-1,-1)) {
                        TryNextHint(); return;
                    } 
                    break;

                default:
                    break;
            }

            Vector3[] positions = new Vector3[] {start, turn, end};
            if (cornerOn2) { 
                positions = new Vector3[] {start, turn, turn2, end};
            } 
            
            for (int i=0; i<positions.Length; i++) {
                Vector3 pos = positions[i];
                switch (pos.x) {
                    case -1:        positions[i].x=0; break;
                    case var value when value == maxSize:   positions[i].x=900; break;
                    default:        positions[i].x= pos.x*(900/maxSize)+(900/(maxSize*2)); break;
                } switch (pos.y) {
                    case -1:        positions[i].y=0; break;
                    case var value when value == maxSize:   positions[i].y=900; break;
                    default:        positions[i].y= pos.y*(900/maxSize)+(900/(maxSize*2)); break;
                }
            }
            GameObject currLine = Instantiate(linePrefab, lineContainer.transform);
            currLine.GetComponent<LineRenderer>().positionCount = positions.Length;
            currLine.GetComponent<LineRenderer>().SetPositions(positions);
            
            if (cornerOn){
                GameObject corner = currLine.transform.GetChild(0).gameObject;
                corner.transform.localPosition += (positions[1] + (offsetPt * cornerOffset));
            }
            if (cornerOn2) {
                GameObject corner2 = currLine.transform.GetChild(1).gameObject;
                corner2.transform.localPosition += (positions[2] + (offsetPt * cornerOffset2));
            }

            currLine.GetComponent<Animator>().SetBool("Corner", cornerOn);
            currLine.GetComponent<Animator>().SetBool("Corner2", cornerOn2);
            currLine.GetComponent<Animator>().SetBool("IsOn", true);            
            
            hintCounter++;
            // Debug.Log($"HintLine: {string.Join("; ", positions)}");
        }

        public void ClearHintLines() {
            foreach (Transform transform in lineContainer.transform) {
                UnityEngine.Object.Destroy(transform.gameObject);
            }
            hintCounter = 0;
            hintPairs.Clear();
            hintType.Clear();

            hintButton.GetComponent<Button>().interactable = true;
        }

        public void AppendHintCoor(Vector3Int orig, Dir origDir, Vector3Int dest, Dir destDir, Marker type) {    
            Vector3Int[] pair = new Vector3Int[] {orig, dest};
            Dir[] dirPair = new Dir[] {origDir, destDir};

            currLevel = BBEvents.GetLevel.Invoke();
            size = currLevel.gridSize;
            maxSize = gridSizeValues[(int)size];

            for (int i=0; i<2; i++) {
                pair[i].z = (int)dirPair[i];
                switch(dirPair[i]) {
                    case Dir.Top:   pair[i].y = maxSize;    break;
                    case Dir.Bot:   pair[i].y = -1;         break;
                    case Dir.Left:  pair[i].x = -1;         break;
                    case Dir.Right: pair[i].x = maxSize;    break;
                }
            }
            // Debug.Log(string.Join("; ", pair));
            hintPairs.Add(pair);
            hintType.Add(type);
        }

        public void ExitWolfiePopup() {
            WolfieAnimator.SetBool("IsOn", false);
        }

        public void TryNextHint() {
            hintCounter++;
            GiveHint();
        }

    }
}