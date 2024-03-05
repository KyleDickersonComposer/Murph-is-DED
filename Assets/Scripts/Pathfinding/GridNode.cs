using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GridNode
    {
        private Vector2Int _positionInGrid;
        private int _gCost;
        private int _hCost;    
        private bool _walkable;
        private GridNode _parentNode;

        public GridNode(Vector2Int positionInGrid,bool walkable,int movementPenalty)
        {
            this._positionInGrid = positionInGrid;
            this._walkable = walkable;
            this._parentNode = null;
        }

        public Vector2Int PositionInGrid { get => _positionInGrid; set => _positionInGrid = value; }
        public int GCost { get => _gCost; set => _gCost = value; }
        public int HCost { get => _hCost; set => _hCost = value; }

        public int FCost { get =>_gCost + _hCost;}
        public GridNode ParentNode { get => _parentNode; set => _parentNode = value; }
        public bool Walkable { get => _walkable; set => this._walkable = value;}
    }
}
