using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Effects
{
    
    public class WeaponParticleTrailEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _trails;
     

        public void Play(int index)
        {
            _trails[index].Play(true);
        }
    }
}