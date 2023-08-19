using Debug = UnityEngine.Debug;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ILoggerService
    {
        void Log(string message);
    }

    public class LoggerService : ILoggerService
    {
        public void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }
    }
}