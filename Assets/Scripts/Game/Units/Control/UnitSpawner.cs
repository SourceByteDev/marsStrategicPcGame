using System;
using System.Collections.Generic;
using Data;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Units.Control
{
    public class UnitSpawner : MonoBehaviour
    {
        public Transform parentPlayerBuildings;

        public Transform parentPlayerSoldiers;

        public Transform parentEnemyBuildings;

        public Transform parentEnemySoldiers;

        public List<Unit> currentUnits;

        public static UnitSpawner Instance { get; private set; }

        public UnityAction<Unit> onUnitSpawned;

        public UnityAction<Unit> onUnitDestroyed;

        public void RemoveUnit(Unit unit)
        {
            currentUnits.Remove(unit);

            var liveUnits = Managers.Values.values.LiveUnits;

            var foundUnit = liveUnits.Find(x =>
                x.parameters == unit.gameParameters && Vector2.Distance(unit.transform.position, x.position) < .1f);

            liveUnits.Remove(foundUnit);
            
            onUnitDestroyed?.Invoke(unit);
            
            Destroy(unit.gameObject);
        }

        public void SpawnUnit(UnitData data, Vector2 position)
        {
            var parameters = data.parameters;

            var rightParent = GetRightParentFrom(parameters.poolType);

            var newUnit = Instantiate(data.prefab.gameObject, position, Quaternion.identity, rightParent);

            var unitController = newUnit.GetComponent<Unit>();

            unitController.InitParameters(data);

            currentUnits.Add(unitController);

            Managers.Values.values.LiveUnits.Add(new ValuesManage.LiveUnitData()
            {
                position = unitController.transform.position,
                parameters = unitController.gameParameters
            });

            onUnitSpawned?.Invoke(unitController);
        }

        public void SpawnWithOutSave(UnitGameParameters parameters, Vector2 position)
        {
            var rightParent = GetRightParentFrom(parameters.poolType);

            var newUnit = Instantiate(parameters.prefab.gameObject, position, Quaternion.identity,
                rightParent);

            var unitController = newUnit.GetComponent<Unit>();

            unitController.InitParameters(parameters);

            currentUnits.Add(unitController);

            onUnitSpawned?.Invoke(unitController);
        }

        private Transform GetRightParentFrom(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.PlayerBuilding:
                    return parentPlayerBuildings;
                case PoolType.PlayerSoldier:
                    return parentPlayerSoldiers;
                case PoolType.EnemyBuilding:
                    return parentEnemyBuildings;
                case PoolType.EnemySoldier:
                    return parentEnemySoldiers;
                default:
                    return null;
            }
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}