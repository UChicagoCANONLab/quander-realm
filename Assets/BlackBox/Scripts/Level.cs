using UnityEngine;

namespace BlackBox
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level")]
    public class Level : ScriptableObject
    {
        public int level;
        public int module;
        public GridSize gridSize;
        public Vector2Int[] nodePositions;
    }
}
