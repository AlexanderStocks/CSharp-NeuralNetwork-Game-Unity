using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;
    private float fitness;

    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        InitNeurons();
        InitWeights();
    }

    public NeuralNetwork(NeuralNetwork networkToCopy)
    {
        layers = new int[networkToCopy.layers.Length];

        for (int i = 0; i < networkToCopy.layers.Length; i++)
        {
            layers[i] = networkToCopy.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(networkToCopy.weights);
    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    private void InitNeurons()
    {
        neurons = layers.Select(i => new float[i]).ToArray();
    }

    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();

        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeights = new List<float[]>();

            int neuronsInPreviousLayer = layers[i - 1];

            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];

                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeights.Add(neuronWeights);
            }

            weightsList.Add(layerWeights.ToArray());
        }
        weights = weightsList.ToArray();
    }
    
    public float[] CalculateValues(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }
        return neurons[neurons.Length - 1];
    }

    public void Evolve()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    int rand = UnityEngine.Random.Range(1, 500);

                    switch (rand)
                    {
                        case 2:
                            weight *= -1;
                            break;

                        case 4:
                            weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                            break;

                        case 6:
                            float mult = UnityEngine.Random.Range(0, 1f) + 1f;
                            break;

                        case 8:
                            float multiply = UnityEngine.Random.Range(0f, 1f);
                            weight *= multiply;
                            break;
                    }
                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void AddFitness(float fitness)
    {
        this.fitness += fitness;
    }

    public void SetFitness(float fitness)
    {
        this.fitness = fitness;
    }

    public float GetFitness()
    {
        return fitness;
    }

    public int CompareTo(NeuralNetwork other)
    {
        if (other == null)
        {
            return 1;
        }
        if (fitness > other.fitness)
        {
            return 1;
        } else if (fitness < other.fitness) {
            return -1;
        } else {
            return 0;
        }
    }
}
