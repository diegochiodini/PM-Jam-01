﻿using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class SimpleController : MonoBehaviour
{
    const string _horizontalAxis = "Horizontal";
    const string _verticalAxis = "Vertical";

    public float speed = 1f;

    public UnityEvent OnChangeDirection;

    Rigidbody2D _body;
    Vector2 _direction = new Vector2(0f, 0f);

    float _previousDirection = 1f;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_body);
        _previousDirection = transform.localScale.x;
    }

    private void Update()
    {
        _direction.Set(Input.GetAxis(_horizontalAxis), Input.GetAxis(_verticalAxis));
        _body.velocity = _direction * speed;          

        if (_direction.x != 0f && Mathf.Sign(_previousDirection) != Mathf.Sign(_direction.x))
        {
            _previousDirection *= -1f;
            OnChangeDirection.Invoke();
        }
    }
}