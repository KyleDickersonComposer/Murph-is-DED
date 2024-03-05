using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;


namespace Game.Pathfinding
{
    public class GridPathfindingManger : MonoBehaviour
    {
        private const int DiagonalCost = 14;
        private const int StraightCost = 10;

        [Min(1)]
        [SerializeField]private int width = 20;
        [Min(1)]
        [SerializeField]private int height = 20;

        [Min(0.5f)]
        [SerializeField]private float cellSize = 0.5f;
        [SerializeField]private HeuristicType heuristicType;
        [Min(1000)]
        [SerializeField]private int calculatorPatience = 1000;
        [SerializeField]private LayerMask obstacleMask;
        private Pathfinder<GridNode> _pathfinder;
        private GridNode[,] _grid;

        private float CalculateHeuristicCost(GridNode first, GridNode second)
        {
            switch(heuristicType)
            {
                case HeuristicType.Manhattan: return ManhattanHeuristic(first, second);

                case HeuristicType.Euclidean: return EuclideanHeuristic(first, second);
            }

            return 0.0f;
        }

        private float ManhattanHeuristic(GridNode first, GridNode second)
        {
            int xDistance = Mathf.Abs(Mathf.RoundToInt(first.PositionInGrid.x - second.PositionInGrid.x));
            int yDistance = Mathf.Abs(Mathf.RoundToInt(first.PositionInGrid.y - second.PositionInGrid.y));

            int remainingDistance = Mathf.Abs(xDistance - yDistance);
            return Mathf.Min(xDistance,yDistance) * DiagonalCost +  remainingDistance * StraightCost;
        }

        private float EuclideanHeuristic(GridNode first, GridNode second)
        {
            return Vector2.SqrMagnitude(first.PositionInGrid - second.PositionInGrid);
        }

        public Vector2 GetWorldPosition(Vector2Int posInGrid)
        {
            return GetWorldPosition(posInGrid.x,posInGrid.y);
        }
        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(transform.position.x, transform.position.y) + new Vector2(x,y) * cellSize;
        }

        private void DrawSquare(Vector2 worldPosition,float squareSize,Color squareColor)
        {
            //Taking points clockwise.
            Vector2 topRightPosition = new Vector2(worldPosition.x + squareSize/2f,worldPosition.y + squareSize/2f);

            Vector2 bottomRightPosition = new Vector2(worldPosition.x + squareSize/2f,worldPosition.y - squareSize/2f);

            Vector2 bottomLeftPosition = new Vector2(worldPosition.x - squareSize/2f,worldPosition.y - squareSize/2f);

            Vector2 topLeftPosition = new Vector2(worldPosition.x - squareSize/2f,worldPosition.y + squareSize/2f);

            Gizmos.color = squareColor;
            Gizmos.DrawLine(topRightPosition,bottomRightPosition);
            Gizmos.DrawLine(bottomRightPosition,bottomLeftPosition);
            Gizmos.DrawLine(bottomLeftPosition,topLeftPosition);
            Gizmos.DrawLine(topLeftPosition,topRightPosition);

        }

        private void CheckNeighbour(int neigbourX, int neighbourY,GridNode originalNode, ref Dictionary<GridNode, float> neighbourList)
        {
            GridNode neighbourNode = _grid[neigbourX, neighbourY];
            Vector2 worldPosition = GetWorldPosition(neigbourX, neighbourY);
            if(Physics2D.OverlapCircle(worldPosition, cellSize, obstacleMask.value) == null)
            {
                neighbourList.Add(neighbourNode, CalculateHeuristicCost(originalNode, neighbourNode));
            }
        }

        private Dictionary<GridNode,float> GetNeighbours(GridNode node)
        {
            Vector2Int posInGrid = node.PositionInGrid;

            Dictionary<GridNode,float> neighbourList = new Dictionary<GridNode,float>();

            //Left
            if(posInGrid.x - 1 >= 0)
            {
                neighbourList.Add(_grid[posInGrid.x - 1, posInGrid.y], CalculateHeuristicCost(node, _grid[posInGrid.x - 1, posInGrid.y]));
                //Down left
                if(posInGrid.y - 1 >= 0)
                {
                    neighbourList.Add(_grid[posInGrid.x - 1,posInGrid.y - 1], CalculateHeuristicCost(node, _grid[posInGrid.x - 1,posInGrid.y - 1]));
                }
                //Up Left.
                if(posInGrid.y + 1 < height)
                {
                    neighbourList.Add(_grid[posInGrid.x - 1,posInGrid.y + 1], CalculateHeuristicCost(node, _grid[posInGrid.x - 1,posInGrid.y + 1]));
                }
            }
            //Right
            if(posInGrid.x + 1 < width)
            {
                neighbourList.Add(_grid[posInGrid.x + 1, posInGrid.y], CalculateHeuristicCost(node, _grid[posInGrid.x + 1, posInGrid.y]));
                //Down Right
                if(posInGrid.y - 1 >= 0)
                {
                    neighbourList.Add(_grid[posInGrid.x + 1,posInGrid.y - 1], CalculateHeuristicCost(node, _grid[posInGrid.x + 1,posInGrid.y - 1]));
                }
                //Up Right.
                if(posInGrid.y + 1 < height)
                {
                    neighbourList.Add(_grid[posInGrid.x + 1,posInGrid.y + 1], CalculateHeuristicCost(node, _grid[posInGrid.x + 1,posInGrid.y + 1]));
                }
            }
            //Up
            if(posInGrid.y - 1 >= 0)
            {
                neighbourList.Add(_grid[posInGrid.x,posInGrid.y - 1], CalculateHeuristicCost(node, _grid[posInGrid.x,posInGrid.y - 1]));
            }
            //Down
            if(posInGrid.y + 1 < height)
            {
                neighbourList.Add(_grid[posInGrid.x,posInGrid.y + 1], CalculateHeuristicCost(node, _grid[posInGrid.x,posInGrid.y + 1]));
            }

            return neighbourList;
        }

        private void Awake() {
            _pathfinder = new Pathfinder<GridNode>(CalculateHeuristicCost, GetNeighbours, calculatorPatience);
        }        
        private void OnDrawGizmosSelected() 
        {
            //For draw grid border.
            
            Vector3 origin = transform.position;

            Vector3 bottomLeftCorner = origin;
            Vector3 topLeftCorner = origin + Vector3.up * height * cellSize;
            Vector3 topRightCorner = origin + (Vector3.up * height + Vector3.right * width) * cellSize;
            Vector3 bottomRightCorner = origin + Vector3.right * width * cellSize;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(bottomLeftCorner, topLeftCorner);
            Gizmos.DrawLine(topLeftCorner, topRightCorner);
            Gizmos.DrawLine(topRightCorner, bottomRightCorner);
            Gizmos.DrawLine(bottomRightCorner, bottomLeftCorner);

            //For drawing cells.
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Vector2 cellWorldPosition = GetWorldPosition(x,y) + Vector2.one * cellSize/2f;
                    if(_grid != null && !_grid[x,y].Walkable)
                    {
                        DrawSquare(cellWorldPosition,cellSize - 0.1f,Color.red);
                    }
                    else
                    {
                        DrawSquare(cellWorldPosition,cellSize - 0.1f,Color.white);
                    }

                }
            }

        }
    }
}
