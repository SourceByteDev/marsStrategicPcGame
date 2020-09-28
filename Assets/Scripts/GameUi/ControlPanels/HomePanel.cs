using System;
using Data;
using FetchUi;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class HomePanel : ControlPanel
    {
        [SerializeField] private Buttons buttons;
        
        public override void UpdateValues(UnitGameParameters parameters)
        {
            var currentLevel = parameters.currentLevel;

            buttons.BuildTier1.Interactable = currentLevel >= 0;

            buttons.BuildTier2.Interactable = currentLevel >= 1;

            buttons.SpecialAttack.Interactable = currentLevel >= 2;

            buttons.UpgradeBase.Interactable = !parameters.IsMaxLevelNow;
        }
        
        [Serializable]
        public struct Buttons
        {
            [SerializeField] private FetchButton buildTier1;

            [SerializeField] private FetchButton buildTier2;

            [SerializeField] private FetchButton buildWorker;

            [SerializeField] private FetchButton upgradeBase;

            [SerializeField] private FetchButton specialAttack;
            
            public FetchButton BuildTier1 => buildTier1;

            public FetchButton BuildTier2 => buildTier2;

            public FetchButton BuildWorker => buildWorker;

            public FetchButton UpgradeBase => upgradeBase;

            public FetchButton SpecialAttack => specialAttack;
        }
    }
}