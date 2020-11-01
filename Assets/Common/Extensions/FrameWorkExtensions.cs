using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Common.Extensions
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                
                return _instance;
            }
            protected set => _instance = value;
        }

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }

    public static class FrameWorkExtensions
    {
        #region Physics

        public static void AddForce(this IEnumerable<Rigidbody> bodies, float force, Vector3 direction)
        {
            bodies.ToList().ForEach(x => x.AddForce(x.transform.TransformDirection(direction) * force));
        }

        #endregion
    }

    public static class DataLoader
    {
        public static T LoadObject<T>(string path) where T : class
        {
            var item = Resources.Load(path, typeof(T)) as T ?? throw new Exception("Cant get data " + nameof(T));

            return item;
        }
    }
}