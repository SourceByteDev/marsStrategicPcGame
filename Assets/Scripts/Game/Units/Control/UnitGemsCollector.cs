using System;
using Data;
using GameUi;
using Manager;
using UnityEngine;

namespace Game.Units.Control
{
    public class UnitGemsCollector : MonoBehaviour
    {
        private ValuesManage _values;
        
        private void Awake()
        {
            _values = Managers.Values;
            
            Timer.Instance.OnSecond += delegate
            {
                var countOfWorkers = _values.CountOfLiveUnits(DataAccess.WorkerUnit.parameters.avatarSet);

                _values.values.CurrentGemsCount += countOfWorkers;
            };
        }
    }
}