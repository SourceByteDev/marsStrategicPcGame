using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Control")]
        
        public Rigidbody forceRigidBody;
        
        [Space(5)]
        
        [Header("Move Parameters")]
        
        public string moveHorizontalAxis = "Horizontal";

        public float moveSpeed = 3;

        public float modiferAddValue = .3f;

        [Space(5)]
        
        [Header("Range")]

        public RangeData rangePositionX;

        public RangeData rangeModiferSpeed;

        public float distanceToStopEdge = 1;

        private float currentModiferToSpeed = 0;

        private float ModiferToSpeed
        {
            get => currentModiferToSpeed;

            set
            {
                value = rangeModiferSpeed.GetRangedValue(value);
                
                currentModiferToSpeed = value;
            }
        }

        private bool IsRangedNow => IsRangedMin || IsRangedMax;
        
        private bool IsRangedMin { get; set; }
        
        private bool IsRangedMax { get; set; }
        
        private void MoveControlCamera()
        {
            var valueMove = Input.GetAxis(moveHorizontalAxis);

            if (valueMove > 0 && IsRangedMin)
                IsRangedMin = false;
            else if (valueMove < 0 && IsRangedMax)
                IsRangedMax = false;
            
            if (Math.Abs(valueMove) < .01f || IsRangedNow)
                return;

            ModiferToSpeed += modiferAddValue * Time.deltaTime;
            
            var forceVector = new Vector2(valueMove * moveSpeed * ModiferToSpeed, 0);

            forceRigidBody.AddForce((Vector3) forceVector * Time.deltaTime);
        }

        private void RangePosition()
        {
            var positionX = forceRigidBody.position.x;

            var isMinRange = rangePositionX.RangeWithMin(positionX) < distanceToStopEdge;

            var isMaxRange = rangePositionX.RangeWithMax(positionX) < distanceToStopEdge;
            
            var isSomeNearEdge = isMinRange ||
                                 isMaxRange;
            
            if (!isSomeNearEdge)
                return;

            var velocityX = forceRigidBody.velocity.x;

            var goingLeft = velocityX < 0;

            IsRangedMin = goingLeft && isMinRange;

            IsRangedMax = !goingLeft && isMaxRange;
        }
        
        private void FixedUpdate()
        {
            RangePosition();
            
            MoveControlCamera();
        }

        [Serializable]
        public struct RangeData
        {
            public float minValue;
            
            public float maxValue;

            public float RangeWithMin(float value)
            {
                return Math.Abs(minValue - value);
            }

            public float RangeWithMax(float value)
            {
                return Mathf.Abs(maxValue - value);
            }
            
            public float GetRangedValue(float value)
            {
                return Mathf.Clamp(value, minValue, maxValue);
            }
        }
    }
}
