﻿using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public interface ITentacleSpell
    {
        bool Active { get; }
        UniTask Cast();
        UniTask Undo();
    }
}