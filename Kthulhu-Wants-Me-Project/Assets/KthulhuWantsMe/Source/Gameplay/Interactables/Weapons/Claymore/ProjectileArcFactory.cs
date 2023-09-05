using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore
{

    public class ProjectileArcFactory : PooledFactory<ProjectileArc>
    {
        private ProjectileArc _projectilePrefab;
        private ClaymoreSpecialAttack _claymoreSpecialAttack;
        
        private IObjectResolver _instantiator;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IObjectResolver instantiator, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }

        public void Init(ProjectileArc projectilePrefab, ClaymoreSpecialAttack claymoreSpecialAttack)
        {
            _claymoreSpecialAttack = claymoreSpecialAttack;
            _projectilePrefab = projectilePrefab;
        }

        public ProjectileArc GetOrCreateProjectile(Vector3 spawnPoint, Quaternion rotation)
        {
            ProjectileArc projectile = Get(null);
            projectile.transform.position = spawnPoint;
            projectile.transform.rotation = rotation;
            projectile.Show();
            return projectile;
        }

        protected override ProjectileArc Create()
        {
            _projectilePrefab.gameObject.SetActive(false);
            ProjectileArc projectile = _instantiator.Instantiate(_projectilePrefab);
            projectile.Init(_claymoreSpecialAttack);
            return projectile;
        }

        protected override void Release(ProjectileArc portal)
        {
            base.Release(portal);
            portal.Hide();
        }

        protected override ProjectileArc Get(Func<ProjectileArc, bool> predicate)
        {
            ProjectileArc portal = base.Get(predicate);
            return portal;
        }
    }
}