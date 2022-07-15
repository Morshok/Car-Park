using System;
using System.IO;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
	public float fitness = 0;
	public string filePath;

	private int[] layers;

	private float[][] neurons;
	private float[][] biases;

	private float[][][] weights;

	//Constructor for the Neural Network
	public NeuralNetwork(int[] layers, string filePath)
	{
		this.layers = new int[layers.Length];
		this.filePath = filePath;

		for (int i = 0; i < layers.Length; i++)
		{
			this.layers[i] = layers[i];
		}

		InitializeNeurons();
		InitializeBiases();
		InitializeWeights();
	}
	
	public float Activate(float value)
	{
		return (float)Math.Tanh(value);
	}

	//The feedforward function of the Neural Network
	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++)
		{
			neurons[0][i] = inputs[i];
		}

		for (int i = 1; i < layers.Length; i++)
		{
			int layer = i - 1;

			for (int j = 0; j < neurons[i].Length; j++)
			{
				float value = 0f;

				for (int k = 0; k < neurons[i - 1].Length; k++)
				{
					value += weights[i - 1][j][k] * neurons[i - 1][k];
				}

				neurons[i][j] = Activate(value + biases[i][j]);
			}
		}

		return neurons[neurons.Length - 1];
	}

	//Method for comparing the fitness of Neural Networks
	public int CompareTo(NeuralNetwork other)
	{
		if (other == null) return 1;

		if (fitness > other.fitness) return 1;
		else if (fitness < other.fitness) return -1;
		else return 0;
	}

	//Method for making a copy of a Neural Network
	public NeuralNetwork Copy(NeuralNetwork neuralNetwork)
	{
		for (int i = 0; i < biases.Length; i++)
		{
			for (int j = 0; j < biases[i].Length; j++)
			{
				neuralNetwork.biases[i][j] = biases[i][j];
			}
		}

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					neuralNetwork.weights[i][j][k] = weights[i][j][k];
				}
			}
		}

		return neuralNetwork;
	}

	//Method used as a simple mutation function for any genetic implementation
	public void Mutate(int chance, float value)
	{
		for (int i = 0; i < biases.Length; i++)
		{
			for (int j = 0; j < biases[i].Length; j++)
			{
				biases[i][j] = (Random.Range(0.0f, (float)chance) <= 5.0f) ?
				biases[i][j] += Random.Range(-value, value) : biases[i][j];
			}
		}

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					weights[i][j][k] = (Random.Range(0.0f, (float)chance) <= 5.0f) ?
					weights[i][j][k] += Random.Range(-value, value) : weights[i][j][k];
				}
			}
		}
	}

	//Method for saving the Neural Network
	public void Save()
	{
		NeuralNetworkSaveData saveData = new NeuralNetworkSaveData
		{
			Fitness = fitness,
			Biases = biases,
			Weights = weights
		};

		for (int i = 0; i < biases.Length; i++)
		{
			for (int j = 0; j < biases[j].Length; j++)
			{
				saveData.Biases[i][j] = biases[i][j];
			}
		}

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					saveData.Weights[i][j][k] = weights[i][j][k];
				}
			}
		}

		ReadWriteFile.WriteToBinaryFile(filePath, saveData);
	}

	//Method for loading the Neural Network
	public void Load()
	{
		if (!File.Exists(filePath))
		{
			return;
		}

		NeuralNetworkSaveData saveData = ReadWriteFile.ReadFromBinaryFile<NeuralNetworkSaveData>(filePath);

		fitness = saveData.Fitness;

		for (int i = 0; i < biases.Length; i++)
		{
			for (int j = 0; j < biases[i].Length; j++)
			{
				biases[i][j] = saveData.Biases[i][j];
			}
		}

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					weights[i][j][k] = saveData.Weights[i][j][k];
				}
			}
		}
	}

	//Method for resetting values of the Neural Network
	public void Reset()
	{
		if (!File.Exists(filePath))
		{
			return;
		}

		NeuralNetworkSaveData saveData = ReadWriteFile.ReadFromBinaryFile<NeuralNetworkSaveData>(filePath);

		saveData.Fitness = 0.0f;

		for (int i = 0; i < biases.Length; i++)
		{
			for (int j = 0; j < biases[i].Length; j++)
			{
				saveData.Biases[i][j] = Random.Range(-0.5f, 0.5f);
			}
		}

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					saveData.Weights[i][j][k] = Random.Range(-0.5f, 0.5f);
				}
			}
		}

		ReadWriteFile.WriteToBinaryFile(filePath, saveData);
	}

	//Initializes the neurons of the Neural Network
	private void InitializeNeurons()
	{
		List<float[]> neuronsList = new List<float[]>();

		for (int i = 0; i < layers.Length; i++)
		{
			neuronsList.Add(new float[layers[i]]);
		}

		neurons = neuronsList.ToArray();
	}

	//Initializes the biases of the Neural Network
	private void InitializeBiases()
	{
		List<float[]> biasList = new List<float[]>();

		for (int i = 0; i < layers.Length; i++)
		{
			float[] bias = new float[layers[i]];

			for (int j = 0; j < layers[i]; j++)
			{
				bias[j] = Random.Range(-0.5f, 0.5f);
			}

			biasList.Add(bias);
		}

		biases = biasList.ToArray();
	}

	//Initializes the weights of the Neural Network
	private void InitializeWeights()
	{
		List<float[][]> weightsList = new List<float[][]>();

		for (int i = 1; i < layers.Length; i++)
		{
			List<float[]> layerWeightsList = new List<float[]>();

			int neuronsInPreviousLayer = layers[i - 1];

			for (int j = 0; j < neurons[i].Length; j++)
			{
				float[] neuronWeights = new float[neuronsInPreviousLayer];

				for (int k = 0; k < neuronsInPreviousLayer; k++)
				{
					//float sd = 1.0f / ((neurons[i].Length + neuronsInPreviousLayer) / 2.0f);

					neuronWeights[k] = Random.Range(-0.5f, 0.5f);
				}

				layerWeightsList.Add(neuronWeights);
			}

			weightsList.Add(layerWeightsList.ToArray());
		}

		weights = weightsList.ToArray();
	}
}