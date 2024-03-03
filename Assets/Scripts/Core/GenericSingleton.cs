using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GenericSingleton<T> : MonoBehaviour where T :  GenericSingleton<T>
    {
        private static T _instance;

        public static T Instance { get => _instance; }

        protected virtual void Awake() {
            if(_instance == null)
            {
                _instance = this as T;
                return;
            }
            Destroy(gameObject);
        }
    }

}
