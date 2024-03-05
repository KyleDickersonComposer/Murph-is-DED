using Game.Starbase;
using UnityEngine;

namespace Game.Entities
{
    public class LaserTurret : MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField]
        private GameObject LaserBeamProjectile;
        private GameObject StarbaseController;
        private RhythmicPulseCallback Callback;
        public FireState fireState;

        public enum FireState
        {
            Ready,
            Fired
        }

        void Awake()
        {
            StarbaseController = GameObject.FindGameObjectWithTag("GameController");
        }
        void Start()
        {
            Callback = StarbaseController.GetComponent<RhythmicPulseCallback>();
            fireState = FireState.Ready;
        }

        void Update()
        {
            Debug.Log(Callback.PulseType.ToString());
            if (Callback.PulseType == 0 && fireState == FireState.Ready)
            {
                Instantiate(LaserBeamProjectile, transform);
                fireState = FireState.Fired;
            }

            if (Callback.PulseType == 1 && fireState == FireState.Fired)
            {
                fireState = FireState.Ready;
            }
        }
    }
}
