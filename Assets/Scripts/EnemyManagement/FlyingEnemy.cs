using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Pathfinding;

namespace Game.EnemyManagement
{
    public class FlyingEnemy : Enemy
    {
        [SerializeField]private GridPathfindingManger gridPathfinding;
        
        [Min(1.0f)]
        [SerializeField]private float moveSpeed = 10f;
        private List<GridNode> _currentPath = new List<GridNode>();
        private int _currentPathIndex = 0;

        private Vector2 _tempPosition;
        protected override IEnumerator UpdatePath()
        {
            yield return new WaitUntil(()=> base.Health.CurrentAmount != 0);
            while(base.Health.CurrentAmount > 0)
            {
                Debug.Log("Trying to update path..");
                Vector2 targetPosition = base.Target.position + base.targetOffset;
                if(gridPathfinding.FindPath(transform.position, targetPosition, out _currentPath))
                {
                    _currentPathIndex = 0;
                }
                else
                {
                    Debug.Log("Path not possible from " + transform.position + " to " + targetPosition);
                }
                yield return new WaitForSeconds(base.PathUpdateTime);
            }
        }
        private void Start() {
            base.EnemyRB.isKinematic = true;
            StartCoroutine(UpdatePath());
        }

        private void FixedUpdate() 
        {
            float squareDistToTarget = Vector2.SqrMagnitude(base.Target.position -  transform.position);
            if(_currentPathIndex >= _currentPath.Count || _currentPathIndex < 0 || squareDistToTarget < stopDistance * stopDistance)
            {
                return;
            }

            Vector2 wayPtPos = gridPathfinding.GetWorldPosition(_currentPath[_currentPathIndex].PositionInGrid);
            Vector2 direction = (wayPtPos - base.EnemyRB.position).normalized;
            Vector2 movePosition = base.EnemyRB.position + direction * moveSpeed * Time.fixedDeltaTime;
            
            base.EnemyRB.MovePosition(movePosition);

            float squareDistToWayPt = Vector2.SqrMagnitude(wayPtPos - base.EnemyRB.position);
            if(squareDistToWayPt < waypointChangeDistance * waypointChangeDistance)
            {
                _currentPathIndex++;
            }
        }
    }
}
