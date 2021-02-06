using System;
using System.Collections.Generic;
using Game.Units.Control;
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
        [SerializeField] private bool isEnemy;

        [SerializeField] private UnitUpgrader.TypeUpgrade upgradeType;

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

        [Header("Build add")] public Sprite buildSprite;

        public int priceBuild = 100;

        public int timeBuildSeconds = 10;

        public bool priceModiferForLiveCount;

        public bool priceEnumerationForLiveCount;

        public int enumerationPrice;

        [Header("Mover parameters")] [SerializeField]
        private float moveSpeed;

        [Header("Worker parameters")] [SerializeField]
        private int secondsToCollect;

        [Header("Attacker parameters")] [SerializeField]
        private float damage;

        [SerializeField] private float shotSpeed = 2;

        public bool IsEnemy => isEnemy;

        public float MoveSpeed => moveSpeed;

        public int SecondsToCollect => secondsToCollect;
        
        public UnitUpgrader.TypeUpgrade UpgradeType => upgradeType;

        public float Damage => damage;

        public float ShotSpeed => shotSpeed;
    }

    [Serializable]
    public class UnitGameParameters
    {
        [SerializeField] private float shotSpeed;

        [SerializeField] private float damage;

        [SerializeField] private bool isEnemy;

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

        public float ShotSpeed => shotSpeed;

        public float Damage => damage;

        public List<BuildUnitParameters> currentBuilds = new List<BuildUnitParameters>();

        public MoveParameters moveParameters;

        public UnitUpgrader.TypeUpgrade typeUpgrade;

        public bool IsMaxLevelNow => currentLevel >= MaxLevel;

        public int MaxLevel => pricesLevelUpgrades.Length;

        public int CurrentPriceUpgrade => pricesLevelUpgrades[currentLevel];

        public bool IsEnemy => isEnemy;

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

            typeUpgrade = parameters.UpgradeType;

            damage = parameters.Damage;

            shotSpeed = parameters.ShotSpeed;
            
            moveParameters = new MoveParameters
            {
                MoveSpeed = parameters.MoveSpeed,
                WorkerMove = new WorkerMoveParameters(),
                InfantryMove = new InfantryMoveParameters()
            };

            moveParameters.WorkerMove.MaxSecondsToCollect = parameters.SecondsToCollect;

            isEnemy = parameters.IsEnemy;
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

    [Serializable]
    public class MoveParameters
    {
        [SerializeField] private float moveSpeed;

        [SerializeField] private WorkerMoveParameters workerMove;

        [SerializeField] private InfantryMoveParameters infantryMove;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        public WorkerMoveParameters WorkerMove
        {
            get => workerMove;
            set => workerMove = value;
        }

        public InfantryMoveParameters InfantryMove
        {
            get => infantryMove;
            set => infantryMove = value;
        }
    }

    [Serializable]
    public class WorkerMoveParameters
    {
        [SerializeField] private WorkerMoveState workerMoveState;

        [SerializeField] private int currentSecondsCollect;

        [SerializeField] private int maxSecondsToCollect;

        public int MaxSecondsToCollect
        {
            get => maxSecondsToCollect;
            set => maxSecondsToCollect = value;
        }

        public WorkerMoveState WorkerMoveState
        {
            get => workerMoveState;
            set => workerMoveState = value;
        }

        public int SecondsToCollect
        {
            get => currentSecondsCollect;
            set => currentSecondsCollect = value;
        }
    }

    [Serializable]
    public class InfantryMoveParameters
    {
        [SerializeField] private InfantryMoveState moveState;

        public InfantryMoveState MoveState
        {
            get => moveState;
            set => moveState = value;
        }
    }

    public enum InfantryMoveState
    {
        MovingToPoint,
        WaitingTarget,
        Attacking
    }

    public enum WorkerMoveState
    {
        GoToGems,
        CollectingGems,
        GoToHome
    }

    public enum PoolType
    {
        PlayerBuilding,
        PlayerSoldier,
        EnemyBuilding,
        EnemySoldier,
        Upgrade
    }
}