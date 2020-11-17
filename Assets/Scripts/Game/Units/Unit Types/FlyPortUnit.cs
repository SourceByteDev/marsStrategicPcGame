using System;
using System.Collections;
using Data;
using Spine.Unity;
using UnityEngine;

namespace Game.Units.Unit_Types
{
    public class FlyPortUnit : Unit
    {
        [SerializeField] private string startName = "flyport_dor";

        [SerializeField] private float offset = 1.5f;

        private SkeletonAnimation skeleton;
        
        public override void OnSpawnedSome(BuildUnitParameters unit)
        {
            base.OnSpawnedSome(unit);
            
            print("Fly have");

            StartCoroutine(StartAnim());
        }

        private void Start()
        {
            skeleton = GetComponent<SkeletonAnimation>();
        }

        private IEnumerator StartAnim()
        {
            if (skeleton == null)
                yield break;
            
            var lastName = skeleton.AnimationName;
            
            skeleton.loop = false;

            skeleton.AnimationName = startName;
            
            yield return new WaitForSeconds(offset);

            skeleton.loop = true;

            skeleton.AnimationName = lastName;
        }
    }
}