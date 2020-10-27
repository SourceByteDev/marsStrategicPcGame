using System;
using System.Linq;
using Data;
using Game.Units.Control;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LogicHelper
{
    public class UnitProcessBuild : MonoBehaviour
    {
        private static bool _lastSpawnedUpper;
        
        public static UnitProcessBuild Instance { get; private set; }
        
        public static void AddAllSeconds()
        {
            var liveUnits = Managers.Values.values.LiveUnits;

            if (liveUnits.Count <= 0)
                return;

            foreach (var unit in liveUnits.ToList().Where(unit => unit.parameters.currentBuilds.Count > 0))
            {
                unit.parameters.currentBuilds[0].AddSeconds();
                
                if (UnitSelector.Instance.SelectedUnit == null)
                    continue;
                
                if (unit.parameters == UnitSelector.Instance.SelectedUnit.gameParameters)
                    UnitSelector.Instance.UpdateSelectedUnit();
            }
        }

        public static void BuildNewFromOther(BuildUnitParameters parameters)
        {
            var liveUnits = Managers.Values.values.LiveUnits;

            var foundedUnits = liveUnits.Find(x => 
                x.parameters.currentBuilds.Contains(parameters));

            var unitData = parameters.toBuildUnit;
            
            foundedUnits.parameters.currentBuilds.Remove(parameters);
            
            UnitSpawner.Instance.SpawnUnit(unitData, UnitPositions.Instance.GetPositionForNew(unitData));
        }

        public static bool IsAnyBuildsByUnit(Unit unit)
        {
            return unit.gameParameters.currentBuilds.Count > 0;
        }

        public static void CancelLastBuildInSelected()
        {
            var unit = UnitSelector.Instance.SelectedUnit;
            
            if (!IsAnyBuildsByUnit(unit))
                return;

            var currentBuilds = unit.gameParameters.currentBuilds;
            
            currentBuilds.RemoveAt(currentBuilds.Count - 1);
            
            UnitSelector.Instance.UpdateSelectedUnit();
        }

        private static Vector2 GetNearPosFromLast()
        {
            var lastUnit = UnitSpawner.Instance.currentUnits.First();

            var addX = lastUnit.GetComponent<MeshRenderer>().bounds.size.x;
            
            addX -= Random.Range(.1f, .2f);

            var countUnits = UnitSpawner.Instance.currentUnits.Count;

            var toAddModiferX = countUnits - 2;

            if (toAddModiferX < 0)
                toAddModiferX = 0;

            var modiferX = (UnitSpawner.Instance.currentUnits.Count > 1 ? 1 : -1) + toAddModiferX;

            addX *= modiferX;
            
            var returnPos = (Vector2) lastUnit.transform.position;

            returnPos.x += addX;
            
            returnPos.y += _lastSpawnedUpper ? Random.Range(.05f, .15f) : Random.Range(.3f, .45f);

            _lastSpawnedUpper = !_lastSpawnedUpper;

            return returnPos;
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}