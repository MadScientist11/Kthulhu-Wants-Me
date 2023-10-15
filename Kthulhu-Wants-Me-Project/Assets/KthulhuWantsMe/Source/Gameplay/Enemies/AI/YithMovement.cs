using KthulhuWantsMe.Source.Infrastructure.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    public class YithMovement : MonoBehaviour
    {
        [SerializeField] private MovementMotor _movementMotor;
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void MoveToPlayer()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                AddKnockback();
            }
            _movementMotor.MoveTo(_gameFactory.Player.transform.position);
        }

        [Button]
        private void AddKnockback()
        {
            _movementMotor.AddVelocity(-transform.forward * 10, 1000);
        }
    }
}