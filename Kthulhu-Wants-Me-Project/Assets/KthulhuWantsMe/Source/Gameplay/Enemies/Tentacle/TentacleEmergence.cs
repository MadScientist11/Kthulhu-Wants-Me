using System.Collections;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Spell;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleEmergence : MonoBehaviour
    {
        public Vector3 InitialPoint { get; private set; }

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private MinionsSpawnSpell _minionsSpawnSpell;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        private TentacleConfiguration _tentacleConfiguration;
        private TentacleSettings _tentacleSettings;

        [Inject]
        public void Construct(IDataProvider dataProvider, IRuntimeData runtimeData)
        {
            _tentacleSettings = runtimeData.TentacleSettings;
            _tentacleConfiguration = dataProvider.TentacleConfig;
        }

        public void Emerge(Vector3 from, Vector3 to)
        {
            InitialPoint = from;
            _tentacleAIBrain.ResetBrain();
            _tentacleAIBrain.BlockProcessing = true;
            StartCoroutine(EmergeFromPortal(from, to));
            
            if (_tentacleSettings.ActivateSpell)
                StartCoroutine(ActivateSpellAfter(_tentacleConfiguration.SpellActivationTime));
        }

        private IEnumerator ActivateSpellAfter(float delay)
        {
            yield return Utilities.WaitForSeconds.Wait(delay);
            _minionsSpawnSpell.Activate();
        }

        private IEnumerator EmergeFromPortal(Vector3 from, Vector3 to)
        {
            _tentacleAnimator.PlayEmerge();

            Vector3 initialPosition = from;
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

            _tentacleAIBrain.BlockProcessing = false;
            _tentacleAnimator.SetEmerged();
        }
    }
}