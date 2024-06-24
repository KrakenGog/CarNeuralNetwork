using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _maxRotate;

    public Rigidbody2D Rigidbody => _rigidBody;

    private Rigidbody2D _rigidBody;

    public void Initialize()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    public void Move(float input)
    {
        if(_rigidBody.velocity.magnitude < _speed)
           _rigidBody.AddForce(transform.up * Mathf.Clamp(input, -_maxForce, _maxForce), ForceMode2D.Impulse);
    }

    public void Rotate(float input)
    {
        transform.Rotate(-Vector3.forward*Mathf.Clamp(input/ 100, -_maxRotate, _maxRotate));
    }
}
