using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Player : MonoBehaviour 
{
    private Rigidbody2D rbody;
    private CircleCollider2D collider;

    private Vector2 defaultPosition;

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
            other.rigidbody.isKinematic = true;
        }
    }
}
