using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class FadeableObject : MonoBehaviour
    {
        public Renderer Renderer;
        private void OnValidate()
        {
            Renderer = GetComponent<Renderer>();
        }
    }
}