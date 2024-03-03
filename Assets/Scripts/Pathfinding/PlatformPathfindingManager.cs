using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;
using UnityEditor;
using System.Linq;

namespace Game.Pathfinding
{
    public enum HeuristicType
    {
        Manhattan,
        Euclidean
    }

    

    [SelectionBase]
    public class PlatformPathfindingManager : MonoBehaviour
    {
        private const int DiagonalCost = 14;
        private const int StraightCost = 10;

        //[Header("Pathfinding Settings: ")]
        public HeuristicType heuristicType;
        [Min(100)]
        public int calculatorPatience = 1000;
        public List<Node> allNodes;

        //[Header("Gizmos Settings: ")]

        public bool displayGizmo = true;

        public bool displayNodes = true;
        public bool displayConnections = true;
        public bool allowLogging = true;
        [Min(0.1f)]
        public float nodeSphereSize = 0.5f;
        private Pathfinder<Node> _pathfinder;


        // Start is called before the first frame update
        void Start()
        {
            _pathfinder = new Pathfinder<Node>(CalculateHeuristicCost, GetConnectedNeighbours, calculatorPatience);
        }

        public void DisplayPath(List<Node> path, float duration)
        {
            if(path.Count < 2)
            {
                return;
            }

            if(allowLogging)
            {
                string pathString = "";
                for(int i = 0; i < path.Count; i++)
                {
                    pathString += path[i].name + ", ";
                }
                Debug.Log(pathString);
            }
            for(int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].transform.position, path[i+1].transform.position, Color.magenta, duration);
            }
        }

        public bool FindPath(Vector2 start, Vector2 end, out List<Node> path)
        {
            Node startNode = GetClosestNode(start);
            Node endNode = GetClosestNode(end);
            bool result = _pathfinder.GenerateAstarPath(startNode, endNode, out path);
            if(result)
            {
                path.Insert(0, startNode);
            }
            return result;
        }

        private float CalculateHeuristicCost(Node first, Node second)
        {
            switch(heuristicType)
            {
                case HeuristicType.Manhattan: return ManhattanHeuristic(first, second);

                case HeuristicType.Euclidean: return EuclideanHeuristic(first, second);
            }

            return 0.0f;
        }

        private float ManhattanHeuristic(Node first, Node second)
        {
            int xDistance = Mathf.Abs(Mathf.RoundToInt(first.transform.position.x - second.transform.position.x));
            int yDistance = Mathf.Abs(Mathf.RoundToInt(first.transform.position.y - second.transform.position.y));

            int remainingDistance = Mathf.Abs(xDistance - yDistance);
            return Mathf.Min(xDistance,yDistance) * DiagonalCost +  remainingDistance * StraightCost;
        }

        private float EuclideanHeuristic(Node first, Node second)
        {
            return Vector2.SqrMagnitude(first.transform.position - second.transform.position);
        }

        
        private Dictionary<Node, float> GetConnectedNeighbours(Node node)
        {
            Dictionary<Node,float> result = new Dictionary<Node, float>();
            if(allowLogging)
            {
                Debug.Log("Get neighbours for " + node.transform.name);
            }
            foreach(Node neighbour in node.Neighbours)
            {
                result.Add(neighbour, CalculateHeuristicCost(node, neighbour));
                
            }
            return result;
        }

        private Node GetClosestNode(Vector2 target)
        {
            Node result = null;
            float closestSqrDistance = float.MaxValue;

            foreach(Node node in allNodes)
            {
                float squareDistance = Vector2.SqrMagnitude((Vector2)node.transform.position - target);
                if(closestSqrDistance > squareDistance)
                {
                    result = node;
                    closestSqrDistance = squareDistance;
                }
            }

            return result;
        }

        [ContextMenu("ReGather Child Nodes")]
        protected void RegatherChildNodes()
        {
            allNodes = GetComponentsInChildren<Node>().ToList();
        }
    }
}
