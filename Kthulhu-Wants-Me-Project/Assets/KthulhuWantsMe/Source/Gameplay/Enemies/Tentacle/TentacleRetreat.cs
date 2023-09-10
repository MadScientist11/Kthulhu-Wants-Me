using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleRetreat : MonoBehaviour
    {
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleEmergence _tentacleEmergence;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;
        [SerializeField] private TentacleHealth _tentacleHealth;
        [SerializeField] private TentacleSpellCastingAbility _tentacleSpellCastingAbility;

        private Portal _boundPortal;

        private ILootService _lootService;

        [Inject]
        public void Construct(ILootService lootService)
        {
            _lootService = lootService;
        }

        public void Init(Portal boundPortal)
        {
            _boundPortal = boundPortal;
        }

        private void Start()
        {
            _tentacleHealth.Died += Retreat;
        }

        private void OnDestroy()
        {
            _tentacleHealth.Died -= Retreat;
        }

        private void Retreat()
        {
            _tentacleAnimator.PlayRetreat();
            _tentacleAIBrain.BlockProcessing = true;
            _tentacleSpellCastingAbility.CancelActiveSpells();
                

            _lootService.SpawnRandomBuff(_tentacleEmergence.InitialPoint + Vector3.up * 5f, Quaternion.identity);
            StartCoroutine(RetreatToPortal(_tentacleEmergence.InitialPoint));
        }

        private IEnumerator RetreatToPortal(Vector3 to)
        {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = to;

            float duration = 4f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _boundPortal.ClosePortal();
        }
    }
}