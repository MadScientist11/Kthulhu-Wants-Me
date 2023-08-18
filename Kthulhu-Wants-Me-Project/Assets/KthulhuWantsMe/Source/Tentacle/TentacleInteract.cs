using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Tentacle
{
    [ExecuteInEditMode]
    public class TentacleInteract : MonoBehaviour {

        public Material material;
        public Transform obj;
        public Vector3 offset;
        public float test;

        private void Start()
        {
        }

        public void Update() {
            if (material == null || obj == null) return;
            float radius = obj.localScale.x - 2f;
            material.SetFloat("_Radius", radius);
            material.SetVector("_InteractPos", transform.InverseTransformPoint(obj.position + offset * radius));
        }
    }
}