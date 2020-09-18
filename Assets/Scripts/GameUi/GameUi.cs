using System;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUi
{
    public class GameUi : MonoBehaviour
    {
        [Header("Introduction values")]
        
        public Button menuButton;

        [Space(5)]
        
        public Text gemsCountText;

        public Text timerText;

        public Text xpText;
        
        [Header("Pre")] 
        
        public Timer timer;
        
        private static ValuesManage.IntroductionValues Values => Managers.Values.values;
        
        [ContextMenu("Load Menu")]
        public void LoadMenu()
        {
            SceneManager.LoadScene("menu");
        }
        
        [ContextMenu("Update UI")]
        public void UpdateUi()
        {
            Additional.SetEventsToButton(menuButton, LoadMenu);

            Additional.SetValueToText(gemsCountText, Values.CurrentGemsCount);
            
            Additional.SetValueToText(xpText, 
                $"{Values.CurrentXp}/{Values.CurrentLevelData.needXpToComplete}");
            
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            var currentSeconds = Values.CurrentTimeSeconds;
            
            var minutes = Mathf.FloorToInt(currentSeconds/ 60f);

            var seconds = currentSeconds - minutes * 60;

            var isAddSeconds = seconds > 9 ? "" : "0";
            
            var isAddMinutes = minutes > 9 ? "" : "0";

            if (timerText == null)
                return;
            
            timerText.text = isAddMinutes + minutes + ":" + isAddSeconds + seconds;
        }
        
        private void Start()
        {
            Managers.Values.onSomeValueChanged += UpdateUi;
            
            UpdateUi();
            
            timer.StartTimer();
        }
    }

    public static class Additional
    {
        public static void SetEventsToButton(Button button, params UnityAction[] actions)
        {
            button.onClick.RemoveAllListeners();

            foreach (var action in actions)
            {
                button.onClick.AddListener(action);
            }
        }

        public static void SetValueToText(Text text, object value)
        {
            text.text = value.ToString();
        }
    }
}