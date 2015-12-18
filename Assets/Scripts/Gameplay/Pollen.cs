using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Pollen : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER COLLISION!");
        if (other.CompareTag(GameConstants.TAG_LAND))
        {
            Debug.Log("LAAAAAAAAAAAAAAAAAND!");
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}