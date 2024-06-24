using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private CarController _prefab;
    [SerializeField] private int _count;
    [SerializeField] private float _timeForOneEvolution;
    [SerializeField] private Transform _start;
    [Header("")]
    [SerializeField] private int _layersCount;
    [SerializeField] private int _layersWidth;
    [SerializeField] private int _evolutionsCount;
    [SerializeField] private float _aducationSpeed;
    [SerializeField] private List<Transform> _points;

    private List<CarController> _carControllers;
    private List<NeuralNetwork> _bestNetworks;

    private float _currentTime;

    private void Start()
    {
        _carControllers = new List<CarController>();

        for (int i = 0; i < _count; i++)
        {
            NeuralNetwork neuralNetwork = new NeuralNetwork();
            neuralNetwork.CreateWithRandomWeights(17, _layersCount, _layersWidth, 2);

            CarController carController = Instantiate(_prefab);
            carController.transform.position = _start.position;

            carController.Initialize(neuralNetwork, _points);

            _carControllers.Add(carController);
        }

        _currentTime = _timeForOneEvolution;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if(_currentTime <= 0)
        {
            StartNewLoop();
            _currentTime = _timeForOneEvolution;
        }
    }

    public void StartNewLoop()
    {
        _bestNetworks = new List<NeuralNetwork>();
        float maxScore = 0;
        CarController bestCar = null;

        for (int i = 0; i < _evolutionsCount; i++)
        {
            foreach (var car in _carControllers)
            {
                if (car.Score > maxScore)
                {
                    maxScore = car.Score;
                    bestCar = car;
                }
            }

            _carControllers.Remove(bestCar);
            _bestNetworks.Add(bestCar.NeuralNetwork);




            Destroy(bestCar.gameObject);

            maxScore = 0;
        }

        foreach (var car in _carControllers)
        {
            Destroy(car.gameObject);
        }

        _carControllers.Clear();
        Debug.Log("Winner: "+_bestNetworks[0].Neurons.Length);

        for (int i = 0; i < _evolutionsCount; i++)
        {
            for (int k = 0; k < _count/_evolutionsCount; k++)
            {
                NeuralNetwork neuralNetwork = new NeuralNetwork();
                neuralNetwork.CreateBaseOn(_bestNetworks[i], _aducationSpeed * k);

                

                CarController carController = Instantiate(_prefab);
                carController.transform.position = _start.position;

                

                carController.Initialize(neuralNetwork, _points);

                _carControllers.Add(carController);
                Debug.Log(_bestNetworks[0].Neurons == neuralNetwork.Neurons);
            }
        }

    }

    public void ChangeGameSpeed()
    {
        if (Time.timeScale < 4)
        {
            Time.timeScale += .5f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
