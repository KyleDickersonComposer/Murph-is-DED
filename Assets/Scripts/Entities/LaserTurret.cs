using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

namespace Game.Entities
{
    public class LaserTurret : MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField]
        private GameObject LaserBeamProjectile;

        [Header("Projectile Speed")]
        [SerializeField]
        private float projectileSpeed = 1f;

        private void OnEnable()
        {
            GameObject Projectile = Instantiate(LaserBeamProjectile);
        }
    }
}
