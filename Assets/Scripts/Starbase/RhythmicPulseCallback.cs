using System.Collections.Generic;
using Game.Audio;
using UnityEngine;
using FMODUnity;
using System.Collections;
using FMOD;

namespace Game.Starbase
{
    public class RhythmicPulseCallback : MonoBehaviour
    {
        public StarbaseController SbController;
        public AudioManager AudioManager;

        [field: Header("Current Pulse Type.")]
        [field: Tooltip("0 is a Rhythmic Pulse, 1 is a Weak Beat.")]
        [field: SerializeField]
        [field: Range(0, 1)]
        public float PulseType { get; private set; }

        void Start()
        {
            SbController = GetComponentInParent<StarbaseController>();


            StartCoroutine(ChangeTimer());
        }

        void Update()
        {
            PulseType = AudioManager.GetMusicParameter("PulseType");
        }
        IEnumerator ChangeTimer()
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            RandomMusicChanger();
            StartCoroutine(ChangeTimer());
        }
        public void RandomMusicChanger()
        {
            SbController.musicChangeValue = Random.Range(0, 6);
        }
    }
}
