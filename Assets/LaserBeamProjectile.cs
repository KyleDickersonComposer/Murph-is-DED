using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LaserBeamProjectile : MonoBehaviour
    {
        [Header("Projectile Speed")]
        [SerializeField]
        private float projectileSpeed = 1f;

        private SpriteRenderer spriteRenderer;



        void Start()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }
        void Update()
        {
            transform.Translate(projectileSpeed * Time.deltaTime * Vector2.right);
        }
    }
}
