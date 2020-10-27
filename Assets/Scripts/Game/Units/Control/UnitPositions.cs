using System;
using System.Linq;
using Data;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Units.Control
{
    public class UnitPositions : MonoBehaviour
    {
        [SerializeField] private PositionParam[] allPositions;

        private UnitPosition lastPosition;
        
        public static UnitPositions Instance { get; private set; }

        public Vector2 GetPositionForNew(UnitData data)
        {
            if (allPositions.All(x => x.Data != data))
                return Vector2.zero;

            var foundParam = allPositions.ToList().Find(x => x.Data == data);

            var freeWayPoints = foundParam.WayPoints.Where(x => !x.IsBusy).ToArray();

            if (data.parameters.isRandomPosition)
            {
                return freeWayPoints[Random.Range(0, freeWayPoints.Length)].Position;
            }
            
            var withIndexPoint = freeWayPoints.Length <= 0 ? null : freeWayPoints.First();

            lastPosition = withIndexPoint;

            return withIndexPoint == null ? Vector2.zero : withIndexPoint.Position;
        }

        public void SetToLastUnit(Unit unit)
        {
            if (lastPosition == null)
                return;

            if (unit.gameParameters.isRandomPosition)
                return;
            
            lastPosition.BusyUnit = unit;
            
            lastPosition = null;
        }
        
        [ContextMenu("InActiveAll")]
        private void InActiveVisualsInChildren()
        {
            SetActiveVisuals(false);
        }

        [ContextMenu("ActiveAll")]
        private void ActiveVisualsInChildren()
        {
            SetActiveVisuals(true);
        }

        private void SetActiveVisuals(bool isActive)
        {
            allPositions.ToList().ForEach(x => x.WayPoints.ToList().ForEach(y =>
            {
                var yTransform = y.transform;

                for (var i = 0; i < yTransform.childCount; i++)
                {
                    yTransform.GetChild(i).gameObject.SetActive(isActive);
                }
            }));
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UnitSpawner.Instance.onUnitSpawned += SetToLastUnit;
        }

        [Serializable]
        public struct PositionParam
        {
            [SerializeField] private UnitData data;

            [SerializeField] private UnitPosition[] wayPoints;

            public UnitData Data => data;

            public UnitPosition[] WayPoints => wayPoints;
        }
    }
}