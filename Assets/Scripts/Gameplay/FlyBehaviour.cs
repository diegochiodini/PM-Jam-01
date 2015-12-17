using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class FlyBehaviour : MonoBehaviour
{
    Rigidbody2D _body;

    Vector3 _direction = Vector3.zero;
    float _actualDistance = 0f;
    public float Distance = 1f;
    public float RefreshRate = 1f;

    public Transform target;
    public float speed = 1f;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
        joint.distance = Distance;
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        _body.velocity =  _direction * speed * _actualDistance;
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(RefreshRate);

        _direction = target.position - transform.position;
        _direction = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
        _actualDistance = _direction.magnitude * Random.value;
        _direction.Normalize();

        StartCoroutine(ChangeDirection());
    }
}