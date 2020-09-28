using System;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;

namespace LogicHelper
{
    public class UnitLevelUpdater : MonoBehaviour
    {
        public static UnitLevelUpdater Instance;

        public void UpdateLevelOfUnit(Unit unit)
        {
            if (unit.gameParameters.IsMaxLevelNow)
                return;
            
            if (!Managers.Values.values.TryRemoveGems(unit.gameParameters.CurrentPriceUpgrade))
                return;

            unit.gameParameters.currentLevel++;
        }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}