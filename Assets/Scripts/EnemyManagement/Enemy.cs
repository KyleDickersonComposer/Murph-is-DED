using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.EnemyManagement
{
    public abstract class Enemy : MonoBehaviour
    {
        private Rigidbody2D _enemyRB;
        private Health _health;

        public Rigidbody2D EnemyRB { get => _enemyRB; }
        public Health Health { get => _health; }

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
