using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth 
{ 
    public class Player : MonoBehaviour
    {
        public string type;
        public bool current;

        public GameObject player;
        public Transform movepoint;
        
        private Vector3 ploc;
        private Vector3 mx; // player middle of grid
        
        private int deg;
        private float speed = 5f;

        public Animator animator;

        private GameBehavior GB;

        public Player() { 
        }

        public void initPlayer(int sign, int degree) {
            GB = GameObject.Find("GameManagerLocal").GetComponent<GameBehavior>();
            // int px = (int)((GB.size-(GB.size/4))/2);
            // int py = px;
            int px = 0;
            int py = GB.size-1;
            
            ploc = new Vector3(px, py, 0);
            // mx = genMx(sign, degree, new Vector3(sign*(px+1),(int)(-py)/2,0));
            mx = startPosition(sign, degree, GB.size);

            player.transform.position += mx;
            movepoint.transform.position += mx;

            deg = degree;
        }

        /* public Vector3 genMx(int sign, int degree, Vector3 mx) {
            Vector3 middle = mx + new Vector3(2,1,0);
            if (GB.size%2 != 0) {
                if (sign < 0) { 
                    return mx; 
                }
                if (degree == 180) {
                    return (middle + new Vector3((-GB.size+(GB.size/3))/2,(GB.size+(GB.size/2))/2,0)); //Vector3(-GB.size/2,(GB.size+1)/2,0));
                }
                else if (degree == 90) {
                    return (middle + new Vector3((GB.size+(GB.size/6))/2,(GB.size+(GB.size/2))/2,0)); //Vector3(GB.size/2,(GB.size+1)/2,0));
                }
                else {
                    return (mx + new Vector3(GB.size-1,0,0));
                }
            }
            else if (GB.size%2 == 0) {
                if (sign < 0) {
                    return (mx); //+ new Vector3(0,1,0));
                }
                if (degree == 180) {
                    return (middle + new Vector3((-GB.size+(GB.size/2))/2,(GB.size-1+(GB.size/2))/2,0));
                }
                else if (degree == 90) {
                    return (middle + new Vector3((GB.size+(GB.size/3))/2,(GB.size-1+(GB.size/2))/2,0));
                }
                else {
                    return (middle + new Vector3(GB.size-2,-1,0));
                }
            }
            return mx;
        } */

        public Vector3 startPosition(int sign, int degree, int size) {
            Vector3 start = new Vector3(0,0,0);
            if (size < 5) {
                    start += new Vector3(-1, -1, 0);
            } else {
                    start += (new Vector3(-1, -2, 0));
            }
            if (sign == -1) { return start; }
                
            switch(size) {
                case 3:
                    if (degree == 180) { return (start += new Vector3(2,2,0)); }
                    else if (degree == 90) { return (start += new Vector3(4,2,0)); }
                    else { return (start += new Vector3(4,0,0)); }
                    break;
                case 4:
                    if (degree == 180) { return (start += new Vector3(3,3,0)); }
                    else if (degree == 90) { return (start += new Vector3(6,3,0)); }
                    else { return (start += new Vector3(6,0,0)); }
                    break;
                case 5:
                    if (degree == 180) { return (start += new Vector3(2,4,0)); }
                    else if (degree == 90) { return (start += new Vector3(6,4,0)); }
                    else { return (start += new Vector3(6,0,0)); }
                    break;
                case 6:
                    if (degree == 180) { return (start += new Vector3(3,5,0)); }
                    else if (degree == 90) { return (start += new Vector3(8,5,0)); }
                    else { return (start += new Vector3(8,0,0)); }
                    break;
                default:
                    return start;
            }
        }

        public void returnPlayer() {
            Vector3 moveBack = mx - player.transform.position;

            movepoint.transform.position += moveBack;
            player.transform.position += moveBack;
            
            // ploc += moveBack;
            ploc = new Vector3(0, GB.size-1, 0);
        }

        public string getType { //either "main" or "mirror"
            get { return type; }
        }

        public int px {
            get { return (int)ploc.x; }
        }

        public int py {
            get { return (int)ploc.y; }
        }

        public Vector3 getPloc {
            get { return ploc; }
        }

        public string getTilePosition {
            get { return $"{ploc.x}, {ploc.y}"; }
        }

        public Vector3 getActualPosition {
            get { return movepoint.position; }
        }

        /* public void setDeg(int degree) {
            deg = degree;
        } */

        public void toggleType() {
            if (type == "main") {
                type = "mirror";
                current = false;
            }
            else if (type == "mirror") {
                type = "main";
                current = true;
            }
        }

        // optional stuff to make the update function less ugly

        public void moveTowards() {
            player.transform.position = Vector3.MoveTowards(player.transform.position, movepoint.position, speed*Time.deltaTime);
        }

        public float distBetween() {
            return Vector3.Distance(player.transform.position, movepoint.position);
        }

        public void move(Vector3 movement) {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            movepoint.position += movement;

            if (deg == 180) {
                ploc += new Vector3(movement.x, movement.y, 0);
            }
            else if (deg == 90) {
                ploc += new Vector3(-1*movement.y, movement.x, 0);
            }
            else { //if (deg == 0)
                ploc += movement * -1;
            }
        }


    }
}