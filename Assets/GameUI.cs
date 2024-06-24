using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private ScreenPicker _screenPicker;
    [SerializeField] private NeuronsPresenter _neuronsPresenter;

    private void Start()
    {
        _screenPicker.OnPicked += OnPicked;
    }


    public void OnPicked(CarController carController)
    {
        _neuronsPresenter.Show(carController.NeuralNetwork);
    }
}
