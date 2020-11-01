using System;
using Data;
using Game.Units.Control;
using Manager;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Units.Unit_Types
{
    public class InfantryUnit : Unit, IUnitMover
    {
        private SkeletonAnimation _skeletonAnimation;
        
        public UnityAction<Unit> OnGotTarget { get; set; }
        public float Speed { get; set; }
        
        public Vector2 Position
        {
            get => transform.position;
            set
            {
                transform.position = value;

                MyLiveUnit.position = value;
            }
        }
        public Vector2 Target { get; set; }
        
        public InfantryMoveState MoveState
        {
            get => gameParameters.moveParameters.InfantryMove.MoveState;
            set
            {
                gameParameters.moveParameters.InfantryMove.MoveState = value; 
                
                OnMoveStateChanged.Invoke(value);
            }
        }

        private UnityAction<InfantryMoveState> OnMoveStateChanged { get; set; }
        
        private ValuesManage.LiveUnitData MyLiveUnit { get; set; }

        public void MoveCurrent()
        {
            if (MoveState != InfantryMoveState.MovingToPoint)
                return;
            
            var myPosition = Position;

            myPosition = Vector2.MoveTowards(myPosition, Target, Speed * Time.deltaTime);

            Position = myPosition;
            
            var isGotToPosition = Vector2.Distance(myPosition, Target) <= .05f;

            if (!isGotToPosition) 
                return;

            OnGotTarget?.Invoke(this);
        }

        private void OnStateChanged(InfantryMoveState state)
        {
            var animationName = "";

            switch (state)
            {
                case InfantryMoveState.WaitingTarget:
                    animationName = "attack";
                    break;
                case InfantryMoveState.MovingToPoint:
                    animationName = "move";
                    break;
            }

            print("change some " + animationName);
            
            _skeletonAnimation.AnimationName = animationName;
        }

        private void Start()
        {
            OnMoveStateChanged += OnStateChanged;

            _skeletonAnimation = GetComponent<SkeletonAnimation>();

            MyLiveUnit = Managers.Values.GetLiveUnitByUnit(this);

            MoveState = MoveState;
        }

        private void FixedUpdate()
        {
            MoveCurrent();
        }
    }
}