using System;
using Common.Extensions;
using Data;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Game.Units.Control
{
    public class UnitMover : MonoBehaviour
    {
        [SerializeField] private WorkerParameters workerParameters;

        [SerializeField] private InfantryParameters infantriesParameters;

        public UnityAction<Unit> OnGotTarget { get; set; }

        private void OnUnitGotTarget(Unit unit)
        {
            switch (unit)
            {
                case WorkerUnit worker:
                    print("got target "  + worker.MoveState);
                    
                    switch (worker.MoveState)
                    {
                        case WorkerMoveState.CollectingGems:
                            worker.Target = workerParameters.BasePoint;
                            
                            worker.MoveState = WorkerMoveState.GoToHome;
                            break;
                        case WorkerMoveState.GoToHome:
                            worker.Target = workerParameters.GemsPoint;

                            worker.MoveState = WorkerMoveState.GoToGems;
                            break;
                        case WorkerMoveState.GoToGems:
                            worker.MoveState = WorkerMoveState.CollectingGems;
                            break;
                    }
                    break;
                
                case InfantryUnit infantry:

                    switch (infantry.MoveState)
                    {
                        case InfantryMoveState.MovingToPoint:
                            infantry.MoveState = InfantryMoveState.WaitingTarget;
                            break;
                    }
                    
                    break;
            }
        }

        private void OnUnitSpawned(Unit unit)
        {
            var isUnitMover = unit is IUnitMover;

            if (!isUnitMover)
                return;

            var unitMover = (IUnitMover) unit;

            var unitParameters = unit.gameParameters;

            var unitMoveParameters = unitParameters.moveParameters;

            unitMover.Parameters = unit.gameParameters;

            switch (unit)
            {
                case WorkerUnit worker:
                    var workerMoveParameters = unitMoveParameters.WorkerMove;

                    unitMover.Target = workerMoveParameters.WorkerMoveState == WorkerMoveState.GoToGems
                        ? workerParameters.GemsPoint
                        : workerParameters.BasePoint;
                    break;
                case InfantryUnit infantry:

                    var infantryParameters = unitMoveParameters.InfantryMove;

                    unitMover.Target = infantryParameters.MoveState == InfantryMoveState.MovingToPoint
                        ? infantriesParameters.FieldPoint
                        : Vector2.zero;

                    break;
            }
            
            unitMover.OnGotTarget += OnGotTarget;
        }

        private void Awake()
        {
            UnitSpawner.Instance.onUnitSpawned += OnUnitSpawned;
            
            OnGotTarget += OnUnitGotTarget;
        }

        [Serializable]
        public struct WorkerParameters
        {
            [SerializeField] private Transform[] gemsPoints;

            [SerializeField] private Transform[] basePoints;

            public Vector2 GemsPoint => gemsPoints[Random.Range(0, gemsPoints.Length)].position;

            public Vector2 BasePoint => basePoints[Random.Range(0, basePoints.Length)].position;
        }

        [Serializable]
        public struct InfantryParameters
        {
            [SerializeField] private Transform[] fieldPoints;

            public Vector2 FieldPoint => fieldPoints[Random.Range(0, fieldPoints.Length)].position;
        }
    }

    internal interface IUnitMover
    {
        UnityAction<Unit> OnGotTarget { get; set; }

        float Speed { get; }

        Vector2 Position { get; set; }
        
        Vector2 Target { get; set; }

        UnitGameParameters Parameters { get; set; }
        
        void MoveCurrent();
    }
}