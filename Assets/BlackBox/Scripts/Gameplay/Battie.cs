using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackBox
{
    public class Battie : MonoBehaviour
    {
        [SerializeField] private Animator BattieAnimator;
        [SerializeField] private GameObject BattieContainer;
        private bool active = false;
        
        private Vector3 start;
        private Vector3 end;
        private Vector3 turn; // if turning

        private int maxSize;
        private Vector3[] flyingCoors; // actual coordinates of flying locations

        private int[] gridSizeValues = new int[4] { 5, 6, 7, 4 };  // Copied from BBGameManager


        void FixedUpdate() {
            if (active) {
                // fly from start (set in Init) to turn
                // switch by start.z (Dir)
                    // case (int)Dir.Top --> move -y
                    // case (int)Dir.Bot --> move +y
                    // case (int)Dir.Left --> move +x
                    // case (int)Dir.Right --> move -x
                // if at turn --> BattieAnimator.SetInteger("MoveType", (int)turn.z);
                // switch by turn.z (Dir)
                    // case (int)Dir.Top --> move -y
                    // case (int)Dir.Bot --> move +y
                    // case (int)Dir.Left --> move +x
                    // case (int)Dir.Right --> move -x
                // if at end --> EndFlyingAnimation();
            }
        }


        public void AppendFlyingCoors(Vector3Int orig, Dir origDir, Vector3Int dest, Dir destDir) {    
            Vector3Int[] pair = new Vector3Int[] {orig, dest};
            Dir[] dirPair = new Dir[] {origDir, destDir};

            size = BBEvents.GetLevel.Invoke().gridSize;
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
            start = (Vector3)pair[0];
            end = (Vector3)pair[1];
            turn = new Vector3(start.x, end.y, 0); 
            

            // Change coordinates by type of hit
            if (start.x==end.x && start.y==end.y) { // Direct Hit
                switch(start.z) {
                    case 1: // Left
                        turn.x = 0.5f; turn.z = 3;         
                        break;
                    case 2: // Bottom
                        turn.y = 0.5f; turn.z = 4;     
                        break;  
                    case 3: // Right
                        turn.x = maxSize-1.5f; turn.z = 1;
                        break;
                    case 4: // Top
                        turn.y = maxSize-1.5f; turn.z = 2;
                        break;  
                }
            }
            else if (start.x==end.x || start.y==end.y) { // Miss
                turn.x = (start.x+end.x)/2;
                turn.y = (start.y+end.y)/2;
                turn.z = start.z;
            } 
            else { // Detour
                switch (end.z) {
                    case 1:     turn.z = 3;     break;  // Left
                    case 2:     turn.z = 4;     break;  // Bottom
                    case 3:     turn.z = 1;     break;  // Right
                    case 4:     turn.z = 2;     break;  // Top
                }
                if (start.x==-1 || start.x==maxSize) { // Invert line shape
                    turn.x = end.x; turn.y = start.y; 
                }
            }

            Vector3[] positions = new Vector3[] {start, turn, end};

            for (int i=0; i<3; i++) {
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

            flyingCoors = positions;
        }


        public void InitFlyingAnimation() {
            BattieContainer.transform.localPosition += flyingCoors[0];
            BattieAnimator.SetInteger("MoveType", (int)start.z);
            active = true;
        }

        public void EndFlyingAnimation() {
            active = false;
            BattieAnimator.SetInteger("MoveType", 0);
            BattieContainer.transform.localPosition -= BattieContainer.transform.localPosition; //return Battie to (0,0)
            
        }


    }
}