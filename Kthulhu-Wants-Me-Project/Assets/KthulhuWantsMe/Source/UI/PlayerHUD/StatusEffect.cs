using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class StatusEffect : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init(EffectData effectData)
        {
            _icon.sprite = effectData.Icon;
        }
    }
}