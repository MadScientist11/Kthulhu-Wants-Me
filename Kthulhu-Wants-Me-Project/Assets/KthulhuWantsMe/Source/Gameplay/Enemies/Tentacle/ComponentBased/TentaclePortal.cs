using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentaclePortal : MonoBehaviour
    {
        public float _tentacleHeight;

        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        private float _currentY;
        private void Awake()
        {
            _tentacleAIBrain.SwitchOff();
        }

        private void Update()
        {
            if (_tentacleAIBrain.transform.localPosition.y >= 0)
            {
                return;
            }

          
            _tentacleAIBrain.transform.Translate(transform.up *  Time.deltaTime);
        }
    }
}
