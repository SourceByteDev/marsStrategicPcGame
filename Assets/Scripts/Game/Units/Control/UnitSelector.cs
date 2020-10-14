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
            if (SelectedUnit == null)
                return;
            
            SelectedUnit = SelectedUnit;
            
            OnUnitSelected.Invoke(SelectedUnit);
        }
        
        public void OnUnitNeedSelect(Unit unit)
        {
            if (SelectedUnit == unit)
            {
                // Unselect
                
                SelectedUnit = null;
                
                OnUnitDeSelect?.Invoke(unit);
                
                unit.GetComponent<MeshRenderer>().material.shader = Shader.Find("Spine/Skeleton");
                
                return;
            }
            if (SelectedUnit != null)
            {
                SelectedUnit.GetComponent<MeshRenderer>().material.shader = Shader.Find("Spine/Skeleton");
                
                SelectedUnit = null;
            }

            // SELECT
            
            print("select " + unit.name);

            SelectedUnit = unit;
            
            OnUnitSelected.Invoke(unit);
            
            unit.GetComponent<MeshRenderer>().material.shader = Shader.Find("Spine/Outline/Skeleton");
        }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}
