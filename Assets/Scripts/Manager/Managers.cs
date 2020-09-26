using System;
using UnityEngine;

namespace Manager
{
    public class Managers : MonoBehaviour
    {
        public static ValuesManage Values { get; private set; }
        
        public static GameControl GameControl { get; private set; }
        
        private static Managers _instance;

        [ContextMenu("Clear")]
        public static void Clear()
        {
            GameControl.ResetAllData();
        }
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                
                return;
            }

            _instance = this;

            Values = GetComponent<ValuesManage>();

            GameControl = GetComponent<GameControl>();
            
            DontDestroyOnLoad(gameObject);
        }
    }
}