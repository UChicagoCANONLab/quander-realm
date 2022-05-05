using UnityEngine;

namespace BlackBox
{
    public class Ray
    {
        public int numDetours = 0;
        public bool rayTurned = false;
        
        public Vector3Int position;
        public Dir direction;

        public Vector3Int origin;
        public Dir originDirection;
        
        public Vector3Int destination;
        public Dir destDirection;

        public Ray(Vector3Int origin, Dir dir, int width, int height)
        {
            direction = dir;
            originDirection = dir;
            this.origin = origin;

            switch (dir)
            {
                case Dir.Left:
                    position.y = origin.y;
                    break;
                case Dir.Bot:
                    position.x = origin.x;
                    break;
                case Dir.Right:
                    position.x = width - 1;
                    position.y = origin.y;
                    break;
                case Dir.Top:
                    position.y = height - 1;
                    position.x = origin.x;
                    break;
            }
        }

        public void AddMarkers()
        {
            SetDestination();

            if (originDirection == destDirection)
            {
                if (origin != destination)
                {
                    //Detour
                    GameEvents.MarkUnit?.Invoke("D" + numDetours, originDirection, origin);
                    GameEvents.MarkUnit?.Invoke("D" + numDetours, destDirection, destination);
                }
                else
                {
                    if (numDetours == 0)
                    {
                        //Hit
                        GameEvents.MarkUnit?.Invoke("H", originDirection, origin);
                    }
                    else
                    {
                        if (rayTurned)
                        {
                            //Detour
                            GameEvents.MarkUnit?.Invoke("D" + numDetours, destDirection, destination);
                        }
                        else
                        {
                            //Reflect
                            GameEvents.MarkUnit?.Invoke("R", originDirection, origin);
                        }
                    }
                }
            }
            else //originDirection != destDirection
            {
                if (numDetours == 0)
                {
                    //Miss
                    GameEvents.MarkUnit?.Invoke("M", originDirection, origin);
                    GameEvents.MarkUnit?.Invoke("M", destDirection, destination);
                }
                else
                {
                    //Detour
                    GameEvents.MarkUnit?.Invoke("D" + numDetours, originDirection, origin);
                    GameEvents.MarkUnit?.Invoke("D" + numDetours, destDirection, destination);
                }
            }
        }

        private void SetDestination()
        {
            destination = Vector3Int.zero;

            switch (direction)
            {
                case Dir.Left:
                    destination.y = position.y;
                    destDirection = Dir.Right;
                    break;
                case Dir.Right:
                    destination.y = position.y;
                    destDirection = Dir.Left;
                    break;
                case Dir.Bot:
                    destination.x = position.x;
                    destDirection = Dir.Top;
                    break;
                case Dir.Top:
                    destination.x = position.x;
                    destDirection = Dir.Bot;
                    break;
            }
        }

        public void Forward()
        {
            switch (direction)
            {
                case Dir.Left: position.x++; break;
                case Dir.Bot: position.y++; break;
                case Dir.Right: position.x--; break;
                case Dir.Top: position.y--; break;
            }
        }

        public void Flip(bool directHit)
        {
            switch(direction)
            {
                case Dir.Left: direction = Dir.Right; break;
                case Dir.Bot: direction = Dir.Top; break;
                case Dir.Right: direction = Dir.Left; break; 
                case Dir.Top: direction = Dir.Bot; break;
            }

            if (!(directHit))
                numDetours += 2;
        }

        public void TurnLeft()
        {
            switch (direction)
            {
                case Dir.Left: direction = Dir.Bot; break;
                case Dir.Bot: direction = Dir.Right; break;
                case Dir.Right: direction = Dir.Top; break;
                case Dir.Top: direction = Dir.Left; break;
            }

            rayTurned = true;
            numDetours++;
        }

        public void TurnRight()
        {
            switch (direction)
            {
                case Dir.Left: direction = Dir.Top; break;
                case Dir.Bot: direction = Dir.Left; break;
                case Dir.Right: direction = Dir.Bot; break;
                case Dir.Top: direction = Dir.Right; break;
            }

            rayTurned = true;
            numDetours++;
        }

        public void Kill(int gridLength) //todo: better fix
        {
            direction = originDirection;
            position = origin;
            numDetours = 0;

            switch(direction)
            {
                case Dir.Right: position.x = gridLength - 1; break;
                case Dir.Top: position.y = gridLength - 1; break;
                default: break;
            }

            Flip(true);
        }
    }
}
