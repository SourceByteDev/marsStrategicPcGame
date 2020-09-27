using System;
using Data;
using Game.Units.Control;
using UnityEngine;

namespace Game.Units.Unit_Types
{
    public abstract class Unit : MonoBehaviour
    {
        public UnitGameParameters gameParameters;

        public void InitParameters(UnitData data)
        {
            gameParameters = new UnitGameParameters(data);
        }

        public void InitParameters(UnitGameParameters parameters)
        {
            gameParameters = parameters;
        }

        private void OnMouseDown()
        {
            UnitSelector.Instance.OnUnitNeedSelect(this);
        }
    }

    public enum ControlType
    {
        None,
        House,
        Barracks,
        Soldier
    }

    
}