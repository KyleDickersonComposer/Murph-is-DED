using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Audio;

namespace Game.Starbase
{
    public class StarbaseController : GenericSingleton<StarbaseController>
    {
        [Header("Change the BPM and Meter to hear a different Rhythmic Pulse")]
        [Range(0, 6)]
        [SerializeField]
        public int musicChangeValue = 0;
        public AudioManager manager;
        public void Start()
        {
        }
        public void Update()
        {
            manager.SetMusicParameter("MusicChange", musicChangeValue);
        }
    }
}
