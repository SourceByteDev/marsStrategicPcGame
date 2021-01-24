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
    public class Enemy : MonoBehaviour
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

        public void StartAttackUnit(Unit unit)
        {
            _attacker.StartAttack(unit);

            CurrentState = State.AttackOnBase;
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
            
            StartAttackUnit(Managers.Values.GetUnitByLiveUnit(houseLiveUnit));
        }
        
        private void OnStateChanged(State state)
        {
            switch (state)
            {
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

        private void Start()
        {
            _basePoint = Managers.Values.LiveUnits.ToArray()[0].position;
            
            _move.SetSpeed(speed);
            
            MoveToPlayerBase();
        }

        private void FixedUpdate()
        {
            CheckToEnterBase();
        }

        public enum State
        {
            MovingToBase,
            FightingWithOther,
            AttackOnBase
        }
    }
}