using System;
using System.Collections.Generic;
using System.IO;
using Data;
using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class ValuesManage : MonoBehaviour
    {
        [Header("Main")]
        
        [Tooltip("Dont fucking touch this")] public IntroductionValues values;

        [Header("Game Variables")]
        
        public StartValueData startData;

        public UnityAction onSomeValueChanged;

        private readonly PathData path = new PathData("values");

        [ContextMenu("Reset all")]
        public void ResetAllValues()
        {
            values.CurrentGemsCount = startData.startGems;

            values.CurrentTimeSeconds = 0;

            values.CurrentMaxSupply = 0;
            
            values.LiveUnits.Clear();

            PlayerPrefs.DeleteAll();
            
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

        private void OnSomeValueChanged()
        {
            onSomeValueChanged?.Invoke();
            
            Save();
        }

        private void Awake()
        {
            Load();
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
        public struct LiveUnitData
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