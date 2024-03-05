using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Game.Audio
{
    public class FmodEvents : MonoBehaviour
    {

        [field: Header("Ambience")]
        [field: SerializeField] public EventReference Ambience { get; private set; }

        [field: Header("Music")]
        [field: SerializeField] public EventReference Music { get; private set; }

        public static FmodEvents Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else Instance = this;
        }
    }
}
