using System;
using System.Linq;
using Data;
using FetchUi;
using Game.Units.Control;
using Game.Units.Unit_Types;
using LogicHelper;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class HomePanel : ControlPanel
    {
        [SerializeField] private Buttons buttons;

        [SerializeField] private IntroductionValues introduction;

        private UnitGameParameters lastParameters;
        
        public override void UpdateValues(UnitGameParameters parameters)
        {
            if (lastParameters != parameters)
                OpenMainWindow();

            lastParameters = parameters;
            
            var currentLevel = parameters.currentLevel;

            UpdateUnitBuyButtons();
            
            buttons.BuildTier1.Interactable = currentLevel >= 0;

            buttons.BuildTier2.Interactable = currentLevel >= 1;

            buttons.SpecialAttack.Interactable = currentLevel >= 2;

            buttons.UpgradeBase.Interactable = !parameters.IsMaxLevelNow;

            buttons.BuildTier1.onClick.RemoveAllListeners();
            
            buttons.BuildTier1.onClick.AddListener(OpenTier1);
            
            buttons.BuildTier2.onClick.RemoveAllListeners();
            
            buttons.BuildTier2.onClick.AddListener(OpenTier2);

            buttons.CancelButton.Interactable = UnitProcessBuild.IsAnyBuildsByUnit(UnitSelector.Instance.SelectedUnit);
            
            buttons.CancelButton.onClick.RemoveAllListeners();
            
            buttons.CancelButton.onClick.AddListener(UnitProcessBuild.CancelLastBuildInSelected);
            
            buttons.BacksButton.ToList().ForEach(x =>
            {
                x.onClick.RemoveAllListeners();
                
                x.onClick.AddListener(OpenMainWindow);
            });
        }

        private void UpdateUnitBuyButtons()
        {
            var buyButtons = buttons.BuyUnitButtons;

            buyButtons.ToList().ForEach(x =>
            {
                var button = x.Button;

                var data = x.Data;

                var canBuy = UnitBuilder.CanBeUnitBuild(data);

                button.Interactable = canBuy;

                button.onClick.RemoveAllListeners();
                
                if (!canBuy)
                    return;
                
                button.onClick.AddListener(delegate
                {
                    UnitBuilder.AddUnitCurrentToBuild(data);
                });
            });
        }
        
        private void OpenMainWindow()
        {
            introduction.MainPart.SetActive(true);
            
            introduction.BuildTier1Part.SetActive(false);
            
            introduction.BuildTier2Part.SetActive(false);
        }

        private void OpenTier1()
        {
            introduction.MainPart.SetActive(false);
            
            introduction.BuildTier1Part.SetActive(true);
            
            introduction.BuildTier2Part.SetActive(false);
        }

        private void OpenTier2()
        {
            introduction.MainPart.SetActive(false);
            
            introduction.BuildTier1Part.SetActive(false);
            
            introduction.BuildTier2Part.SetActive(true);
        }

        private void Awake()
        {
            UnitSelector.Instance.OnUnitDeSelect += delegate(Unit unit)
            {
                lastParameters = null;
            };
        }

        [Serializable]
        public struct Buttons
        {
            [SerializeField] private FetchButton buildTier1;

            [SerializeField] private FetchButton buildTier2;

            [SerializeField] private FetchButton buildWorker;

            [SerializeField] private FetchButton upgradeBase;

            [SerializeField] private FetchButton specialAttack;

            [SerializeField] private FetchButton[] backsButton;

            [SerializeField] private FetchButton cancelButton;

            [SerializeField] private UnitBuyButton[] buyUnitButtons;
            
            public FetchButton BuildTier1 => buildTier1;

            public FetchButton BuildTier2 => buildTier2;

            public FetchButton BuildWorker => buildWorker;

            public FetchButton UpgradeBase => upgradeBase;

            public FetchButton SpecialAttack => specialAttack;

            public FetchButton[] BacksButton => backsButton;

            public UnitBuyButton[] BuyUnitButtons => buyUnitButtons;

            public FetchButton CancelButton => cancelButton;
        }

        [Serializable]
        public struct IntroductionValues
        {
            [SerializeField] private GameObject mainPart;

            [SerializeField] private GameObject buildTier1Part;

            [SerializeField] private GameObject buildTier2Part;

            public GameObject MainPart => mainPart;

            public GameObject BuildTier1Part => buildTier1Part;

            public GameObject BuildTier2Part => buildTier2Part;
        }
    }
    
    [Serializable]
    public struct UnitBuyButton
    {
        [SerializeField] private FetchButton button;

        [SerializeField] private UnitData data;

        public FetchButton Button => button;

        public UnitData Data => data;
    }
}