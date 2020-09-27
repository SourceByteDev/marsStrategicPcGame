using System;
using UnityEngine;

namespace Addone
{
    public class ClearChildrenOnStart : MonoBehaviour
    {
        private void Awake()
        {
            if (transform.childCount <= 0)
                return;

            for (var i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
