using System.Collections.Generic;
using Common.Extensions;
using Game.Enemies.Item;
using UnityEngine;

namespace Game.Enemies.Control
{
    public class EnemiesSpawner : Singleton<EnemiesSpawner>
    {
        [SerializeField] private Enemy enemyPrefab;

        [SerializeField] private Transform enemiesParent;
        
        [SerializeField] private List<Enemy> currentEnemies;

        public Enemy SpawnEnemy(Vector2 position)
        {
            var newEnemy = Instantiate(enemyPrefab, enemiesParent);

            newEnemy.transform.position = position;

            return newEnemy;
        }
    }
}