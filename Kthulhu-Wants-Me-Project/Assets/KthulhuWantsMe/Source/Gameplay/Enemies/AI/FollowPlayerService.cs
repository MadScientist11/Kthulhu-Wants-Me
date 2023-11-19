using System.Collections.Generic;
using System.Linq;
using Freya;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    public interface IFollowPlayerService
    {
        Vector3 GetClosestVacantPoint(Transform follower);
        IReadOnlyCollection<PlayerPoint> PointsAroundPlayer { get; }
    }

    public class PlayerPoint
    {
        public Vector3 Point;
        public bool IsVacant;
    }

    public class FollowPlayerService : IFollowPlayerService, IInitializable, ITickable
    {
        public IReadOnlyCollection<PlayerPoint> PointsAroundPlayer => _pointsAroundPlayer;
        
        private readonly List<PlayerPoint> _pointsAroundPlayer = new();
        
        private readonly IGameFactory _gameFactory;
        private IWaveSystemDirector _waveSystemDirector;

        public FollowPlayerService(IGameFactory gameFactory, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
            for (var i = 0; i < 4; i++)
            {
                _pointsAroundPlayer.Add(new PlayerPoint()
                {
                    Point = Vector3.zero,
                    IsVacant = true,
                });
            }
        }

        public void Tick()
        {
            return;
            if(_waveSystemDirector.CurrentWaveState == null)
                return;
            for (var i = 0; i < _pointsAroundPlayer.Count; i++)
            {
                Vector3 point = _gameFactory.Player.transform.position +
                              new Vector2(Mathf.Cos(Mathf.PI * 2 * i / _pointsAroundPlayer.Count), Mathf.Sin(Mathf.PI * 2 * i / _pointsAroundPlayer.Count))
                                  .XZtoXYZ() * 3;

                _pointsAroundPlayer[i].Point = point;
            }

            foreach (Health aliveEnemy in _waveSystemDirector.CurrentWaveState.AliveEnemies)
            {
                foreach (PlayerPoint playerPoint in _pointsAroundPlayer)
                {
                    if (Vector3.Distance(aliveEnemy.transform.position, playerPoint.Point) < 1f)
                    {
                        playerPoint.IsVacant = false;
                    }

                }

            }
        }

        public Vector3 GetClosestVacantPoint(Transform follower)
        {
            PlayerPoint playerPoint = _pointsAroundPlayer.First(point => point.IsVacant);
            return playerPoint.Point;
        }
    }
}