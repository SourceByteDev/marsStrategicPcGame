using System;
using System.Text;
using Game.Units.Control;
using LogicHelper;
using TMPro;
using UnityEngine;

namespace Game.GameHoverHelper
{
    public class HoverPanel : MonoBehaviour
    {
        [SerializeField] private PanelTexts texts;

        public void Init(string title, string description, BuildParameters buildParameters, bool sellButton,
            bool isUpgrade, HoverElement.PanelParameters parameters)
        {
            var parametersForm = $"{buildParameters.Price} crystals ";

            var builder = new StringBuilder(parametersForm);

            builder.Append(buildParameters.Supply > 0 ? $"{buildParameters.Supply} supply" : null);

            var buildTimeForm = $"{buildParameters.Time} sec";

            texts.Title.text = title;

            texts.Description.text = description;

            texts.BuildParams.text = buildParameters.IsNull ? "" : builder.ToString();

            texts.BuildTime.text = buildParameters.IsNull ? "" : buildTimeForm;

            if (sellButton)
            {
                var currentUnit = UnitSelector.Instance.SelectedUnit;

                var setName = currentUnit.gameParameters.unitName;

                texts.Title.text = $"Sell";

                texts.BuildParams.text = "";

                texts.BuildTime.text = "";

                texts.Description.text = $"If you sell any of your {setName} you only get half the money back";
            }

            if (isUpgrade)
            {
                var setType = parameters.TypeUpgrade.ToString();

                var setVariable = parameters.TypeVariable.ToString();

                var newBuildParameters =
                    UnitUpgrader.Instance.GenerateParametersOf(parameters.TypeUpgrade, parameters.TypeVariable);
                
                texts.BuildParams.text = $"{newBuildParameters.Price} crystals ";

                texts.BuildTime.text = $"{newBuildParameters.Time} sec";
                
                texts.Title.text = $"Upgrade {setType} {setVariable}";
            }
        }

        [Serializable]
        public struct BuildParameters
        {
            [SerializeField] private int price;

            [SerializeField] private int supply;

            [SerializeField] private int time;

            [SerializeField] private bool isGiveSupply;

            public int Price
            {
                get => price;
                set => price = value;
            }

            public int Supply
            {
                get => supply;
                set => supply = value;
            }

            public int Time
            {
                get => time;
                set => time = value;
            }

            public bool IsGiveSupply
            {
                get => isGiveSupply;
                set => isGiveSupply = value;
            }

            public bool IsNull => Price == 0 && Supply == 0 && Time == 0;

            public BuildParameters(int price, int supply, int time, bool isGiveSupply)
            {
                this.price = price;
                this.supply = supply;
                this.time = time;
                this.isGiveSupply = isGiveSupply;
            }
        }

        [Serializable]
        private struct PanelTexts
        {
            [SerializeField] private TMP_Text title;

            [SerializeField] private TMP_Text buildParams;

            [SerializeField] private TMP_Text buildTime;

            [SerializeField] private TMP_Text description;

            public TMP_Text Title => title;

            public TMP_Text BuildParams => buildParams;

            public TMP_Text BuildTime => buildTime;

            public TMP_Text Description => description;
        }
    }

    public static class HoverPanelExtensions
    {
        public static void Init(this HoverPanel panel, HoverElement.PanelParameters parameters)
        {
            panel.Init(parameters.Title, parameters.Description, parameters.BuildParameters, parameters.IsSellButton,
                parameters.IsUpgradeElement, parameters);
        }
    }
}