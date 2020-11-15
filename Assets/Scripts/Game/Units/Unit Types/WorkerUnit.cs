using System;
using System.Collections;
using Data;
using Game.Units.Control;
using Manager;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Units.Unit_Types
{
    public class WorkerUnit : Unit, IUnitMover
    {
        private Coroutine _collectingGames;
        
        private SkeletonAnimation _skeletonAnimation;
        
        public UnityAction<Unit> OnGotTarget { get; set; }

        public float Speed => Parameters.moveParameters.MoveSpeed;

        public Vector2 Target { get; set; }
        public UnitGameParameters Parameters { get; set; }

        public WorkerMoveState MoveState
        {
            get => WorkerMoveParameters.WorkerMoveState;
            set
            {
                WorkerMoveParameters.WorkerMoveState = value;

                OnStateChanged.Invoke(value);
            }
        }

        public Vector2 Position
        {
            get => transform.position;

            set
            {
                transform.position = value;

                MyLiveUnit.position = value;
            }
        }

        private ValuesManage.LiveUnitData MyLiveUnit { get; set; }
        
        private UnityAction<WorkerMoveState> OnStateChanged { get; set; }

        private WorkerMoveParameters WorkerMoveParameters => gameParameters.moveParameters.WorkerMove;

        public void MoveCurrent()
        {
            if (MoveState == WorkerMoveState.CollectingGems)
                return;

            var myPosition = Position;

            myPosition = Vector2.MoveTowards(myPosition, Target, Speed * Time.deltaTime);

            Position = myPosition;
            
            var isGotToPosition = Vector2.Distance(myPosition, Target) <= .05f;

            if (!isGotToPosition) 
                return;

            OnGotTarget?.Invoke(this);
        }

        private void OnMoveStateChange(WorkerMoveState state)
        {
            switch (state)
            {
                case WorkerMoveState.CollectingGems:
                    if (_collectingGames != null)
                        StopCoroutine(_collectingGames);

                    _collectingGames = StartCoroutine(StoppingOnCollecting());

                    _skeletonAnimation.AnimationName = "work"; 
                    break;
                case WorkerMoveState.GoToGems:
                    _skeletonAnimation.AnimationName = "move";

                    WorkerMoveParameters.SecondsToCollect = WorkerMoveParameters.MaxSecondsToCollect;
                    break;
                case WorkerMoveState.GoToHome:
                    _skeletonAnimation.AnimationName = "move_back";    
                    break;
            }
        }

        private void Start()
        {
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
            
            OnStateChanged += OnMoveStateChange;

            MoveState = MoveState;

            MyLiveUnit = Managers.Values.GetLiveUnitByUnit(this);
        }

        private void FixedUpdate()
        {
            MoveCurrent();
        }

        private IEnumerator StoppingOnCollecting()
        {
            while (WorkerMoveParameters.SecondsToCollect > 0)
            {
                yield return new WaitForSeconds(1);

                WorkerMoveParameters.SecondsToCollect--;
            }
            
            OnGotTarget?.Invoke(this);
        }
    }
}