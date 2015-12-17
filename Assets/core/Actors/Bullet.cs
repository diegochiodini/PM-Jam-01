using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour 
{
	public event Action<Bullet, Collision2D> hitEvent;

    private static int bulletCount = 0;

	public Vector3 velocity;

    public float radius;
    public string gameplayLayer;
    public string animationLayer;

    public bool dynamic
    {
        get
        {
            return !GetComponent<Rigidbody2D>().isKinematic;
        }

        set
        {
            GetComponent<Rigidbody2D>().isKinematic = !value;
        }
    }

    private bool _animationLayer = false;
    public bool isCollisionLayer
    {
        get
        {
            return gameObject.layer == LayerMask.NameToLayer(gameplayLayer);
        }

        set
        {
            gameObject.layer = value ? LayerMask.NameToLayer(gameplayLayer) : LayerMask.NameToLayer(animationLayer);
        }
    }

	void Awake()
	{
        //Assert.NotNull(GetComponent<Collider>(), "A bullet must have a collider defined");
        this.dynamic = false;
        name += (++bulletCount).ToString();
	}

	public void Fire() 
	{
        this.dynamic = true;
		GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
	}

    protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
        if (!isCollisionLayer)
        {
            return; //HACK: sometime the collision is triggered even if i already set the animation layer. I guess this call was in a queue before entering the collision callback.
        }

        dynamic = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (hitEvent != null)
        {
            hitEvent.Invoke(this, collision);
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
