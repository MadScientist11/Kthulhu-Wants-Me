using System.Collections;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public class CoroutineRunner : MonoBehaviour
    {
        public Coroutine StartRoutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
                
    }
}