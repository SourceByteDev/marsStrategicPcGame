using System;
using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Units.Control
{
    public class UnitSelector : MonoBehaviour
    {
        public static UnitSelector Instance;

        public UnityAction<Unit> OnUnitSelected { get; set; }
        
        public UnityAction<Unit> OnUnitDeSelect { get; set; }
        
        public Unit SelectedUnit { get; private set; }

        public void UpdateSelectedUnit()
        {
            SelectedUnit = SelectedUnit;
            
            OnUnitSelected.Invoke(SelectedUnit);
        }
        
        public void OnUnitNeedSelect(Unit unit)
        {
            if (SelectedUnit == unit)
            {
                OnUnitDeSelect?.Invoke(unit);

                SelectedUnit = null;
                
                return;
            }

            print("select " + unit.name);

            SelectedUnit = unit;
            
            OnUnitSelected.Invoke(unit);
        }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}
