using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using LogicHelper;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace GameUi
{
    public class Timer : MonoBehaviour
    {
        public static UnityAction OnSecond { get; set; }
        
        private Coroutine timer;
        
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
                
                // OnSecond?.Invoke();
                
                yield return new WaitForSeconds(1);
            }
        }
    }
}