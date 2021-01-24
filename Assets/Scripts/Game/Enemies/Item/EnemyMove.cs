using System;
using UnityEngine;

namespace Game.Enemies.Item
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMove : MonoBehaviour
    {
        public bool IsMoving { get; private set; }
        
        public Vector2 CurrentTarget { get; private set; }
        
        public float CurrentSpeed { get; private set; }

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
        }
        
        private void FixedUpdate()
        {
            CheckMove();
        }
    }
}