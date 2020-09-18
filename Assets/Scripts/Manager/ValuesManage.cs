using System;
using System.IO;
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

        public LevelData[] levelsData;

        public UnityAction onSomeValueChanged;

        private readonly PathData path = new PathData("values");

        [ContextMenu("Reset all")]
        public void ResetAllValues()
        {
            values.CurrentGemsCount = startData.startGems;

            values.CurrentTimeSeconds = 0;

            values.CurrentXp = 0;

            values.CurrentLevelIndex = 0;

            print("rest");
            
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

            [SerializeField] private int currentXp;

            [SerializeField] private int currentLevelIndex;

            public LevelData CurrentLevelData => Managers.Values.levelsData[CurrentLevelIndex];
            
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

            public int CurrentXp
            {
                get => currentXp;
                set
                {
                    currentXp = value; 
                
                    OnSomeValueChanged();
                }
            }
        
            public int CurrentLevelIndex
            {
                get => currentLevelIndex;
                set
                {
                    currentLevelIndex = value; 
                
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
        public struct LevelData
        {
            public int needXpToComplete;
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
