using System.Collections.Generic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public interface IEffectReceiver
    {
        Transform Transform { get; }
        IReadOnlyList<IBuffDebuff> Effects { get; }
    }
}