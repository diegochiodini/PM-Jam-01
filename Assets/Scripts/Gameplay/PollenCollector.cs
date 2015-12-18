using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PollenCollector : MonoBehaviour
{
    private Collider2D pollenCollider;
    public UnityEvent onPollenDrop;

    private void OnCollisionEnter2D(Collision2D other)
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