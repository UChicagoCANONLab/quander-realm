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

        private Vector3[] flyingCoors; // actual coordinates of flying locations
        private float speed = 175f;
        private int phase = 0;

        private int[] gridSizeValues = new int[4] { 5, 6, 7, 4 };  // Copied from BBGameManager
        private int panelSize = 1250;


        private void OnEnable() 
        {
            BBEvents.AppendFlyingCoordinates += AppendFlyingCoors;
            BBEvents.StartFlyingAnimation += InitFlyingAnimation;
        }
        private void OnDisable() 
        {
            BBEvents.AppendFlyingCoordinates -= AppendFlyingCoors;
            BBEvents.StartFlyingAnimation -= InitFlyingAnimation;
        }

        void Update() {
            if (active) {
                if (phase != 0) {
                    BattieContainer.transform.localPosition = Vector3.MoveTowards(BattieContainer.transform.localPosition,
                        flyingCoors[phase], speed * Time.deltaTime);

                    // If at turn, change animation and direction
                    if (BattieContainer.transform.localPosition == flyingCoors[1]) {
                        BattieAnimator.SetInteger("MoveType", (int)turn.z);
                        phase = 2;
                    } // If at end, end flying animation and return to start
                    else if (BattieContainer.transform.localPosition == flyingCoors[2]) {
                        EndFlyingAnimation();
                    }
                }
            }
        }


        public void InitFlyingAnimation() {
            if (flyingCoors == null) { return; }
            ReturnBattieHelper(); // Return before start

            // Debug.Log($"Coordinates: {string.Join("; ", flyingCoors)}");

            BattieContainer.transform.localPosition += flyingCoors[0];
            BattieAnimator.SetInteger("MoveType", (int)start.z);
            active = true; phase = 1;
        }

        public void EndFlyingAnimation() {
            active = false; phase = 0;
            BattieAnimator.SetInteger("MoveType", 0);
            Invoke("ReturnBattieHelper", 1f);

            flyingCoors = null; // Clear array
        }

        public void ReturnBattieHelper() {
            BattieContainer.transform.localPosition = new Vector3(0,0,0); //return Battie to (0,0)            
        }


        public void AppendFlyingCoors(Vector3Int orig, Dir origDir, Vector3Int dest, Dir destDir) {    
            Vector3Int[] pair = new Vector3Int[] {orig, dest};
            Dir[] dirPair = new Dir[] {origDir, destDir};

            int size = (int)BBEvents.GetLevel.Invoke().gridSize;
            int gridSize = gridSizeValues[(int)size];

            for (int i=0; i<2; i++) {
                pair[i].z = (int)dirPair[i];
                switch(dirPair[i]) {
                    case Dir.Top:   pair[i].y = gridSize;    break;
                    case Dir.Bot:   pair[i].y = -1;         break;
                    case Dir.Left:  pair[i].x = -1;         break;
                    case Dir.Right: pair[i].x = gridSize;    break;
                }
            }
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
                        turn.x = gridSize-1.5f; turn.z = 1;
                        break;
                    case 4: // Top
                        turn.y = gridSize-1.5f; turn.z = 2;
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
                if (start.x==-1 || start.x==gridSize) { // Invert line shape
                    turn.x = end.x; turn.y = start.y; 
                }
            }

            Vector3[] positions = new Vector3[] {start, turn, end};

            for (int i=0; i<3; i++) {
                Vector3 pos = positions[i];
                switch (pos.x) {
                    case -1:        
                        positions[i].x = -(panelSize/2); 
                        break;
                    case var value when value == gridSize:   
                        positions[i].x = (panelSize/2); 
                        break;
                    default:        
                        positions[i].x = pos.x*(panelSize/(gridSize+1)) + (panelSize/(gridSize+1)) - (panelSize/2);
                        break;
                } 
                switch (pos.y) {
                    case -1:        
                        positions[i].y = -(panelSize/2); 
                        break;
                    case var value when value == gridSize:   
                        positions[i].y = (panelSize/2); 
                        break;
                    default:        
                        positions[i].y = pos.y*(panelSize/(gridSize+1)) + (panelSize/(gridSize+1)) - (panelSize/2);
                        break;
                } 
                positions[i].z=0;
            }

            flyingCoors = positions;
        }


    }
}