using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
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
        public void Construct(IObjectResolver resolver, IUIFactory uiFactory, IInputService inputService)
        {
            _inputService = inputService;
            _uiFactory = uiFactory;
            _resolver = resolver;
        }

        private  void Start()
        {
            Scene startUp = SceneManager.GetSceneByPath(GameConstants.Scenes.StartUpPath);
            SceneManager.MoveGameObjectToScene(gameObject, startUp);
            _console = _uiFactory.CreateConsoleUI();

            _inputService.GameScenario.ToggleConsole += ToggleConsole;
        }

        private void OnDestroy()
        {
            _inputService.GameScenario.ToggleConsole -= ToggleConsole;
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
                var aliveEnemy = waveSystemDirector.CurrentWaveState.AliveEnemies[i];
                aliveEnemy.TakeDamage(10000000);
            }
        }
    }
}