using Common.Extensions;
using UnityEngine;

namespace Game.Enemies.Control
{
    [RequireComponent(typeof(EnemiesSpawner))]
    [RequireComponent(typeof(EnemiesMover))]
    public class EnemiesControl : Singleton<EnemiesControl>
    {
        private EnemiesSpawner _spawner;

        private EnemiesMover _mover;

        protected override void Awake()
        {
            base.Awake();

            _spawner = GetComponent<EnemiesSpawner>();

            _mover = GetComponent<EnemiesMover>();
        }
    }
}