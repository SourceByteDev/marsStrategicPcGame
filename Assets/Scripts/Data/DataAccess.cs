using System;
using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    public class DataAccess : MonoBehaviour
    {
        [FormerlySerializedAs("houseUnit")] [FormerlySerializedAs("homeUnit")] [SerializeField]
        private UnitData mainHouseUnit;

        [SerializeField] private UnitData workerUnit;

        private static DataAccess _instance;

        public static UnitData MainHouseUnit => _instance.mainHouseUnit;

        public static UnitData WorkerUnit => _instance.workerUnit;

        private void Awake()
        {
            _instance = this;
        }
    }
}