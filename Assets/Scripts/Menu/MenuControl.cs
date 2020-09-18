using System;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuControl : MonoBehaviour
    {
        public Button startButton;

        private void OnPressStartButton()
        {
            GameControl.OnGameStarted();
            
            SceneManager.LoadScene("game");
        }

        private void Start()
        {
            startButton.onClick.AddListener(OnPressStartButton);
        }
    }
}
