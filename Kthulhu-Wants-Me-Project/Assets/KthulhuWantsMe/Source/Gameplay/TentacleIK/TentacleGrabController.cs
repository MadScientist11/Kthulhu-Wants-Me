using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    [ExecuteInEditMode]
    public class TentacleGrabController : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;
        

        private Material _tentacleMaterial;
        
        private void Update()
        {                        
            if(_renderer == null || _player == null)
                return;
            
            float radius = _player.localScale.x - 1f;   
            _renderer.material.SetFloat("_Radius", radius);
            _renderer.material.SetVector("_InteractPos", _player.localPosition+ _offset );
        }

      
        
    }
}
