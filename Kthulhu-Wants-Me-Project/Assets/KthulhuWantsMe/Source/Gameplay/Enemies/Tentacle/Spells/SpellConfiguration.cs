using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    [CreateAssetMenu(menuName = "Create SpellConfiguration", fileName = "SpellConfiguration", order = 0)]
    public class SpellConfiguration : SerializedScriptableObject
    {
        public TentacleSpell SpellId;
        public GameObject Prefab;
        public float EffectiveRange;
        public float Cooldown;
        
        
    }
}