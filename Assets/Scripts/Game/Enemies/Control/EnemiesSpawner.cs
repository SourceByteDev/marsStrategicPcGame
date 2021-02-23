using System;
using System.Collections;
using System.Collections.Generic;
using Common.Extensions;
using Data;
using Game.Enemies.Item;
using Game.Units.Control;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies.Control
{
    public class EnemiesSpawner : Singleton<EnemiesSpawner>
    {
        [SerializeField] private SpawnParameters[] difficultiesSpawn;

        [SerializeField] private Vector2 startPosition;

        [SerializeField] private UnitData[] enemiesData;

        [SerializeField] private Transform enemiesParent;
        
        [SerializeField] private List<Enemy> currentEnemies;

        private UnitSpawner _spawner;
        
        private UnitData RandomEnemyData => enemiesData[Random.Range(0, enemiesData.Length)];
        
        public void LoadSpawn(int difficult)
        {
            var indexDifficult = Mathf.Clamp(difficult, 0, difficultiesSpawn.Length - 1);

            var parameters = difficultiesSpawn[indexDifficult];

            StartCoroutine(SpawningByParameters(parameters));
        }

        [ContextMenu("Spawn new enemy")]
        private void SpawnNewEnemy()
        {
            var data = RandomEnemyData;
            
            _spawner.SpawnUnit(data, startPosition);
        }

        private void Start()
        {
            _spawner = UnitSpawner.Instance;
        }

        private IEnumerator SpawningByParameters(SpawnParameters parameters)
        {
            while (true)
            {
                
                
                yield return new WaitForSeconds(parameters.RandomOffsetSpawn);
            }
        }

        [Serializable]
        private class SpawnParameters
        {
            [SerializeField] private float minOffsetSpawn = 1;

            [SerializeField] private float maxOffsetSpawn = 3;

            public float RandomOffsetSpawn => Random.Range(minOffsetSpawn, maxOffsetSpawn);
        }
    }
}