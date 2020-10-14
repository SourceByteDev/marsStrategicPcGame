using System;
using System.Linq;
using Data;
using Manager;
using UnityEngine;

namespace Game.Units.Control
{
    public class UnitPositions : MonoBehaviour
    {
        [SerializeField] private PositionParam[] allPositions;

        public static UnitPositions Instance { get; private set; }

        public Vector2 GetPositionForNew(UnitData data)
        {
            if (allPositions.All(x => x.Data != data))
                return Vector2.zero;
            
            var foundParam = allPositions.ToList().Find(x => x.Data == data);

            var wayPoints = foundParam.WayPoints;

            var countOfCurrentType = Managers.Values.CountOfTypes(data.parameters.controlType);

            if (wayPoints.Length <= countOfCurrentType)
                return Vector2.zero;

            return wayPoints[countOfCurrentType].position;
        }
        
        private void Awake()
        {
            Instance = this;
        }

        [Serializable]
        public struct PositionParam
        {
            [SerializeField] private UnitData data;

            [SerializeField] private Transform[] wayPoints;

            public UnitData Data => data;

            public Transform[] WayPoints => wayPoints;
        }
    }
}