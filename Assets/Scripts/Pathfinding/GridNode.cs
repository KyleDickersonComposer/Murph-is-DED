using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GridNode
    {
        private Vector2Int _positionInGrid;
        
        public GridNode(Vector2Int positionInGrid)
        {
            this._positionInGrid = positionInGrid;
        }

        public Vector2Int PositionInGrid { get => _positionInGrid; set => _positionInGrid = value; }  
    }
}
