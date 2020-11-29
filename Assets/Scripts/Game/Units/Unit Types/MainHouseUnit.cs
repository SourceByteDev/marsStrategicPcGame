using System;
using Game.Units.Control;
using UnityEngine;

namespace Game.Units.Unit_Types
{
    public class MainHouseUnit : Unit, IUnitLevelUpdate
    {
        public VisualLevelData[] Visuals => visuals;

        [SerializeField] private VisualLevelData[] visuals;

        public override void OnUpdateIndexLevel(int index, bool isOnnew)
        {
            base.OnUpdateIndexLevel(index, isOnnew);
            
            visuals[index].UpdateUnit(this, isOnnew);
        }
    }
}