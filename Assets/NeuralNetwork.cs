using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{

    public Neuron[][] Neurons;

    public void CreateWithRandomWeights(int inputs ,int layers, int width, int outputs)
    {
        Neurons = new Neuron[layers][];

        Neurons[0] = new Neuron[inputs];
        Neurons[layers - 1] = new Neuron[outputs];

        for (int i = 1; i < layers - 1; i++)
        {
            Neurons[i] = new Neuron[width];
        }

        for (int i = 0; i < Neurons.Length; i++)
        {
            for (int t = 0; t < Neurons[i].Length; t++)
            {
                Neurons[i][t] = new Neuron();

                if (i > 0)
                {
                    Neurons[i][t].Initialize(Neurons[i - 1].Length);

                    Neurons[i][t].SetRandomWeights();
                }

            }
        }
    }

    public void CreateBaseOn(NeuralNetwork neuralNetwork, float aducationValue)
    {
        Neurons = new Neuron[neuralNetwork.Neurons.Length][];

        

        for (int x = 0; x < neuralNetwork.Neurons.Length; x++)
        {
            Neurons[x] = new Neuron[neuralNetwork.Neurons[x].Length];
            for (int y = 0; y < neuralNetwork.Neurons[x].Length; y++)
            {
                
                Neurons[x][y] = neuralNetwork.Neurons[x][y].Copy();

                //Debug.Log(neuralNetwork.Neurons[x+1][y+1].Weights[0]);
            }
        }

        for (int i = 1; i < Neurons.Length; i++)
        {
            for (int t = 0; t < Neurons[i].Length; t++)
            {
                //Debug.Log(Neurons[i][t].Value);
                Neurons[i][t].ChangeWeights(aducationValue);
            }
        }
    }

    public float[] Run(float[] inputs)
    {

        for (int i = 0; i < Neurons[0].Length; i++)
        {
            Neurons[0][i].Value = inputs[i];
        }

        for (int i = 1; i < Neurons.Length; i++)
        {
            for (int t = 0; t < Neurons[i].Length; t++)
            {
                Neurons[i][t].Value = 0;
                for (int f = 0; f < Neurons[i - 1].Length; f++)
                {
                    
                    //Debug.Log(Neurons[i][t].Weights.Length);
                    Neurons[i][t].Value += Neurons[i][t].Weights[f] * Neurons[i - 1][f].Value;
                }
            }
        }

        float[] result = new float[Neurons[Neurons.Length - 1].Length];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Neurons[Neurons.Length - 1][i].Value;
        }

        return result;
    }
}

public struct Neuron
{
    public float[] Weights;

    public float Value;

    public void Initialize(int inputs)
    {
        Weights = new float[inputs];
    }

    public void SetRandomWeights()
    {
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] = Random.value * 2 - 1;
        }
    }

    public void ChangeWeights(float value)
    {
        for (int i = 0; i < Weights.Length; i++)
        {
            float was = Weights[i];
            Weights[i] = Mathf.Clamp( Weights[i] + (Random.value * 2 - 1) * value, -1, 1);
        }
    }

    public Neuron Copy()
    {
        Neuron copy = new Neuron();

        if (Weights != null)
        {
            float[] weights = new float[Weights.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = Weights[i];
            }

            copy.Weights = weights;
        }

        
        copy.Value = Value;

        return copy;
    }
}
