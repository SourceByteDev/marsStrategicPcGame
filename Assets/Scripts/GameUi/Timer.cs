using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Data;
using LogicHelper;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace GameUi
{
    public class Timer : Singleton<Timer>
    {
        private Coroutine timer;
        
        public UnityAction OnSecond { get; set; }

        public void StartTimer()
        {
            if (timer != null)
                StopCoroutine(timer);
            
            timer = StartCoroutine(Timing());
        }

        
        
        
        private IEnumerator Timing()
        {
            while (true)
            {
                Managers.Values.values.CurrentTimeSeconds++;
                
                UnitProcessBuild.AddAllSeconds(); 
                
                OnSecond?.Invoke();
                
                yield return new WaitForSeconds(1);
            }
        }
    }
}