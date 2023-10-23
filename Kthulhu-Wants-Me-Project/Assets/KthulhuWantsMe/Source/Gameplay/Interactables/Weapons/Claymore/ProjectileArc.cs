using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore
{
    public class ProjectileArc : MonoBehaviour, IPoolable<ProjectileArc>
    {
        public Action<ProjectileArc> Release { get; set; }

        [SerializeField] private TriggerObserver _projectileHitBoxes;
        
        private ClaymoreSpecialAttack _claymoreSpecialAttack;
        
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Init(ClaymoreSpecialAttack claymoreSpecialAttack)
        {
            _claymoreSpecialAttack = claymoreSpecialAttack;
        }
        
        private void OnEnable()
        {
            _coroutineRunner.ExecuteAfter(2f, ReturnToPool);
            _projectileHitBoxes.TriggerEnter += OnHit;
        }

        private void OnDisable()
        {
            _projectileHitBoxes.TriggerEnter -= OnHit;
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * 10 * Time.deltaTime);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnHit(Collider hit)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                _claymoreSpecialAttack.Apply(damageable);
            }
        }

        private void ReturnToPool()
        {
            Release?.Invoke(this);
        }

        
    }
}