using System;
using Game.Units.Unit_Types;
using UnityEngine;

namespace Game.Units.Control
{
    public class UnitPosition : MonoBehaviour
    {
        public Unit BusyUnit { get; set; }

        public bool IsBusy => BusyUnit != null;

        public Vector2 Position => transform.position;
    }
}