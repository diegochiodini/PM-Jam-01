using UnityEngine;
using UnityEngine.Assertions;

public class SimpleController : MonoBehaviour
{
    const string _horizontalAxis = "Horizontal";
    const string _verticalAxis = "Vertical";

    public float speed = 1f;

    Rigidbody2D _body;
    Vector2 _direction = new Vector2(0f, 0f);

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_body);
    }

    private void Update()
    {
        _direction.Set(Input.GetAxis(_horizontalAxis), Input.GetAxis(_verticalAxis));
        _body.velocity = _direction * speed;            
    }
}