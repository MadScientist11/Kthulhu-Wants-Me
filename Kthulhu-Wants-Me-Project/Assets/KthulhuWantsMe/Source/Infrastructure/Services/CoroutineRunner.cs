using System.Collections;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartRoutine(IEnumerator routine);
    }

    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine StartRoutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
                
    }
}