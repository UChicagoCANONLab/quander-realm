using UnityEngine;

namespace BlackBox
{
    public class Ray
    {
        public Vector3Int position;
        public Dir direction;
        
        private bool directHit = false;
        private bool rayDetoured = false;

        private Vector3Int origin;
        private Dir originDirection;

        private Vector3Int destination;
        private Dir destDirection;

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
                if (origin == destination)
                {
                    if (directHit)
                    {
                        //Hit
                        BBEvents.MarkUnits?.Invoke("H", originDirection, origin);
                    }
                    else // only other case for returning to the same cell is a reflection
                    {
                        //Reflect
                        BBEvents.MarkUnits?.Invoke("R", originDirection, origin);
                    }
                }
                else // different cell
                {
                    //Detour
                    BBEvents.MarkDetourUnits?.Invoke(originDirection, origin, destDirection, destination);
                }
            }
            else // diff entry/exit direction
            {
                if (rayDetoured)
                {
                    //Detour
                    BBEvents.MarkDetourUnits?.Invoke(originDirection, origin, destDirection, destination);
                }
                else // straight through
                {
                    //Miss
                    BBEvents.MarkUnits?.Invoke("M", originDirection, origin);
                    BBEvents.MarkUnits?.Invoke("M", destDirection, destination);
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

        public void Flip()
        {
            switch(direction)
            {
                case Dir.Left: direction = Dir.Right; break;
                case Dir.Bot: direction = Dir.Top; break;
                case Dir.Right: direction = Dir.Left; break; 
                case Dir.Top: direction = Dir.Bot; break;
            }
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

            rayDetoured = true;
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

            rayDetoured = true;
        }

        public void Kill(int gridLength) //todo: better fix
        {
            direction = originDirection;
            position = origin;
            directHit = true;

            switch(direction)
            {
                case Dir.Right: position.x = gridLength - 1; break;
                case Dir.Top: position.y = gridLength - 1; break;
                default: break;
            }

            Flip();
        }
    }
}
