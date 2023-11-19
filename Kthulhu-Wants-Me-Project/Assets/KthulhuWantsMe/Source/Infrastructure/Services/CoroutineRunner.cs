using System;
using System.Collections;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartRoutine(IEnumerator routine);
        Coroutine ExecuteAfter(float delay, Action action);
    }

    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine StartRoutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
        
        public Coroutine ExecuteAfter(float delay, Action action)
        {
            return StartRoutine(ExecuteActionAfter(delay, action));
        }

        private IEnumerator ExecuteActionAfter(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
                
    }
}