using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    [CreateAssetMenu(menuName = "Create AllSpells", fileName = "AllSpells", order = 0)]
    public class AllSpells : SerializedScriptableObject
    {
        [SerializeField] private SpellConfiguration[] _spellConfigurations;

        public SpellConfiguration this[TentacleSpell spellId]
        {
            get { return _spellConfigurations.First(config => config.SpellId == spellId); }
        }
    }
}