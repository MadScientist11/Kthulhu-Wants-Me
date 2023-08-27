using System;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class PickableItem : MonoBehaviour, IPickable
    {
        public Transform Transform => transform;
        public bool Equipped { get; set; }

        private PlayerFacade _player;
        protected IInventorySystem _inventorySystem;
        protected IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory, IInventorySystem inventorySystem)
        {
            _gameFactory = gameFactory;
            _inventorySystem = inventorySystem;
            gameFactory.OnPlayerInitialized += GetPlayer;
        }

        private void OnDestroy()
        {
            _gameFactory.OnPlayerInitialized -= GetPlayer;
        }

        public bool Interact()
        {
            if (!Equipped)
            {
                _inventorySystem.ReplaceItem(this);
                return true;
            }

            return false;
        }

        public virtual void PickUp()
        {
            _player.PlayerActions.PickUp(this);
            Equipped = true;
        }

        public virtual void ThrowAway()
        {
            _player.PlayerActions.ThrowAway(this);
            Equipped = false;
        }

        private void GetPlayer(PlayerFacade player) =>
            _player = player;
    }
}