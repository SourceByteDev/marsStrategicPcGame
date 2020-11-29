using System;
using Data;
using Game.Units.Control;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameUi
{
    public class GameUi : MonoBehaviour
    {
        [Header("Introduction values")] public Button menuButton;

        [Space(5)] public Text gemsCountText;

        public Text timerText;

        public Text soldiersCountText;

        [Header("Preferences")] public Timer timer;

        public CentralPanel centralPanel;

        [FormerlySerializedAs("controlPanel")] public ControlManagePanel controlManagePanel;

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

            Additional.SetValueToText(soldiersCountText, $"{Values.CurrentSupply}/{Values.CurrentMaxSupply}");

            UpdateTimer();
        }

        public void InitSelectedParameters(Unit unit)
        {
            centralPanel.InitItem(unit.gameParameters);

            controlManagePanel.OpenRightPanel(unit.gameParameters.controlType,
                Managers.Values.GetLiveUnitByUnit(unit).parameters.poolType != PoolType.PlayerSoldier);
        }

        public void InActiveSelect(Unit unit = null)
        {
            centralPanel.InActive();

            controlManagePanel.InActive();
        }

        private void UpdateTimer()
        {
            var currentSeconds = Values.CurrentTimeSeconds;

            var minutes = Mathf.FloorToInt(currentSeconds / 60f);

            var seconds = currentSeconds - minutes * 60;

            var isAddSeconds = seconds > 9 ? "" : "0";

            var isAddMinutes = minutes > 9 ? "" : "0";

            if (timerText == null)
                return;

            timerText.text = isAddMinutes + minutes + ":" + isAddSeconds + seconds;
        }

        private void Awake()
        {
            UnitSelector.Instance.OnUnitSelected += InitSelectedParameters;

            UnitSelector.Instance.OnUnitDeSelect += InActiveSelect;

            Managers.Values.onSomeValueChanged += UpdateUi;
        }

        private void Start()
        {
            UpdateUi();

            timer.StartTimer();

            InActiveSelect();
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
            if (text == null)
                return;

            text.text = value.ToString();
        }
    }
}