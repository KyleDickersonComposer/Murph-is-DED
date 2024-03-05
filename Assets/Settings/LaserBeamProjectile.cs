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

        void Start()
        {
            StartCoroutine(DestroyTimer());
        }

        void Update()
        {
            transform.Translate(projectileSpeed * Time.deltaTime * Vector2.right);
        }

        IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}
