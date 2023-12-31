﻿using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.EffectsData
{
    [CreateAssetMenu(menuName = "Create EffectData", fileName = "EffectData", order = 0)]
    public class EffectData : ScriptableObject
    {
        public EffectId EffectId;
        public Sprite Icon;
    }
}