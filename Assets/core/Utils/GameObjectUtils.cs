using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    class GameObjectUtils
    {
        public static void DestroyAllChildren(GameObject parent, bool immediate)
        {
            foreach ( Transform child in parent.transform)
            {
                if (immediate)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
                else
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        public static void DestroyAllObjectsWithComponent<T>(bool immediate) where T:UnityEngine.Component
        {
            T[] results = GameObject.FindObjectsOfType<T>();
            foreach (T component in results)
            {
                if (immediate)
                {
                    GameObject.DestroyImmediate(component.gameObject);
                }
                else
                {
                    GameObject.Destroy(component.gameObject);
                }
            }
        }
    }
}
