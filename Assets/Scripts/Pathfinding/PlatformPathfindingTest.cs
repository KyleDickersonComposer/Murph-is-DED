using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class PlatformPathfindingTest : MonoBehaviour
    {
        [SerializeField] private PlatformPathfindingManager manager;
        [SerializeField]private Camera lookCamera;

        private List<Node> _path;
        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 destination = lookCamera.ScreenToWorldPoint(Input.mousePosition);

                if(manager.FindPath(transform.position, destination, out _path))
                {
                    manager.DisplayPath(_path, 5.0f);
                }
            }
        }
    }
}
