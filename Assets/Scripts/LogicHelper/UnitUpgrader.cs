using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Extensions;
using Data;
using Game.GameHoverHelper;
using Game.Units.Control;
using GameUi;
using Manager;
using UnityEngine;

namespace LogicHelper
{
    public class UnitUpgrader : Singleton<UnitUpgrader>
    {
        [SerializeField] private SaveUpgrades save;

        [SerializeField] private UpgradeVariables fly;

        [SerializeField] private UpgradeVariables ground;

        private static string SavePath => Path.Combine(Application.persistentDataPath, "upgrades.json");

        private IEnumerable<UpgradeVariables> AllUpgrades => new[]
        {
            fly,
            ground
        };

        public HoverPanel.BuildParameters GenerateParametersOf(TypeUpgrade upgradeType,
            TypeVariableUpgrade variableType)
        {
            var rightUpgrade = AllUpgrades.ToList().Find(x => x.Type == upgradeType);

            var rightVariable = rightUpgrade.GetVariableByType(variableType);

            return new HoverPanel.BuildParameters()
            {
                IsGiveSupply = false,
                Price = rightVariable.Price,
                Supply = 0,
                Time = rightVariable.SecondsToGet
            };
        }

        public bool CanUpgradeOf(TypeUpgrade upgradeType, TypeVariableUpgrade variableType)
        {
            var gems = Managers.Values.values.CurrentGemsCount;

            var rightUpgrade = AllUpgrades.ToList().Find(x => x.Type == upgradeType);

            var rightVariable = rightUpgrade.GetVariableByType(variableType);

            var isInUpgrading =
                save.CurrentUpgrades.Any(x => x.UpgradeType == upgradeType && x.VariableUpgrade == variableType);

            return gems >= rightVariable.Price && !isInUpgrading;
        }

        public void StartUpgradeTimer(TypeUpgrade type, TypeVariableUpgrade variable)
        {
        }

        public void StartUpgradeTimer(UpgradeItem item)
        {
            if (save.CurrentUpgrades.Count(x => x.SecondsToGet > 0) >= 4)
                return;

            var upgradeType = item.UpgradeType;

            var variableType = item.VariableUpgrade;

            if (AllUpgrades.All(x => x.Type != upgradeType))
                return;

            var rightUpgrade = AllUpgrades.ToList().Find(x => x.Type == upgradeType);

            var rightSeconds = 0;

            var values = Managers.Values.values;

            var data = rightUpgrade.Health.Data;

            switch (variableType)
            {
                case TypeVariableUpgrade.Weapon:
                    var weapon = rightUpgrade.Weapon;

                    if (!values.TryRemoveGems(weapon.Price))
                        return;

                    rightSeconds = weapon.SecondsToGet;

                    data = weapon.Data;
                    break;
                case TypeVariableUpgrade.Speed:
                    var speed = rightUpgrade.Speed;

                    if (!values.TryRemoveGems(speed.Price))
                        return;

                    rightSeconds = speed.SecondsToGet;

                    data = speed.Data;
                    break;
                case TypeVariableUpgrade.Health:
                    var health = rightUpgrade.Health;

                    if (!values.TryRemoveGems(health.Price))
                        return;

                    rightSeconds = health.SecondsToGet;

                    data = health.Data;
                    break;
            }

            save.CurrentUpgrades.Add(new UpgradeItem(upgradeType, variableType, data, rightSeconds));

            UnitBuilder.AddUnitCurrentToBuild(data);

            UnitSelector.Instance.UpdateSelectedUnit();
        }

        public void DoUpgrade(UpgradeItem item)
        {
            var upgradeType = item.UpgradeType;

            var variableType = item.VariableUpgrade;

            if (AllUpgrades.All(x => x.Type != upgradeType))
                return;

            var values = Managers.Values.values;

            var rightUpgrade = AllUpgrades.ToList().Find(x => x.Type == upgradeType);

            var units = GetAllUnitsOf(upgradeType).ToList();

            switch (variableType)
            {
                case TypeVariableUpgrade.Health:

                    var health = rightUpgrade.Health;

                    units.ForEach(x => x.parameters.currentHealth += (int) health.Value);

                    units.ForEach(x => x.parameters.startHealth += (int) health.Value);

                    break;

                case TypeVariableUpgrade.Speed:

                    var speed = rightUpgrade.Speed;

                    units.ForEach(x => x.parameters.moveParameters.MoveSpeed += speed.Value);

                    break;

                case TypeVariableUpgrade.Weapon:

                    var weapon = rightUpgrade.Weapon;

                    if (!values.TryRemoveGems(weapon.Price))
                        break;

                    // TODO: Add damage value

                    break;
            }

            UnitSelector.Instance.UpdateSelectedUnit();
        }

        [ContextMenu("Load")]
        private void Load()
        {
            if (!File.Exists(SavePath))
                return;

            save = JsonUtility.FromJson<SaveUpgrades>(File.ReadAllText(SavePath));
        }

        [ContextMenu("Save")]
        private void Save()
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(save));
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            save = new SaveUpgrades();

            Save();
        }

        public static void DoClear()
        {
            if (!File.Exists(SavePath))
                return;

            File.Delete(SavePath);

            if (Instance != null)
                Instance.save = new SaveUpgrades();
        }

        protected override void Awake()
        {
            base.Awake();

            Load();
        }

        private void Start()
        {
            Timer.Instance.OnSecond += save.RemoveSecondFromAll;

            Timer.Instance.OnSecond += Save;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
                return;

            Save();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private static IEnumerable<ValuesManage.LiveUnitData> GetAllUnitsOf(TypeUpgrade typeUpgrade)
        {
            return Managers.Values.values.LiveUnits.Where(x =>
                x.parameters.typeUpgrade == typeUpgrade && x.parameters.poolType == PoolType.PlayerSoldier);
        }

        [Serializable]
        public struct UpgradeVariables
        {
            [SerializeField] private UpgradeVariable speed;

            [SerializeField] private UpgradeVariable weapon;

            [SerializeField] private UpgradeVariable health;

            [SerializeField] private TypeUpgrade type;

            public UpgradeVariable Speed => speed;

            public UpgradeVariable Weapon => weapon;

            public UpgradeVariable Health => health;

            public TypeUpgrade Type => type;

            private UpgradeVariable[] AllVariables => new[]
            {
                speed,
                weapon,
                health
            };

            public UpgradeVariable GetVariableByType(TypeVariableUpgrade gotType)
            {
                return AllVariables.ToList().Find(x => x.Type == gotType);
            }
        }

        [Serializable]
        public struct UpgradeVariable
        {
            [SerializeField] private float value;

            [SerializeField] private int price;

            [SerializeField] private TypeVariableUpgrade type;

            [SerializeField] private int secondsToGet;

            [SerializeField] private UnitData data;

            public float Value => value;

            public int Price => price;

            public TypeVariableUpgrade Type => type;

            public int SecondsToGet => secondsToGet;

            public UnitData Data => data;
        }

        [Serializable]
        public struct SaveUpgrades
        {
            [SerializeField] private List<UpgradeItem> currentUpgrades;

            public List<UpgradeItem> CurrentUpgrades => currentUpgrades;

            public void RemoveSecondFromAll()
            {
                var currentToRemove = currentUpgrades.Where(x => x.SecondsToGet > 0).ToList();

                if (currentToRemove.Count <= 0)
                    return;

                currentToRemove.ForEach(x => x.SecondsToGet--);

                var foundToDoUpgrade = currentToRemove.Where(x => x.SecondsToGet == 0).ToList();

                if (foundToDoUpgrade.Count <= 0)
                    return;

                foundToDoUpgrade.ForEach(x => Instance.DoUpgrade(x));
            }
        }

        [Serializable]
        public class UpgradeItem
        {
            [SerializeField] private TypeUpgrade upgradeType;

            [SerializeField] private TypeVariableUpgrade variableUpgrade;

            [SerializeField] private int secondsToGet;

            [SerializeField] private UnitData data;

            public TypeUpgrade UpgradeType => upgradeType;

            public TypeVariableUpgrade VariableUpgrade => variableUpgrade;

            public UnitData Data => data;

            public int SecondsToGet
            {
                get => secondsToGet;
                set => secondsToGet = value;
            }

            public UpgradeItem(TypeUpgrade typeUpgrade, TypeVariableUpgrade typeVariable, UnitData data = null,
                int secondsToGet = 0)
            {
                upgradeType = typeUpgrade;

                variableUpgrade = typeVariable;

                this.secondsToGet = secondsToGet;

                this.data = data;
            }
        }

        public enum TypeVariableUpgrade
        {
            Speed,
            Weapon,
            Health
        }

        public enum TypeUpgrade
        {
            Fly,
            Ground
        }
    }
}