using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using Game.Units.Control;
using Game.Units.Unit_Types;
using LogicHelper;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class ValuesManage : MonoBehaviour
    {
        [Header("Main")] [Tooltip("Dont fucking touch this")]
        public IntroductionValues values;

        [Header("Game Variables")] public StartValueData startData;

        public UnityAction onSomeValueChanged;

        public int CurrentMaxCountWorkers => 5 + values.LiveUnits[0].parameters.currentLevel * 5 -
                                             (2 + values.LiveUnits[0].parameters.currentLevel);

        private readonly PathData path = new PathData("values");

        public LiveUnitData GetLiveUnitByUnit(Unit unit)
        {
            var liveUnits = Managers.Values.values.LiveUnits;

            var foundUnit = liveUnits.Find(x =>
                x.parameters == unit.gameParameters && Vector2.Distance(unit.transform.position, x.position) < .1f);

            return foundUnit;
        }

        public Unit GetUnitByLiveUnit(LiveUnitData data)
        {
            var unitSpawner = UnitSpawner.Instance;

            var allUnits = unitSpawner.currentUnits;

            var isContains = allUnits.Any(x =>
                x.gameParameters == data.parameters && Vector2.Distance(data.position, x.transform.position) < .1f);

            if (!isContains)
                return null;

            var found = allUnits.Find(x =>
                x.gameParameters == data.parameters && Vector2.Distance(data.position, x.transform.position) < .1f);

            return found;
        }

        public bool IsSupplyFullIfSell(int removeFromMax)
        {
            return values.CurrentSupply > values.CurrentMaxSupply - removeFromMax;
        }

        public bool IsSupplyFull(int addSupply)
        {
            return values.CurrentSupply + addSupply > values.CurrentMaxSupply;
        }

        [ContextMenu("Reset all")]
        public void ResetAllValues()
        {
            values.CurrentGemsCount = startData.startGems;

            values.CurrentTimeSeconds = 0;

            values.CurrentMaxSupply = 0;

            values.CurrentSupply = 0;

            values.LiveUnits.Clear();

            PlayerPrefs.DeleteAll();

            UnitUpgrader.DoClear();

            print("Reset values");

            Save();
        }

        [ContextMenu("Load")]
        private void Load()
        {
            if (!File.Exists(path.Full))
                return;

            values = JsonUtility.FromJson<IntroductionValues>(File.ReadAllText(path.Full));
        }

        [ContextMenu("Save")]
        private void Save()
        {
            File.WriteAllText(path.Full, JsonUtility.ToJson(values));
        }

        public int CountOfAllTypes(Sprite avatar)
        {
            var countLive = CountOfLiveUnits(avatar) + CountOfBuildUnits(avatar);

            return countLive;
        }

        public int CountOfLiveUnits(Sprite avatar)
        {
            return values.LiveUnits.Count(x => x.parameters.avatarSet == avatar);
        }

        public int CountOfBuildUnits(Sprite avatar)
        {
            return values.LiveUnits.Sum(liveUnit =>
                liveUnit.parameters.currentBuilds.Count(x =>
                    x.toBuildUnit.parameters.avatarSet == avatar));
        }

        private void OnSomeValueChanged()
        {
            onSomeValueChanged?.Invoke();

            Save();
        }

        private void Awake()
        {
            Load();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            Save();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Save();
        }

        [Serializable]
        public class IntroductionValues
        {
            [SerializeField] private int timeSeconds;

            [SerializeField] private int gemsCount;

            [SerializeField] private int currentMaxSupply;

            [SerializeField] private int currentSupply;

            [SerializeField] private List<LiveUnitData> liveUnits;

            public int CurrentTimeSeconds
            {
                get => timeSeconds;

                set
                {
                    timeSeconds = value;

                    OnSomeValueChanged();
                }
            }

            public int CurrentGemsCount
            {
                get => gemsCount;

                set
                {
                    value = Mathf.Clamp(value, 0, int.MaxValue);

                    gemsCount = value;

                    OnSomeValueChanged();
                }
            }

            public int CurrentSupply
            {
                get => currentSupply;
                set
                {
                    currentSupply = value;

                    OnSomeValueChanged();
                }
            }

            public int CurrentMaxSupply
            {
                get => currentMaxSupply;
                set
                {
                    currentMaxSupply = value;

                    OnSomeValueChanged();
                }
            }

            public List<LiveUnitData> LiveUnits
            {
                get => liveUnits;
                set
                {
                    liveUnits = value;

                    OnSomeValueChanged();
                }
            }

            public bool TryRemoveGems(int value)
            {
                if (value > gemsCount)
                    return false;

                CurrentGemsCount -= value;

                return true;
            }

            private void OnSomeValueChanged()
            {
                Managers.Values.OnSomeValueChanged();
            }
        }

        [Serializable]
        public struct StartValueData
        {
            public int startGems;
        }

        [Serializable]
        public class LiveUnitData
        {
            public Vector2 position;

            public UnitGameParameters parameters;
        }
    }

    public readonly struct PathData
    {
        private readonly string nameFile;

        public PathData(string nameFile)
        {
            this.nameFile = nameFile;
        }

        public string Full => $"{Application.persistentDataPath}/{nameFile}.json";
    }
}