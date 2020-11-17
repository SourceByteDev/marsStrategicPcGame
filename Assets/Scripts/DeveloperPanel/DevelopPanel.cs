using System;
using Game.Units.Control;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeveloperPanel
{
    public class DevelopPanel : MonoBehaviour
    {
        [SerializeField] private TimeParameters timeParameters;

        [SerializeField] private GemsParameters gemsParameters;

        private static void SetGemsCount(int gems)
        {
            Managers.Values.values.CurrentGemsCount = gems;
            
            UnitSelector.Instance.UpdateSelectedUnit();
        }

        private void SetTimeScale(float value)
        {
            var roundValue = (float) Math.Round(value, 1);
            
            timeParameters.TimeSlider.value = roundValue;

            Time.timeScale = roundValue;

            timeParameters.CurrentTimeScale.text = $"x{roundValue}";
        }

        private void Start()
        {
            SetTimeScale(1);
            
            timeParameters.TimeSlider.onValueChanged.AddListener(SetTimeScale);
            
            gemsParameters.SetGemsButton.onClick.AddListener(delegate
            {
                if (!int.TryParse(gemsParameters.GemsCountField.text, out var count))
                    return;
                
                SetGemsCount(count);
            });
        }

        [Serializable]
        private struct TimeParameters
        {
            [SerializeField] private TMP_Text currentTimeScale;
            
            [SerializeField] private Slider timeSlider;

            public TMP_Text CurrentTimeScale => currentTimeScale;

            public Slider TimeSlider => timeSlider;
        }

        [Serializable]
        private struct GemsParameters
        {
            [SerializeField] private TMP_InputField gemsCountField;

            [SerializeField] private Button setGemsButton;

            public TMP_InputField GemsCountField => gemsCountField;

            public Button SetGemsButton => setGemsButton;
        }
    }
}