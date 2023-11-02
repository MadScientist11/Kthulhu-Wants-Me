using System;
using System.Linq;
using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        [field: SerializeField] public PlayerLocomotion PlayerLocomotion { get; private set; }
        [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }
        [field: SerializeField] public TentacleGrabAbilityResponse TentacleGrabAbilityResponse { get; private set; }
        [field: SerializeField] public PlayerInteractionAbility PlayerInteractionAbility { get; private set; }
        [field: SerializeField] public TentacleSpellResponse TentacleSpellResponse { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

        private IUpgradeService _upgradeService;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IUpgradeService upgradeService, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _upgradeService = upgradeService;
        }

        private void Start()
        {
            if (_dataProvider.PlayerConfig.UnlockAllSkills)
            {
                foreach (SkillId id in Enum.GetValues(typeof(SkillId)).Cast<SkillId>())
                {
                    _upgradeService.ApplyUpgrade(new UpgradeData()
                    {
                        UpgradeType = UpgradeType.SkillAcquirement,
                        SkillId = id,
                    });
                }
            }
        }

        public CinemachineCameraPanning GetCameraPanningLogic() =>
            PlayerVirtualCamera.GetComponent<CinemachineCameraPanning>();


        public void ChangePlayerLayer(int layer)
        {
            gameObject.layer = layer;
            PlayerLocomotion.MovementController.RebuildMovementCollisionsLayerMask();
        }
    }
}