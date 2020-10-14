using Data;
using Game.Units.Control;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;

namespace LogicHelper
{
    public class UnitBuilder : MonoBehaviour
    {
        public void AddUnitCurrentToBuild(UnitData unitBuild)
        {
            var price = unitBuild.parameters.priceBuild;

            var selectedUnit = UnitSelector.Instance.SelectedUnit;

            // Max count builds check
            if (Managers.Values.CountOfTypes(unitBuild.parameters.controlType) >= unitBuild.parameters.maxCountToBuild)
                return;
            // Max in build selected unit builds
            if (selectedUnit.gameParameters.currentBuilds.Count >= 4)
                return;
            // Check price remove coins
            if (!Managers.Values.values.TryRemoveGems(price))
                return;
            
            selectedUnit.gameParameters.currentBuilds.Add(
                new BuildUnitParameters(unitBuild.parameters.timeBuildSeconds, unitBuild));

            UnitSelector.Instance.UpdateSelectedUnit();
        }
    }
}