using UnityEngine;

namespace Addone
{
    public class SaveVariables
    {
        public static Variable firstLaunch = new Variable("FirstLaunch");
    }

    public struct Variable
    {
        private readonly string name;

        public int GetInt => PlayerPrefs.GetInt(name);

        public float GetFloat => PlayerPrefs.GetFloat(name);

        public string GetString => PlayerPrefs.GetString(name);
        
        public Variable(string name)
        {
            this.name = name;
        }

        public void SetInt(int value)
        {
            PlayerPrefs.SetInt(name, value);
        }

        public void SetFloat(float value)
        {
            PlayerPrefs.SetFloat(name, value);
        }

        public void SetString(string value)
        {
            PlayerPrefs.SetString(name, value);
        }
    }
}