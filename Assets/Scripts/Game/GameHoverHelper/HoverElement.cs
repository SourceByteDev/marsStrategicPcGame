using System;
using Data;
using LogicHelper;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.GameHoverHelper
{
    public class HoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PanelParameters panelParameters; 
        
        private HoverPanelControl _panelControl;
        
        public UnityAction<HoverElement> OnMouseEnter { get; set; }
        
        public UnityAction<HoverElement> OnMouseExit { get; set; }

        public PanelParameters Parameters
        {
            get
            {
                panelParameters.BuildParameters = GeneratePanelParameters();
                
                return panelParameters;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit?.Invoke(this);
        }

        private HoverPanel.BuildParameters GeneratePanelParameters()
        {
            var parameters = new HoverPanel.BuildParameters();

            switch (panelParameters.TypePanel)
            {
                case PanelParameters.TypePanelParameters.None:
                    parameters = panelParameters.BuildParameters;
                    break;
                
                case PanelParameters.TypePanelParameters.BuildUnit:
                    parameters = UnitBuilder.GenerateParametersOf(panelParameters.ToBuildData);
                    break;
                
                case PanelParameters.TypePanelParameters.SellCurrent:
                    parameters = new HoverPanel.BuildParameters();
                    break;
            }

            return parameters;
        }

        private void Start()
        {
            _panelControl = HoverPanelControl.Instance;

            OnMouseEnter += delegate
            {
                _panelControl.OnMouseEnterElement?.Invoke(this);
            };
            
            OnMouseExit += delegate
            {
                _panelControl.OnMouseExitElement?.Invoke(this);
            };
        }

        [Serializable]
        public struct PanelParameters
        {
            [SerializeField] private string title;

            [SerializeField] private string description;

            [SerializeField] private UnitData toBuildData;
            
            [SerializeField] private HoverPanel.BuildParameters buildParameters;

            [SerializeField] private TypePanelParameters typePanel;

            [SerializeField] private bool isSellButton;

            [Space(5)] [SerializeField] private UnitUpgrader.TypeVariableUpgrade typeVariable;

            [SerializeField] private UnitUpgrader.TypeUpgrade typeUpgrade;
            
            public string Title => title;

            public string Description => description;

            public bool IsSellButton => isSellButton;

            public bool IsUpgradeElement => typePanel == TypePanelParameters.UpgradeElement;

            public UnitUpgrader.TypeVariableUpgrade TypeVariable => typeVariable;

            public UnitUpgrader.TypeUpgrade TypeUpgrade => typeUpgrade;

            public HoverPanel.BuildParameters BuildParameters
            {
                get => buildParameters;
                set => buildParameters = value;
            }

            public UnitData ToBuildData => toBuildData;

            public TypePanelParameters TypePanel => typePanel;

            public enum TypePanelParameters
            {
                None,
                BuildUnit,
                SellCurrent,
                UpgradeElement,
                UpgradeMainHome
            }
        }
    }
}