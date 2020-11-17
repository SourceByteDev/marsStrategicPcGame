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
    public class InfantryUnit : Unit, IUnitMover
    {
        [SerializeField] private string moveName = "move";

        [SerializeField] private bool needDoStart;

        [SerializeField] private string startName = "start";

        [SerializeField] private float offsetStart = 1.2f;

        private bool isStarting;
        
        private SkeletonAnimation skeletonAnimation;
        
        public UnityAction<Unit> OnGotTarget { get; set; }
        public float Speed => Parameters.moveParameters.MoveSpeed;
        
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
        
        public UnitGameParameters Parameters { get; set; }

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
            if (MoveState != InfantryMoveState.MovingToPoint || isStarting)
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
                    animationName = moveName;
                    break;
            }

            print("change some " + animationName);
            
            skeletonAnimation.AnimationName = animationName;
        }

        private void Start()
        {
            OnMoveStateChanged += OnStateChanged;

            skeletonAnimation = GetComponent<SkeletonAnimation>();

            MyLiveUnit = Managers.Values.GetLiveUnitByUnit(this);

            MoveState = MoveState;

            if (needDoStart)
                StartCoroutine(Starting());
        }

        private void FixedUpdate()
        {
            MoveCurrent();
        }

        private IEnumerator Starting()
        {
            skeletonAnimation.loop = false;

            var lastName = skeletonAnimation.AnimationName;

            skeletonAnimation.AnimationName = startName;

            isStarting = true;
            
            yield return new WaitForSeconds(offsetStart);

            skeletonAnimation.loop = true;

            skeletonAnimation.name = lastName;
            
            isStarting = false;
        }
    }
}