using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class GridPathfindingTest : MonoBehaviour
    {
        [SerializeField]private GridPathfindingManger gridPathfinding;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                List<GridNode> nodes = null;
                if(gridPathfinding.FindPath(transform.position, clickedPosition, out nodes))
                {
                    
                    Debug.Log(gridPathfinding.GetWorldPosition(gridPathfinding.GetXY(clickedPosition)));
                    for(int i = 0; i < nodes.Count - 1; i++)
                    {
                        Vector2 nodeAPos = gridPathfinding.GetWorldPosition(nodes[i].PositionInGrid);
                        Vector2 nodeBPos = gridPathfinding.GetWorldPosition(nodes[i+1].PositionInGrid);
                        Debug.DrawLine(nodeAPos, nodeBPos, Color.red, 5.0f);
                    }
                }

            }
        }
    }
}
