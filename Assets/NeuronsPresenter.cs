using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuronsPresenter : MonoBehaviour
{
    [SerializeField] private Image _neuronView;
    [SerializeField] private LineRenderer _transitionView;
    [SerializeField] private Image _background;
    [SerializeField] private float _maxTransitionWdth;
    
    [SerializeField] private Transform _viewParent;
    [SerializeField] private Vector2 _startSize;

    private List<GameObject> _views;

    private void Start()
    {
        _views = new List<GameObject>();
    }

    public void Show(NeuralNetwork neuralNetwork)
    {
        _viewParent.gameObject.SetActive(true);

        Neuron[][] neurons = neuralNetwork.Neurons;
        NeuronView[][] neuronViews = new NeuronView[neurons.Length][];

        _background.rectTransform.sizeDelta = _startSize * 1.2f;

        for (int x = 0; x < neurons.Length; x++)
        {
            neuronViews[x] = new NeuronView[neurons[x].Length];

            for (int y = 0; y < neurons[x].Length; y++)
            {
                Image neuron = Instantiate(_neuronView);

                neuron.transform.SetParent(_viewParent, false);

                neuron.rectTransform.localPosition = new Vector2(-_startSize.x / 2 + x * (_startSize.x / neurons.Length),_startSize.y / 2 - y * (_startSize.y / neurons[x].Length));

                Debug.Log( -_startSize.x / 2 + x * (_startSize.x / neurons.Length));

                NeuronView neuronView = new NeuronView();

                neuronView.Neuron = neurons[x][y];

                neuronView.Transform = neuron.transform;

                neuronViews[x][y] = neuronView;

                _views.Add(neuron.gameObject);
            }
        }

        for (int x = 1; x < neurons.Length; x++)
        {
            for (int y = 0; y < neurons[x].Length; y++)
            {
                Neuron neuron = neurons[x][y];

                for (int i = 0; i < neurons[x - 1].Length; i++)
                {
                    LineRenderer transitionView = Instantiate(_transitionView);
                    transitionView.transform.SetParent(_viewParent, false);
                    Vector3[] positions = new Vector3[2];
                    positions[0] = neuronViews[x - 1][i].Transform.localPosition;
                    positions[1] = neuronViews[x][y].Transform.localPosition;

                    transitionView.SetPositions(positions);

                    float width = Mathf.Abs(neuron.Weights[i]) * _maxTransitionWdth;

                    transitionView.SetWidth(width, width);

                    _views.Add(transitionView.gameObject);

                }

            }
        }

    }

    private void Clear()
    {
        _viewParent.gameObject.SetActive(false);

        foreach (var item in _views)
        {
            Destroy(item);
        }

        _views.Clear();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _viewParent.localScale += (Vector3)Vector2.one * 0.01f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _viewParent.localScale -= (Vector3)Vector2.one * 0.01f;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Clear();
        }
    }
}

public struct NeuronView
{
    public Transform Transform;
    public Neuron Neuron;
}
