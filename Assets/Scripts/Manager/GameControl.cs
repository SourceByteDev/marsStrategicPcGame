using UnityEngine;
using Addone;
using Data;
using Game.Units.Control;

namespace Manager
{
    public class GameControl : MonoBehaviour
    {
        public Vector2 homeStartPosition = new Vector2(-18, -5);
        
        private static Variable FirstLaunch => SaveVariables.firstLaunch;

        [ContextMenu("Reset all")]
        public void ResetAllData()
        {
            PlayerPrefs.DeleteAll();
            
            Managers.Values.ResetAllValues();
        }
        
        public static void OnGameStarted()
        {
            if (FirstLaunch.GetInt == 0)
            {
                Managers.GameControl.CreateStartUnit();
            }
            else
            {
                Managers.GameControl.SpawnSavedUnits();
            }

            FirstLaunch.SetInt(1);
        }

        public static void OnGameOver()
        {
            
        }

        private void SpawnSavedUnits()
        {
            var savedUnits = Managers.Values.values.LiveUnits;

            if (savedUnits.Count < 0)
                return;

            foreach (var unitData in savedUnits)
            {
                UnitSpawner.Instance.SpawnWithOutSave(unitData.parameters, unitData.position);
            }
        }
        
        private void CreateStartUnit()
        {
            var unitData = DataAccess.MainHouseUnit;
            
            Managers.Values.values.CurrentMaxSupply = 10;
            
            UnitSpawner.Instance.SpawnUnit(unitData, homeStartPosition);

            var workerUnit = DataAccess.WorkerUnit;

            var startPositionWorker = homeStartPosition - Vector2.right * 5;
            
            UnitSpawner.Instance.SpawnUnit(workerUnit, startPositionWorker);

            Managers.Values.values.CurrentSupply++;
        }
    }
}