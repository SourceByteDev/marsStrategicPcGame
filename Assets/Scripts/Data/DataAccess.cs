using System;
using UnityEngine;

namespace Data
{
    public class DataAccess : MonoBehaviour
    {
        public UnitData homeUnit;

        private static DataAccess _instance;

        public static UnitData HomeUnit => _instance.homeUnit;

        private void Awake()
        {
            _instance = this;
        }
    }
}