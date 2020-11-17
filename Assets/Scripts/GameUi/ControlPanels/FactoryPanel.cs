using System;
using Data;
using FetchUi;
using LogicHelper;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class FactoryPanel : ControlPanel
    {
        [SerializeField] private FactoryButtons buttons;

        public override void UpdateValues(UnitGameParameters parameters)
        {
            UpdateActiveButtons();
        }

        private void UpdateActiveButtons()
        {
            var sell = buttons.Sell;

            sell.Interactable = UnitSeller.CanSellCurrent();
        }
        
        private void InitButtons()
        {
            var sell = buttons.Sell;
            
            sell.onClick.RemoveAllListeners();
            
            sell.onClick.AddListener(delegate
            {
                UnitSeller.Instance.TrySellSelectedUnit();
            });

            var buyTank = buttons.TankBuy.Button;

            var tankData = buttons.TankBuy.Data;
            
            buyTank.onClick.RemoveAllListeners();
            
            buyTank.onClick.AddListener(delegate
            {
                UnitBuilder.AddUnitCurrentToBuild(tankData);
            });
        }

        private void Start()
        {
            InitButtons();
        }

        [Serializable]
        private struct FactoryButtons
        {
            [SerializeField] private FetchButton sell;

            [SerializeField] private UnitBuyButton tankBuy;

            public FetchButton Sell => sell;

            public UnitBuyButton TankBuy => tankBuy;
        }
    }
}