using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class PlayerUI : MonoBehaviour
    {
        private UIDocument document;
        private void Awake()
        {
            document = GetComponentInParent<UIDocument>();
        }
        void Start()
        {
            VisualElement backgroundUIElement = document.rootVisualElement.Q<VisualElement>("BackgroundImage");
        }
    }
}
