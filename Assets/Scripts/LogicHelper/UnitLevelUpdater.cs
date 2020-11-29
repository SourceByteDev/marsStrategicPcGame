using System;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace LogicHelper
{
    public sealed class UnitLevelUpdater : MonoBehaviour
    {
        public static UnitLevelUpdater Instance { get; private set; }

        public event UnityAction<Unit> OnUnitLevelUpdated;

        public void UpdateLevelOfUnit(Unit unit)
        {
            if (unit.gameParameters.IsMaxLevelNow)
                return;
            
            if (!Managers.Values.values.TryRemoveGems(unit.gameParameters.CurrentPriceUpgrade))
                return;

            unit.gameParameters.currentLevel++;
            
            OnOnUnitLevelUpdated(unit);
        }
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnOnUnitLevelUpdated(Unit unit)
        {
            OnUnitLevelUpdated?.Invoke(unit);
        }
    }
}