using System;
using Data;
using FetchUi;
using LogicHelper;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class FlyPortPanel : ControlPanel
    {
        [SerializeField] private FlyPortButtons buttons;

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

            var flierBuy = buttons.FlierBuy.Button;

            var flierData = buttons.FlierBuy.Data;
            
            flierBuy.onClick.RemoveAllListeners();
            
            flierBuy.onClick.AddListener(delegate
            {
                UnitBuilder.AddUnitCurrentToBuild(flierData);
            });
        }

        private void Start()
        {
            InitButtons();
        }

        [Serializable]
        private struct FlyPortButtons
        {
            [SerializeField] private FetchButton sell;

            [SerializeField] private UnitBuyButton flierBuy;

            public FetchButton Sell => sell;

            public UnitBuyButton FlierBuy => flierBuy;
        }
    }
}