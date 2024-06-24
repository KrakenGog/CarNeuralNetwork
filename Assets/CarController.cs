using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private LayerMask _castMask;
    [SerializeField] private float _castDistance;
    [SerializeField] private float _achivePointDistance;

    public float Score => _currentScore;
    public NeuralNetwork NeuralNetwork => _neuralNetwork;

    private NeuralNetwork _neuralNetwork;

    private Car _car;


    private float[] _outputs;
    private float _moveInput => _outputs[0];
    private float _rotateInput => _outputs[1];

    [SerializeField] private float _currentScore;

    private List<Transform> _points;

    private int _currentPointIndex;

    private float _timeSinceLastPoint;
    private Transform _currentPoint => _points[_currentPointIndex];

    

    public void Initialize(NeuralNetwork neuralNetwork, List<Transform> points)
    {
        _neuralNetwork = neuralNetwork;
        _car = GetComponent<Car>();

        _car.Initialize();

        _points = points;

        _outputs = new float[2];

        _outputs[0] = 0;
        _outputs[1] = 0;

        _timeSinceLastPoint = 1;
        
    }

    private void Update()
    {
        _timeSinceLastPoint += Time.deltaTime;

       
        if(Vector2.Distance(transform.position, _currentPoint.position) <= _achivePointDistance)
        {
            _currentScore += 1/ _timeSinceLastPoint;

            _timeSinceLastPoint = 0;

            if (_currentPointIndex + 1 < _points.Count)
                _currentPointIndex++;
            else
                _currentPointIndex = 0;

        }
    }


    private void FixedUpdate()
    {
        float[] inputs = new float[17];

        inputs[0] = GetDistance(transform.up);
        inputs[1] = GetDistance(transform.up + transform.right);
        inputs[2] = GetDistance(transform.up - transform.right);
        inputs[3] = GetDistance(transform.right);
        inputs[4] = GetDistance(transform.up*2 + transform.right);
        inputs[5] = GetDistance(transform.up * 2 - transform.right);

        inputs[6] = GetDistance(-transform.right);
        inputs[7] = GetDistance(-transform.up + transform.right);
        inputs[8] = GetDistance(-transform.up);
        inputs[9] = GetDistance(-transform.up - transform.right);

        inputs[10] = _car.Rigidbody.velocity.x;
        inputs[11] = _car.Rigidbody.velocity.y;
        inputs[12] = _car.Rigidbody.angularVelocity;


        Vector2 direction = _currentPoint.position - transform.position;

        inputs[13] = direction.x;
        inputs[14] = direction.y;

        inputs[15] = transform.up.x;
        inputs[16] = transform.up.y;

        float[] outputs = _neuralNetwork.Run(inputs);

        _outputs = outputs;

        _car.Move(_moveInput);
        _car.Rotate(_rotateInput);
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentScore -= 2;
    }

    private float GetDistance(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _castDistance, _castMask);

        float distance = _castDistance;

        if (hit)
        {
            distance = Vector2.Distance(transform.position, hit.point);
        }

        return distance;
    }

}
