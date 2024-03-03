using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Game.Pathfinding
{
    [Serializable]
    public class Node : MonoBehaviour
    {
        
        [SerializeField]private List<Node> neighbours = new List<Node>();

        public List<Node> Neighbours { get => neighbours; }

        public void AddNeighbour(Node neighbour)
        {
            neighbours.Add(neighbour);
        }

        public void RemoveNeighbour(Node neighbour)
        {
            if(neighbours.Contains(neighbour))
            {
                neighbours.Remove(neighbour);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.6f);
            Gizmos.color = Color.cyan;
            neighbours.ForEach(n =>
            {
                if(n != null)
                {
                    Gizmos.DrawWireSphere(n.transform.position, 0.5f);
                }
            });
        }

        [ContextMenu("Create Neighbor")]
        void CreateNeighbor()
        {
            Node node = Instantiate(this, transform.position + Vector3.up, Quaternion.identity, transform.parent.transform);
            node.gameObject.name = this.name + "(+)";
            node.neighbours.Clear();
            neighbours.Add(node);
            node.neighbours.Add(this);
            Selection.activeGameObject = node.gameObject;
            //return node;
        }
    }
}
