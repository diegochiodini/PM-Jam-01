using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private const float DISTANCE_THRESHOLD = 0.1f;

    public Transform rootParent;
    public float jumpForce;

    private Transform lastParent;
    private Rigidbody2D rbody;
    private CircleCollider2D collider;

    private Vector2 defaultPosition;

    void Awake()
    {
        defaultPosition = transform.position;
        rbody = GetComponent<Rigidbody2D>();
        Assert.NotNullSafe(rbody, "Player must have a Rigidbody2D");
        collider = GetComponent<CircleCollider2D>();
        Assert.NotNullSafe(collider, "Player must have a " + collider.GetType().ToString());
    }

    private void Jump()
    {
        rbody.isKinematic = false;
        rbody.velocity = Vector2.zero;
        Vector2 direction = transform.position - transform.parent.position;
        rbody.AddForce(direction.normalized * jumpForce, ForceMode2D.Impulse);
        transform.parent = rootParent;
    }

    public void Reset()
    {
        transform.parent = rootParent;
        transform.position = defaultPosition;
        lastParent = rootParent;
        rbody.velocity = Vector2.zero;
        rbody.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (lastParent == rootParent && coll.gameObject.tag == GameConstants.TAG_ROLLER)
         {
             transform.parent = coll.transform;
             lastParent = transform.parent;
             rbody.isKinematic = true;
             rbody.velocity = Vector2.zero;
         }
    }

    void Update()
    {
        if (lastParent != rootParent && lastParent.tag == GameConstants.TAG_ROLLER)
        {
            CircleCollider2D circle = GetComponent<CircleCollider2D>();

            if ((lastParent.position - transform.position).sqrMagnitude > (circle.radius + collider.radius + DISTANCE_THRESHOLD))
            {
                lastParent = rootParent;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }
    }
}
