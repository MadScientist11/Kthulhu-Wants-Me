using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public class TentacleSettings
    {
        public float MaxHealth;
        public float BaseDamage;
        public float TentacleGrabDamage;

        public bool AllowSpellCasting = true;
    }
    public interface IRuntimeData : IInitializableService
    {
        TentacleSettings TentacleSettings { get; }
    }

    public class RuntimeData : IRuntimeData
    {
        public bool IsInitialized { get; set; }
        
        public TentacleSettings TentacleSettings => _tentacleSettings;

        private TentacleSettings _tentacleSettings;
        
        private readonly IDataProvider _dataProvider;

        public RuntimeData(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public UniTask Initialize()
        {
            _tentacleSettings = _dataProvider.TentacleConfig.AsRuntimeSettings();
            return UniTask.CompletedTask;
        }
    }
}