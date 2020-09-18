using System.Collections;
using Manager;
using UnityEngine;

namespace GameUi
{
    public class Timer : MonoBehaviour
    {
        private Coroutine _timer;
        
        public void StartTimer()
        {
            if (_timer != null)
                StopCoroutine(_timer);
            
            _timer = StartCoroutine(Timing());
        }

        private static IEnumerator Timing()
        {
            while (true)
            {
                Managers.Values.values.CurrentTimeSeconds++;
                
                yield return new WaitForSeconds(1);
            }
        }
    }
}