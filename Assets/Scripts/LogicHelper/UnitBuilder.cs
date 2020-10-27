using System;
using Data;
using Game.Units.Control;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;

namespace LogicHelper
{
    public class UnitBuilder : MonoBehaviour
    {
        [SerializeField] private UnitData workerData;
        
        public static UnitBuilder Instance { get; private set; }

        public static bool CanBeUnitBuild(UnitData data)
        {
            var parameters = data.parameters;

            var countLiveUnits = Managers.Values.CountOfAllTypes(data.parameters.avatarSet);
            
            // Check worker data
            if (data == Instance.workerData)
            {
                return countLiveUnits < Managers.Values.CurrentMaxCountWorkers;
            }

            return countLiveUnits < parameters.maxCountToBuild || parameters.maxCountToBuild <= 0;
        }

        public static void AddUnitCurrentToBuild(UnitData unitBuild)
        {
            var parameters = unitBuild.parameters;

            var countLiveUnits = Managers.Values.CountOfAllTypes(unitBuild.parameters.avatarSet);

            var price = parameters.priceModiferForLiveCount
                ? parameters.priceBuild * (countLiveUnits + 1)
                : parameters.priceBuild;

            if (parameters.priceEnumerationForLiveCount)
            {
                price += parameters.enumerationPrice * countLiveUnits;
            }

            var selectedUnit = UnitSelector.Instance.SelectedUnit;

            // Supply check
            if (Managers.Values.IsSupplyFull(parameters.countSupply) && !parameters.isGiveSupply)
                return;
            // Max count builds check
            if (!CanBeUnitBuild(unitBuild))
                return;
            // Max in build selected unit builds
            if (selectedUnit.gameParameters.currentBuilds.Count >= 4)
                return;
            // Check price remove coins
            if (!Managers.Values.values.TryRemoveGems(price))
                return;

            selectedUnit.gameParameters.currentBuilds.Add(
                new BuildUnitParameters(parameters.timeBuildSeconds, unitBuild));

            UnitSelector.Instance.UpdateSelectedUnit();

            // Check maybe it give supply to max supply
            if (parameters.isGiveSupply)
                Managers.Values.values.CurrentMaxSupply += parameters.countSupply;
            else
                Managers.Values.values.CurrentSupply += parameters.countSupply;
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}