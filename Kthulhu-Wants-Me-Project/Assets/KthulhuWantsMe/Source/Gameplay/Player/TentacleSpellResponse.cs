using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class TentacleSpellResponse : MonoBehaviour, IAbilityResponse<TentacleSpellCastingAbility>
    {
        public Dictionary<TentacleSpell, ITentacleSpell> ActivePlayerDebuffs = new();


        public void RespondTo(TentacleSpellCastingAbility ability)
        {
            
        }

        public bool IsActiveDebuff(TentacleSpell tentacleSpell)
        {
           return ActivePlayerDebuffs.ContainsKey(tentacleSpell);
        }
    }
}