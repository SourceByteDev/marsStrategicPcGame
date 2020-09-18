using UnityEngine;
using Addone;

namespace Manager
{
    public class GameControl : MonoBehaviour
    {
        private static Variable FirstLaunch => SaveVariables.firstLaunch;
        
        [ContextMenu("Reset all")]
        public void ResetAllData()
        {
            PlayerPrefs.DeleteAll();
            
            Managers.Values.ResetAllValues();
        }
        
        public static void OnGameStarted()
        {
            if (FirstLaunch.GetInt == 0)
            {
                print("First launch");
                
                Managers.Values.ResetAllValues();
            }
            
            FirstLaunch.SetInt(1);
        }

        
    }
}