using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Tentacle
{
    [ExecuteInEditMode]
    public class TentacleInteractTest : MonoBehaviour {

        public Material material;
        public Transform obj;
        public Vector3 offset;
        public float test;
        public float scaleOffset;

        private void Start()
        {
            material.SetInt("_EnableTentacleGrab", 1);

        }

        public void Update() {
            if (material == null || obj == null) return;
            float radius = obj.localScale.x - scaleOffset;
            material.SetFloat("_Radius", radius);
            material.SetVector("_InteractPos", obj.localPosition + offset * radius);
        }
    }
}