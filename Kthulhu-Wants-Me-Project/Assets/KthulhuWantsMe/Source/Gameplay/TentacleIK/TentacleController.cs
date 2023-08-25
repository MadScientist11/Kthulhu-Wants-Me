using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class TentacleController : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;

        public Transform IKTarget;
        public ChainIKConstraint ChainIK;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            ChainIK.weight = 0;
        }

        private void Update()
        {
            

            if (Vector3.Distance(_gameFactory.Player.transform.position, transform.position) < 15f)
            {
                ChainIK.weight = 1;
                IKTarget.position = _gameFactory.Player.transform.position;
                if (Vector3.Distance(_gameFactory.Player.transform.position, transform.position) < 8f)
                {
                   
                    ChainIK.weight = 0;
                    IKTarget.position = _gameFactory.Player.transform.position;
                    _gameFactory.Player.transform.SetParent(_renderer.transform);
                    float radius = _gameFactory.Player.transform.localScale.x - 1f;   
                    _renderer.material.SetFloat("_Radius", radius);
                    _renderer.material.SetInt("_GrabPlayer", 1);
                    _renderer.material.SetVector("_InteractPos", _gameFactory.Player.transform.localPosition+ _offset );
                }
                else
                {
                    _renderer.material.SetInt("_GrabPlayer", 0);
                }
            }
            
        }
    }
}
