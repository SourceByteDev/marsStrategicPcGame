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

        public static UnitSpawner Instance;

        public UnityEvent<Unit> onUnitSpawned;
        
        public void SpawnUnit(UnitData data, Vector2 position)
        {
            var parameters = data.parameters;

            var rightParent = GetRightParentFrom(parameters.poolType);

            var newUnit = Instantiate(data.prefab.gameObject, position, Quaternion.identity, rightParent);

            var unitController = newUnit.GetComponent<Unit>();

            unitController.InitParameters(data);

            currentUnits.Add(unitController);

            if (parameters.isGiveSupply)
                Managers.Values.values.CurrentMaxSupply += parameters.countSupply;
            else
                Managers.Values.values.CurrentSupply += parameters.countSupply;
            
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