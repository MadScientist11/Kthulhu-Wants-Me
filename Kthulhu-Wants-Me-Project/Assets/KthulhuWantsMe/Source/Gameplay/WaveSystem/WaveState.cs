﻿using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class WaveState
    {
        public event Action BatchCleared;
        public event Action WaveCleared;

        public WaveObjective WaveObjective
        {
            get
            {
                return _waveData.WaveObjective;
            }
        }

        public Batch CurrentBatchData
        {
            get
            {
                return _waveData.Batches[_currentBatchIndex];
            }
        }

        public int CurrentBatchIndex
        {
            get
            {
                return _currentBatchIndex;
            }
        }

        public Dictionary<EnemySpawnerId, List<Health>> AliveEnemiesByPlace
        {
            get
            {
                return _aliveEnemiesByPlace;
            }
        }
        
        public Dictionary<EnemySpawnerId, Health> PendingEnemies
        {
            get
            {
                return _pendingEnemies;
            }
        }
        
        public List<Health> AliveEnemies
        {
            get
            {
                return _aliveEnemies;
            }
        }

        public WaveData CurrentWaveData
        {
            get
            {
                return _waveData;
            }
        }


        private readonly Dictionary<EnemySpawnerId, List<Health>> _aliveEnemiesByPlace = new();
        private readonly Dictionary<EnemySpawnerId, Health> _pendingEnemies = new();
        private readonly List<Health> _aliveEnemies = new();

        private readonly WaveData _waveData;
        private int _currentBatchIndex;

        public WaveState(WaveData waveData)
        {
            _waveData = waveData;
        }

        public void RegisterEnemy(EnemySpawnerId enemySpawnPlace, Health enemy)
        {
            _aliveEnemiesByPlace.GetOrCreate(enemySpawnPlace).Add(enemy);
            _aliveEnemies.Add(enemy);

            enemy.Died += TrackEnemyTillDeath(enemySpawnPlace, enemy);
        }

        public void DeregisterEnemy(EnemySpawnerId enemySpawnPlace, Health enemy)
        {
            _aliveEnemiesByPlace[enemySpawnPlace].Remove(enemy);
            _aliveEnemies.Remove(enemy);

            enemy.Died -= TrackEnemyTillDeath(enemySpawnPlace, enemy);
        }
        
        public void RegisterEnemyAsPending(EnemySpawnerId enemySpawnPlace, Health enemy)
        {
            _pendingEnemies[enemySpawnPlace] = enemy;
        }
        
        public void DeregisterEnemyAsPending(EnemySpawnerId enemySpawnPlace, Health enemy)
        {
            _pendingEnemies.Remove(enemySpawnPlace);
        }

        public void ModifyBatchIndex(int index)
        {
            _currentBatchIndex = index;
        }

        public void CleanUp()
        {
            _aliveEnemiesByPlace.Clear();
            _aliveEnemies.Clear();
            _currentBatchIndex = 0;

            WaveCleared = null;
            BatchCleared = null;
        }

        private Action TrackEnemyTillDeath(EnemySpawnerId enemySpawnPlace, Health enemy)
        {
            return () => TrackEnemiesDeath(enemySpawnPlace, enemy);
        }

        private void TrackEnemiesDeath(EnemySpawnerId wasSpawnedAt, Health deathHealth)
        {
            _aliveEnemiesByPlace[wasSpawnedAt].Remove(deathHealth);
            _aliveEnemies.Remove(deathHealth);

            if (NoEnemiesLeft())
            {
                if (IsLastBatch())
                {
                    BatchCleared?.Invoke();
                    WaveCleared?.Invoke();
                    return;
                }

                BatchCleared?.Invoke();
            }
        }

        private bool NoEnemiesLeft()
        {
            return _aliveEnemies.Count == 0 && _pendingEnemies.Count == 0;
        }

        public bool IsLastBatch()
        {
            return _waveData.Batches.Count - 1 <= _currentBatchIndex;
        }
    }
}