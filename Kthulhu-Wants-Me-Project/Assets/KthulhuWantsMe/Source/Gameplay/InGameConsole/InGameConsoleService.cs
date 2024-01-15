using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using QFSW.QC;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.InGameConsole
{
    public class InGameConsoleService : MonoBehaviour
    {
        private QuantumConsole _console;
        
        private IObjectResolver _resolver;
        private IUIFactory _uiFactory;
        private IInputService _inputService;

        [Inject]
        public void Construct(IUIFactory uiFactory, IInputService inputService)
        {
            _inputService = inputService;
            _uiFactory = uiFactory;
        }

        private void Start()
        {
            if (!Debug.isDebugBuild)
                return;
            
            Scene startUp = SceneManager.GetSceneByPath(GameConstants.Scenes.StartUpPath);
            SceneManager.MoveGameObjectToScene(gameObject, startUp);
            _console = _uiFactory.CreateConsoleUI();

            _inputService.GameScenario.ToggleConsole += ToggleConsole;
        }

        private void OnDestroy()
        {
            _inputService.GameScenario.ToggleConsole -= ToggleConsole;
        }

        public void Enqueue(IObjectResolver objectResolver)
        {
            _resolver = objectResolver;
        }

        private void ToggleConsole()
        {
            if (_console.IsActive)
            {
                _console.Deactivate();
            }
            else
            {
                _console.Activate();
            }
        }

        [Command("heal", MonoTargetType.All)]
        private void Heal(int amount)
        {
            ThePlayer player = _resolver.Resolve<ThePlayer>();
            player.Heal(amount);
        }

        [Command("unlock-skill", MonoTargetType.All)]
        private void UnlockSkill(SkillId skillId)
        {
            IUpgradeService upgradeService = _resolver.Resolve<IUpgradeService>();

            upgradeService.ApplyUpgrade(new UpgradeData()
            {
                UpgradeType = UpgradeType.SkillAcquirement,
                SkillId = skillId,
            });
        }
        
        [Command("kill-all", MonoTargetType.All)]
        private void KillAll()
        {
            IWaveSystemDirector waveSystemDirector = _resolver.Resolve<IWaveSystemDirector>();

            for (int i = waveSystemDirector.CurrentWaveState.AliveEnemies.Count - 1; i >= 0; i--)
            {
                Health aliveEnemy = waveSystemDirector.CurrentWaveState.AliveEnemies[i];
                aliveEnemy.TakeDamage(10000000);
            }
        }

        public enum WaveOutcome
        {
            Victory,
            Loss,
        }
        
        [Command("complete-wave", MonoTargetType.All)]
        private void CompleteWave(WaveOutcome waveOutcome)
        {
            IWaveSystemDirector waveSystemDirector = _resolver.Resolve<IWaveSystemDirector>();
            KillAll();
            if (waveOutcome == WaveOutcome.Victory)
            {
                waveSystemDirector.CompleteWaveAsVictory();
            }
            else
            {
                waveSystemDirector.CompleteWaveAsFailure();
            }
        }
        
        [Command("load-wave", MonoTargetType.All)]
        private void LoadWave(int index)
        {
            IProgressService progressService = _resolver.Resolve<IProgressService>();
            progressService.ProgressData.CompletedWaveIndex = index - 3;
            CompleteWave(WaveOutcome.Loss);
        }
        
        [Command("die", MonoTargetType.All)]
        private void Die()
        {
            ThePlayer player = _resolver.Resolve<ThePlayer>();
            player.TakeDamage(new Damage(1000000000));
        }
    }
}