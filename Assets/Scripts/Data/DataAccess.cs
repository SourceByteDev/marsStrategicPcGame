using System;
using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    public class DataAccess : MonoBehaviour
    {
        [FormerlySerializedAs("houseUnit")] [FormerlySerializedAs("homeUnit")] [SerializeField] private UnitData mainHouseUnit;

        private static DataAccess _instance;

        public static UnitData MainHouseUnit => _instance.mainHouseUnit;

        private void Awake()
        {
            _instance = this;
        }
    }
}