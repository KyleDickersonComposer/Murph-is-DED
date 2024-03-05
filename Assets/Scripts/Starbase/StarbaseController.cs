using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StarbaseController : MonoBehaviour
    {

        public static StarbaseController Instance { get; private set; }
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
