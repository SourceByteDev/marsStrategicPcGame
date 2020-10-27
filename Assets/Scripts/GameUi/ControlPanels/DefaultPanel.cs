using Data;
using FetchUi;
using Game.Units.Control;
using LogicHelper;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class DefaultPanel : ControlPanel
    {
        [SerializeField] private FetchButton sellButton;
        
        public override void UpdateValues(UnitGameParameters parameters)
        {
            sellButton.Interactable = UnitSeller.CanSellCurrent();

            sellButton.onClick.RemoveAllListeners();
            
            if (!UnitSeller.CanSellCurrent())
                return;
            
            sellButton.onClick.AddListener(delegate
            {
                UnitSeller.Instance.TrySellSelectedUnit();
            });
        }
    }
}