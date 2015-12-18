using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Player : MonoBehaviour 
{
    private Rigidbody2D rbody;
    private CircleCollider2D collider;

    private Vector2 defaultPosition;

    private Collider2D pollenCollider;

    public UnityEvent onPollenDrop;

    void Awake()
    {
        defaultPosition = transform.position;
        rbody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rbody, "Player must have a Rigidbody2D");
        collider = GetComponent<CircleCollider2D>();
        Assert.IsNotNull(collider, "Player must have a " + collider.GetType().ToString());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(GameConstants.TAG_POLLEN))
        {
            other.transform.GetComponent<Rigidbody2D>().isKinematic = true;
            other.transform.SetParent(transform);
            other.transform.localPosition = Vector2.up * -0.8f;
            other.collider.enabled = false;
            pollenCollider = other.collider;
        }
        else if (pollenCollider != null && other.collider.CompareTag(GameConstants.TAG_PLAYER))
        {
            //pollenCollider.enabled = true;
            pollenCollider.transform.SetParent(null);
            pollenCollider.GetComponent<Rigidbody2D>().isKinematic = false;
            pollenCollider = null;

            if (onPollenDrop != null)
            {
                onPollenDrop.Invoke();
            }
        }
    }
}
