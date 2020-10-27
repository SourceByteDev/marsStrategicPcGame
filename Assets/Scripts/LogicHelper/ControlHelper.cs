using System;
using Game.Units.Control;
using UnityEngine;

namespace LogicHelper
{
    public class ControlHelper : MonoBehaviour
    {
        public static ControlHelper Instance { get; private set; }

        public void TryUpdateLevelSelectedUnit()
        {
            var selectedUnit = UnitSelector.Instance.SelectedUnit;
            
            UnitLevelUpdater.Instance.UpdateLevelOfUnit(selectedUnit);

            UnitSelector.Instance.UpdateSelectedUnit();
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}