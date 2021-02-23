using System;
using Manager;
using UnityEngine;

namespace Game.Enemies.Item
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMove : MonoBehaviour
    {
        private Enemy _myEnemy;
        
        public bool IsMoving { get; private set; }
        
        public Vector2 CurrentTarget { get; private set; }
        
        public float CurrentSpeed { get; private set; }

        public void GoMove()
        {
            
        }
        
        public void SetIsMove(bool isMove)
        {
            IsMoving = isMove;
        }
        
        public void SetSpeed(float speed)
        {
            CurrentSpeed = speed;
        }
        
        public void SetTarget(Vector2 target)
        {
            CurrentTarget = target;
        }

        private void CheckMove()
        {
            if (!IsMoving)
                return;

            var position = transform.position;

            position = Vector2.MoveTowards(position, CurrentTarget, CurrentSpeed * Time.deltaTime);

            transform.position = position;

            _myEnemy.MyLiveUnitData.position = position;
        }

        private void Start()
        {
            _myEnemy = GetComponent<Enemy>();
        }

        private void FixedUpdate()
        {
            CheckMove();
        }
    }
}