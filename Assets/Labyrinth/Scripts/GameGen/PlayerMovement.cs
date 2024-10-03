using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Labyrinth 
{ 
    public class PlayerMovement : MonoBehaviour
    {
        private float speed = 5f;
        private float sign = 1f; //1 for 90deg, -1 for -90deg
        private int deg;
        private int size;
        private string mov = "none";

        // public GameBehavior GB;
        // public Maze MAZE;

        public Player player1;
        public Player player2;


        private void OnEnable() 
        {
            TTEvents.StartPlayerMovement += StartPM;
            TTEvents.SwitchPlayer += SwitchPlayer;
            TTEvents.GetButtonPress += getButtonPress;
            TTEvents.GetPlayer += getPlayer;
            TTEvents.ReturnPlayers += returnPlayers;
        }
        private void OnDisable() 
        {
            TTEvents.StartPlayerMovement -= StartPM;
            TTEvents.SwitchPlayer -= SwitchPlayer;
            TTEvents.GetButtonPress -= getButtonPress;
            TTEvents.GetPlayer -= getPlayer;
            TTEvents.ReturnPlayers -= returnPlayers;
        }


        public void StartPM()
        {
            deg = SaveData.Instance.Degree;
            size = TTEvents.Size.Invoke();

            player1.initPlayer(-1,0, size);
            player2.initPlayer(1,deg, size);
        }

        // Update is called once per frame
        void Update()
        {
            Player main; Player mirror;
            
            if (player1.getType == "main") {
                main = player1;
                mirror = player2;
            }   
            else { // if (player1.getType == "mirror") {
                main = player2;
                mirror = player1;
            }

            main.player.transform.position = Vector3.MoveTowards(main.player.transform.position, main.movepoint.position, speed*Time.deltaTime);
            mirror.player.transform.position = Vector3.MoveTowards(mirror.player.transform.position, mirror.movepoint.position, speed*Time.deltaTime);

            Vector3 buttonMov = getButtonPress(mov);
            if (main.distBetween() <= 0.05f) {
                
                if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) || 
                (buttonMov == new Vector3(-1,0,0)) || 
                (buttonMov == new Vector3(1,0,0))) {
                
                    Vector3 movementx;
                    if (buttonMov != new Vector3(0,0,0)) {
                        movementx = buttonMov;
                    }
                    else {
                        movementx = new Vector3(Input.GetAxisRaw("Horizontal"),0,0);
                    }

                    Vector3 mirrorMovementx = mirroredMovement(deg, sign, movementx, "horizontal");

                    if (!Physics2D.OverlapCircle(main.getActualPosition + movementx/3, 0.25f)) {

                        main.move(movementx);
                        mirror.move(mirrorMovementx);
                        // GB.steps++;
                        TTEvents.IncrementSteps?.Invoke();
                        
                        movementx = new Vector3(0,0,0);
            
                        if ((main.getPloc == new Vector3(size-1, 0, 0)) || 
                        mirror.getPloc == new Vector3(size-1, 0, 0)) {
                            Invoke("goalTime", 0.3f);
                        }
                    }
                    /* else {
                        //Debug.Log("Can't go there!");
                    } */
                }
                if ((Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f || (
                buttonMov == new Vector3(0,-1,0)) || 
                (buttonMov == new Vector3(0,1,0)))) {
                
                    Vector3 movementy;
                    if (buttonMov != new Vector3(0,0,0)) {
                        movementy = buttonMov;
                    }
                    else {
                        movementy = new Vector3(0,Input.GetAxisRaw("Vertical"),0);
                    }
                    Vector3 mirrorMovementy = mirroredMovement(deg, sign, movementy, "vertical");
                    
                    if (!Physics2D.OverlapCircle(main.getActualPosition + movementy/3, .25f)) {
                        
                        main.move(movementy);
                        mirror.move(mirrorMovementy);
                        // GB.steps++;
                        TTEvents.IncrementSteps?.Invoke();

                        movementy = new Vector3(0,0,0);
            
                        if ((main.getPloc == new Vector3(size-1, 0, 0)) || 
                        mirror.getPloc == new Vector3(size-1, 0, 0)) {
                            Invoke("goalTime", 0.3f);
                        }
                    }
                    /* else {
                        //Debug.Log("Can't go there!");
                    } */
                }
            }
            mov = "none";
        }

        public void goalTime() {
            // GB.collectGoal();
            TTEvents.CollectGoal?.Invoke();
        }

        public Vector3 getButtonPress(string dir) {
            mov = dir;
            var movement = new Vector3(0,0,0);
            if (dir=="up") {
                movement = new Vector3(0,1,0);
            }
            else if (dir=="down") {
                movement = new Vector3(0,-1,0);
            }
            else if (dir=="left") {
                movement = new Vector3(-1,0,0);
            }
            else if (dir=="right") {
                movement = new Vector3(1,0,0);
            }
            return movement;
        }

        public Vector3 mirroredMovement(int deg, float sign, Vector3 origMovement, string dir) {
            if (deg == 0) {
                return origMovement;
            }
            else if (deg == 90) {
                if (dir == "vertical") {
                    return (-1* sign * new Vector3(origMovement.y,0,0));
                }
                else if (dir == "horizontal") {
                    return (sign * new Vector3(0,origMovement.x,0));
                }
            }
            else if (deg == 180) {
                return (-1 * origMovement);
            }
            return (new Vector3(0,0,0));
        }

        public void SwitchPlayer() {
            player1.toggleType();
            player2.toggleType();

            TTEvents.GetMap.Invoke(1).switchMaps();
            TTEvents.GetMap.Invoke(2).switchMaps();

            sign *= -1;

            // MAZE.map1.toggleRenderer(MAZE.map1.overlay);
            // MAZE.map1.toggleCollider(MAZE.map1.walls);
            // MAZE.map2.toggleRenderer(MAZE.map2.overlay);
            // MAZE.map2.toggleCollider(MAZE.map2.walls);
        }

        public void returnPlayers() {
            this.player1.returnPlayer();
            this.player2.returnPlayer();
        }

        /* public bool getPlayerCurrent(int i) {
            switch(i) {
                case 1: return player1.current;
                case 2: return player2.current;
                default: return player1.current;
            }
        } */

        /* public Vector3 getPlayerLoc(int i) {
            switch(i) {
                case 1: return player1.getPloc;
                case 2: return player2.getPloc;
                default: return player1.getPloc;
            }
        } */

        public Player getPlayer(int i) {
            if (i == 1) { return this.player1; }
            else        { return this.player2; }
        }

    }
}