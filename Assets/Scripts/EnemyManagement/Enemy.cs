using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.EnemyManagement
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]private Transform target;

        [Min(0.5f)]
        [SerializeField]private float pathUpdateTime = 1.0f;

        [Min(0.5f)]
        [SerializeField]protected float stopDistance = 1.0f;

        [SerializeField]protected float waypointChangeDistance = 0.5f;

        [SerializeField]protected Vector3 targetOffset = Vector3.zero; 
        private Rigidbody2D _enemyRB;
        private Health _health;

        public Rigidbody2D EnemyRB { get => _enemyRB; }
        public Health Health { get => _health; }
        public Transform Target { get => target; set => target = value; }
        protected float PathUpdateTime { get => pathUpdateTime; }

        protected abstract IEnumerator UpdatePath();
        
        private void Awake()
        {
            if(!TryGetComponent<Rigidbody2D>(out _enemyRB))
            {
                _enemyRB = gameObject.AddComponent<Rigidbody2D>();
            }

            if(!TryGetComponent<Health>(out _health))
            {
                _health = gameObject.AddComponent<Health>();
            }
        }

        
    }
}
