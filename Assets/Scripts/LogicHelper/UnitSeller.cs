using System;
using Game.GameHoverHelper;
using Game.Units.Control;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace LogicHelper
{
    public class UnitSeller : MonoBehaviour
    {
        public static UnitSeller Instance { get; private set; }

        public static bool CanSellCurrent()
        {
            var selectedUnit = UnitSelector.Instance.SelectedUnit;

            var parameters = selectedUnit.gameParameters;
            
            var values = Managers.Values;

            // Check if it remove max supply and it will be full
            return !(values.IsSupplyFullIfSell(parameters.countSupply) && parameters.isGiveSupply);
        }
        
        public void TrySellSelectedUnit()
        {
            var selectedUnit = UnitSelector.Instance.SelectedUnit;

            var parameters = selectedUnit.gameParameters;
            
            var values = Managers.Values;

            if (!CanSellCurrent())
                return;

            values.values.CurrentGemsCount += Mathf.CeilToInt(parameters.price / 2f);
            
            if (parameters.isGiveSupply)
            {
                values.values.CurrentMaxSupply -= parameters.countSupply;
            }
            else
            {
                values.values.CurrentSupply -= parameters.countSupply;
            }
            
            UnitSelector.Instance.UnSelectUnit();
            
            UnitSpawner.Instance.RemoveUnit(selectedUnit);
        }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}