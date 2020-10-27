using System;
using System.Collections.Generic;
using Game.Units.Unit_Types;
using LogicHelper;
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
            if (parameters == null)
                return;
            
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

        public int maxCountToBuild = 4;

        public int[] pricesLevelUpdate =
        {
            400,
            900
        };

        public Sprite avatarSet;

        public string unitName;

        public bool isRandomPosition;

        [Header("Build add")] 
        
        public Sprite buildSprite;
        
        public int priceBuild = 100;

        public int timeBuildSeconds = 10;

        public bool priceModiferForLiveCount;

        public bool priceEnumerationForLiveCount;

        public int enumerationPrice;
    }

    [Serializable]
    public class UnitGameParameters
    {
        public ControlType controlType;

        public int currentHealth;

        public Unit prefab;

        public PoolType poolType;

        public Sprite avatarSet;

        public string unitName;

        public int startHealth;

        public int currentLevel;

        public int[] pricesLevelUpgrades;

        public Sprite buildSprite;

        public int countSupply;

        public bool isGiveSupply;

        public int price;

        public bool isRandomPosition;
        
        public List<BuildUnitParameters> currentBuilds = new List<BuildUnitParameters>();
        
        public bool IsMaxLevelNow => currentLevel >= MaxLevel;

        public int MaxLevel => pricesLevelUpgrades.Length;

        public int CurrentPriceUpgrade => pricesLevelUpgrades[currentLevel];

        public UnitGameParameters(UnitData data)
        {
            var parameters = data.parameters;
            
            controlType = parameters.controlType;

            currentHealth = parameters.startHealth;

            prefab = data.prefab;

            poolType = parameters.poolType;

            avatarSet = parameters.avatarSet;

            unitName = parameters.unitName;

            startHealth = parameters.startHealth;

            pricesLevelUpgrades = parameters.pricesLevelUpdate;

            buildSprite = data.parameters.buildSprite;

            countSupply = parameters.countSupply;

            isGiveSupply = parameters.isGiveSupply;

            price = parameters.priceBuild;

            isRandomPosition = parameters.isRandomPosition;
        }
    }

    [Serializable]
    public class BuildUnitParameters
    {
        public int currentSeconds;
        
        public int needSeconds;

        public UnitData toBuildUnit;

        public BuildUnitParameters(int seconds, UnitData unitToBuild)
        {
            needSeconds = seconds;

            toBuildUnit = unitToBuild;
        }
        
        public void BuildCurrent()
        {
            UnitProcessBuild.BuildNewFromOther(this);
        }
        
        public void AddSeconds()
        {
            currentSeconds++;

            if (currentSeconds >= needSeconds)
            {
                BuildCurrent();
            }
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