using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;
using WaitForSeconds = KthulhuWantsMe.Source.Utilities.WaitForSeconds;

namespace KthulhuWantsMe.Source.Gameplay.Spell
{
    public class MinionsSpawnSpell : MonoBehaviour
    {
        private float _radius = 2f;
        
        private Material _spellMaterial;
        
        private static readonly int Color = Shader.PropertyToID("_Color");
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Awake()
        {
            _spellMaterial = GetComponent<Renderer>().material;
            _spellMaterial.SetTransparency(Color, 0);
        }

        public async UniTask Activate()
        {
            await _spellMaterial
                .DOFade(1, 3)
                .ToUniTask();

             StartSpawnLoop();
        }
        
        public async UniTask Deactivate()
        {
           await _spellMaterial
                .DOFade(0, 2).ToUniTask();
        }

        private void StartSpawnLoop()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                BatchSpawn(1);
                yield return WaitForSeconds.Wait(60);
            }
        }

        private void BatchSpawn(int count)
        {
            float angleStep = Mathfs.TAU / count;
            
            for (float i = 0; i < Mathfs.TAU; i += angleStep)
            { 
                Vector2 pointOnCircle = new Vector2(Mathfs.Cos(i),Mathfs.Sin(i)) * _radius;
                _gameFactory.CreateEnemy(transform.position + pointOnCircle.XZtoXYZ(), Quaternion.identity, EnemyType.Cyeagha);
            }
        }
    }
}