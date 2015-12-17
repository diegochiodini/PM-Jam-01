using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetInterface<T>(this GameObject inObj) where T : class
        {
#if DEBUG
            if (!typeof(T).IsInterface)
            {
                Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
                return null;
            }
#endif
            return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
        {
#if DEBUG
            if (!typeof(T).IsInterface)
            {
                Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
                return Enumerable.Empty<T>();
            }
#endif
            return inObj.GetComponents<Component>().OfType<T>();
        }
    }

    public static class MonoBehaviourExtensions
    {
        public static Coroutine WaitForSecondFireAction(this MonoBehaviour inBehaviour, float seconds, Action action)
        {
            return inBehaviour.StartCoroutine(WaitForSecondFireActionCoroutine(seconds, action));
        }

        private static IEnumerator WaitForSecondFireActionCoroutine(float seconds, Action action)
        {
            DateTime now = DateTime.UtcNow;
            DateTime endTime = now.AddSeconds(seconds);

            while (endTime > now) 
            {
                now = DateTime.UtcNow;
                yield return null; 
            }

            action();
        }
    }
}