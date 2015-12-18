using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

class Pollen : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.TAG_LAND))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
        else if (other.CompareTag(GameConstants.TAG_BOUNDS))
        {
            MainGameplay gameplay = GetComponentInParent<MainGameplay>();
            gameplay.OnScore();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(GameConstants.TAG_BOUNDS))
        {
            MainGameplay gameplay = GameObject.FindObjectOfType<MainGameplay>();
            gameplay.OnScore();
            Destroy(gameObject);
        }
    }
}