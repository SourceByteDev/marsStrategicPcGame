using System;
using System.Linq;
using Data;
using Game.Units.Control;
using Game.Units.Unit_Types;
using Manager;
using UnityEngine;

namespace Game.Enemies.Item
{
    [RequireComponent(typeof(EnemyVisual))]
    [RequireComponent(typeof(EnemyMove))]
    [RequireComponent(typeof(EnemyAttacker))]
    public class Enemy : Unit
    {
        [SerializeField] private State currentState;

        [SerializeField] private float speed = 3;

        [SerializeField] private float distanceToStartAttack = 2;

        private Vector2 _basePoint;

        public State CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;

                OnStateChanged(value);
            }
        }

        private EnemyVisual _visual;

        private EnemyMove _move;

        private EnemyAttacker _attacker;

        public void StartAttackUnit(Unit unit, bool isHome)
        {
            _attacker.StartAttack(unit);

            CurrentState = isHome ? State.AttackOnBase : State.FightingWithOther;
        }

        public void MoveToPlayerBase()
        {
            _move.SetTarget(_basePoint);

            CurrentState = State.MovingToBase;
        }

        private void CheckToEnterBase()
        {
            if (CurrentState != State.MovingToBase)
                return;

            if (Vector2.Distance(_basePoint, transform.position) > distanceToStartAttack)
                return;

            var houseLiveUnit = Managers.Values.LiveUnits.ToArray()[0];

            StartAttackUnit(Managers.Values.GetUnitByLiveUnit(houseLiveUnit), true);
        }

        private void CheckToAttackAnyTarget()
        {
            if (CurrentState != State.MovingToBase)
                return;

            var liveUnits = Managers.Values.LiveUnits.ToList();

            if (!liveUnits.Where(x => Vector2.Distance(transform.position, x.position) > 1).Any(x => Vector2.Distance(transform.position, x.position) <= distanceToStartAttack))
                return;

            var foundLiveUnit =
                liveUnits.First(x => Vector2.Distance(transform.position, x.position) <= distanceToStartAttack);

            var unit = Managers.Values.GetUnitByLiveUnit(foundLiveUnit);
            
            if (unit.gameParameters == null)
                return;
            
            if (unit.gameParameters.IsEnemy)
                return;
            
            StartAttackUnit(unit, false);
        }

        private void OnStateChanged(State state)
        {
            switch (state)
            {
                case State.FightingWithOther:
                case State.AttackOnBase:
                    _move.SetIsMove(false);
                    break;
            
                case State.MovingToBase:
                    _move.SetIsMove(true);
                    break;
            }
        }

        private void Awake()
        {
            _visual = GetComponent<EnemyVisual>();

            _move = GetComponent<EnemyMove>();

            _attacker = GetComponent<EnemyAttacker>();
        }

        protected override void Start()
        {
            base.Start();
            
            _basePoint = Managers.Values.LiveUnits.ToArray()[0].position;

            _move.SetSpeed(speed);
            
            _attacker.OnTargetDie += delegate
            {
                CurrentState = State.MovingToBase;
            };

            MoveToPlayerBase();
        }

        private void FixedUpdate()
        {
            CheckToEnterBase();

            CheckToAttackAnyTarget();
        }

        public enum State
        {
            MovingToBase,
            FightingWithOther,
            AttackOnBase
        }
    }
}