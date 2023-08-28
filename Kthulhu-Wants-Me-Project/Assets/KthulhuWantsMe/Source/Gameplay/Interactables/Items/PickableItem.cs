using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class PickableItem : MonoBehaviour, IPickable, IInjectable
    {
        [field: SerializeField] public virtual PickableData ItemData { get; set; }
        
        public InteractableData InteractableData => ItemData;
        
        public Transform Transform => transform;
        
        public bool Equipped { get; set; }

        protected IInventorySystem _inventorySystem;
        protected IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory, IInventorySystem inventorySystem)
        {
            _gameFactory = gameFactory;
            _inventorySystem = inventorySystem;
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

        [Button]
        private void CollectPositionAndRotation()
        {
            ItemData.ItemInHandPosition = transform.localPosition;
            ItemData.ItemInHandRotation = transform.localRotation;
            EditorUtility.SetDirty(ItemData);
        }
    }
}