using System;
using System.Linq;
using Data;
using FetchUi;
using Game.Units.Control;
using LogicHelper;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class LaboratoryPanel : ControlPanel
    {
        [SerializeField] private LaboratoryWindows windows;

        [SerializeField] private LaboratoryButtons buttons;
        
        private UnitGameParameters lastParameters;

        private UnitSeller seller;

        private UnitUpgrader upgrader;
        
        public override void UpdateValues(UnitGameParameters parameters)
        {
            if (lastParameters != parameters)
                OpenMain();
            
            lastParameters = parameters;
            
            UpdateActiveButtons();
        }

        public void OpenMain()
        {
            windows.Open(windows.Main);
        }

        public void OpenGround()
        {
            windows.Open(windows.Ground);
        }

        public void OpenFly()
        {
            windows.Open(windows.Fly);
        }

        private void UpdateActiveButtons()
        {
            buttons.SellButton.Interactable = UnitSeller.CanSellCurrent();
            
            buttons.UpgradeButtons.ToList().ForEach(InitUpgradeButton);
        }
        
        private void InitGeneralButtons()
        {
            buttons.RemoveListeners();
            
            buttons.FlyButton.onClick.AddListener(OpenFly);
            
            buttons.GroundButton.onClick.AddListener(OpenGround);
            
            buttons.SellButton.onClick.AddListener(seller.TrySellSelectedUnit);
            
            buttons.BackButtons.ToList().ForEach(x => x.onClick.AddListener(OpenMain));
        }

        private void InitUpgradeButton(UpgradeButton upgradeButton)
        {
            var button = upgradeButton.Button;

            var upgradeType = upgradeButton.UpgradeType;

            var variableType = upgradeButton.VariableType;
            
            button.onClick.RemoveAllListeners();

            button.Interactable = upgrader.CanUpgradeOf(upgradeType, variableType);
            
            if (!button.Interactable)
                return;
            
            button.onClick.AddListener(delegate
            {
                upgrader.StartUpgradeTimer(new UnitUpgrader.UpgradeItem(upgradeType, variableType));
            });
        }
        
        private void Start()
        {
            seller = UnitSeller.Instance;

            upgrader = UnitUpgrader.Instance;

            InitGeneralButtons();
        }

        [Serializable]
        private struct LaboratoryWindows
        {
            [SerializeField] private GameObject main;

            [SerializeField] private GameObject ground;

            [SerializeField] private GameObject fly;

            public GameObject Main => main;

            public GameObject Ground => ground;

            public GameObject Fly => fly;
            
            private GameObject[] AllObjects => new GameObject[]
            {
                Main,
                Ground,
                Fly
            };

            public void HideAll()
            {
                AllObjects.ToList().ForEach(x => x.SetActive(false));
            }

            public void Open(GameObject window)
            {
                if (!AllObjects.Contains(window))
                    return;
                
                HideAll();
                
                window.SetActive(true);
            }
        }

        [Serializable]
        private struct LaboratoryButtons
        {
            [Header("General")] [SerializeField] private FetchButton groundButton;

            [SerializeField] private FetchButton flyButton;

            [SerializeField] private FetchButton sellButton;

            [SerializeField] private FetchButton[] backButtons;

            [Header("Upgrades")] [SerializeField] private UpgradeButton[] upgradeButtons;

            public FetchButton GroundButton => groundButton;

            public FetchButton FlyButton => flyButton;

            public FetchButton SellButton => sellButton;

            public FetchButton[] BackButtons => backButtons;

            public UpgradeButton[] UpgradeButtons => upgradeButtons;

            private FetchButton[] AllButtons => new []
            {
                groundButton,
                flyButton,
                sellButton
            };

            public void RemoveListeners()
            {
                AllButtons.ToList().ForEach(x => x.onClick.RemoveAllListeners());
                
                backButtons.ToList().ForEach(x => x.onClick.RemoveAllListeners());
            }
        }

        [Serializable]
        private struct UpgradeButton
        {
            [SerializeField] private UnitUpgrader.TypeUpgrade upgradeType;

            [SerializeField] private UnitUpgrader.TypeVariableUpgrade variableType;

            [SerializeField] private FetchButton button;

            public UnitUpgrader.TypeUpgrade UpgradeType => upgradeType;

            public UnitUpgrader.TypeVariableUpgrade VariableType => variableType;

            public FetchButton Button => button;
        }
    }
}