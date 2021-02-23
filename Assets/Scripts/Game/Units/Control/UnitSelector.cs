using System;
using Data;
using Game.Units.Unit_Types;
using LogicHelper;
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

        public void UnSelectUnit()
        {
            if (SelectedUnit == null)
                return;
            
            SelectedUnit.IsSelected = false;

            OnUnitDeSelect.Invoke(SelectedUnit);
            
            SelectedUnit = null;
        }

        public void SelectUnit(Unit unit)
        {
            print("select " + unit.name);

            SelectedUnit = unit;
            
            OnUnitSelected.Invoke(unit);

            unit.IsSelected = false;
            
            unit.IsSelected = true;
        }
        
        [ContextMenu("Test")]
        public void UpdateSelectedUnit()
        {
            if (SelectedUnit == null)
                return;

            SelectUnit(SelectedUnit);
        }
        
        public void OnUnitNeedSelect(Unit unit)
        {
            if (unit.gameParameters.IsEnemy)
                return;
            
            if (SelectedUnit == unit)
            {
                // Unselect
                
                UnSelectUnit();
                
                return;
            }
            
            if (SelectedUnit != null)
            {
                SelectedUnit.IsSelected = false;
                
                SelectedUnit = null;
            }

            // SELECT

            SelectUnit(unit);
        }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UnitLevelUpdater.Instance.OnUnitLevelUpdated += unit => UpdateSelectedUnit();
        }
    }
}
