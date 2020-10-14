using System;
using System.Linq;
using Data;
using FetchUi;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class HomePanel : ControlPanel
    {
        [SerializeField] private Buttons buttons;

        [SerializeField] private IntroductionValues introduction;

        private UnitGameParameters _lastParameters;
        
        public override void UpdateValues(UnitGameParameters parameters)
        {
            if (_lastParameters == parameters)
                return;

            _lastParameters = parameters;
            
            var currentLevel = parameters.currentLevel;

            buttons.BuildTier1.Interactable = currentLevel >= 0;

            buttons.BuildTier2.Interactable = currentLevel >= 1;

            buttons.SpecialAttack.Interactable = currentLevel >= 2;

            buttons.UpgradeBase.Interactable = !parameters.IsMaxLevelNow;
            
            CloseAllOtherWindows();
            
            buttons.BuildTier1.onClick.RemoveAllListeners();
            
            buttons.BuildTier1.onClick.AddListener(OpenTier1);
            
            buttons.BuildTier2.onClick.RemoveAllListeners();
            
            buttons.BuildTier2.onClick.AddListener(OpenTier2);
            
            buttons.BacksButton.ToList().ForEach(x =>
            {
                x.onClick.RemoveAllListeners();
                
                x.onClick.AddListener(CloseAllOtherWindows);
            });
        }

        private void CloseAllOtherWindows()
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
        
        [Serializable]
        public struct Buttons
        {
            [SerializeField] private FetchButton buildTier1;

            [SerializeField] private FetchButton buildTier2;

            [SerializeField] private FetchButton buildWorker;

            [SerializeField] private FetchButton upgradeBase;

            [SerializeField] private FetchButton specialAttack;

            [SerializeField] private FetchButton[] backsButton;
            
            public FetchButton BuildTier1 => buildTier1;

            public FetchButton BuildTier2 => buildTier2;

            public FetchButton BuildWorker => buildWorker;

            public FetchButton UpgradeBase => upgradeBase;

            public FetchButton SpecialAttack => specialAttack;

            public FetchButton[] BacksButton => backsButton;
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
}