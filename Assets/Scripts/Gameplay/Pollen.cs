using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

class Pollen : MonoBehaviour
{
    public UnityEvent OnDestroy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.TAG_LAND))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
        else if (other.CompareTag(GameConstants.TAG_BOUNDS))
        {
            Destroy(gameObject);
            if (OnDestroy != null)
            {
                OnDestroy.Invoke();
            }
        }
    }
}