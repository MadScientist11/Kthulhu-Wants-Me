using System;
using System.Collections;
using KthulhuWantsMe.Source.Utilities;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IRandomService
    {
        void ProvideRandomValue(Action<float> provide, float everySeconds);
    }

    public class RandomService : IRandomService
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public RandomService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void ProvideRandomValue(Action<float> provide, float everySeconds)
        {
            _coroutineRunner.StartRoutine(FireEvery(provide, everySeconds));
        }

        private IEnumerator FireEvery(Action<float> provide, float everySeconds)
        {
            while (true)
            {
                provide?.Invoke(Random.value);
                yield return WaitForSeconds.Wait(everySeconds);
            }
        }
    }
}