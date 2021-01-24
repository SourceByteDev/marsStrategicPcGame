using System.Collections;
using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Enemies.Item
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private float delayAttack = .2f;

        [Space(5)] [SerializeField] private float minOffset = .4f;

        [SerializeField] private float maxOffset = 1f;

        [Space(5)] [SerializeField] private int damage = 10;

        private Coroutine _currentAttacking;

        private float RandomOffset => Random.Range(minOffset, maxOffset);

        private Unit CurrentTarget { get; set; }

        public UnityAction OnTargetDie { get; set; }

        public bool IsAttacking => _currentAttacking != null;

        public void StartAttack(Unit unit)
        {
            if (IsAttacking)
            {
                StopAttack();
            }

            CurrentTarget = unit;

            CurrentTarget.OnDead += OnTargetDie;
            
            _currentAttacking = StartCoroutine(Attacking());
        }

        public void StopAttack()
        {
            if (!IsAttacking)
                return;
            
            if (CurrentTarget != null)
                CurrentTarget.OnDead -= OnTargetDie;
            
            CurrentTarget = null;
            
            StopCoroutine(_currentAttacking);

            _currentAttacking = null;
        }

        private IEnumerator Attacking()
        {
            yield return new WaitForSeconds(delayAttack);

            while (true)
            {
                yield return new WaitForSeconds(RandomOffset);

                if (CurrentTarget == null)
                    break;
                
                CurrentTarget.Damage(damage);
            }
        }
    }
}