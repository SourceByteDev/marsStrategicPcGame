using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class Managers : MonoBehaviour
    {
        private bool menuWasLoaded;
        
        public static ValuesManage Values { get; private set; }

        public static GameControl GameControl { get; private set; }


        private static Managers _instance;

        [ContextMenu("Clear")]
        public static void Clear()
        {
            GameControl.ResetAllData();
        }

        private void CheckIfGameLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "menu")
                menuWasLoaded = true;
            
            if (scene.name != "game")
                return;

            if (!menuWasLoaded)
            {
                SceneManager.LoadScene("menu");
                
                return;
            }

            GameControl.OnGameStarted();
            
            print("game");
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

            SceneManager.sceneLoaded += CheckIfGameLoaded;
        }
    }
}