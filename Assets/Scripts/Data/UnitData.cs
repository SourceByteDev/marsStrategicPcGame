using System;
using Game.Units.Unit_Types;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Unit Data", menuName = "Game/Unit Data", order = 0)]
    public class UnitData : ScriptableObject
    {
        public Unit prefab;

        public UnitParameters parameters;

        private void Awake()
        {
            if (string.IsNullOrEmpty(parameters.unitName))
                parameters.unitName = name;
        }
    }

    [Serializable]
    public class UnitParameters
    {
        public PoolType poolType;
        
        public ControlType controlType;

        public bool isGiveSupply;

        public int countSupply = 1;

        public int startHealth = 100;

        public Sprite avatarSet;

        public string unitName;
    }

    [Serializable]
    public class UnitGameParameters
    {
        public ControlType controlType;

        public int currentHealth;

        public Unit prefab;

        public PoolType poolType;

        public Sprite avatar;

        public string unitName;

        public int startHealth;
        
        public UnitGameParameters(UnitData data)
        {
            var parameters = data.parameters;
            
            controlType = parameters.controlType;

            currentHealth = parameters.startHealth;

            prefab = data.prefab;

            poolType = parameters.poolType;

            avatar = parameters.avatarSet;

            unitName = parameters.unitName;

            startHealth = parameters.startHealth;
        }
    }
    
    public enum PoolType
    {
        PlayerBuilding,
        PlayerSoldier,
        EnemyBuilding,
        EnemySoldier
    }
}