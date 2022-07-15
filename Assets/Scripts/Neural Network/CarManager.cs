using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class CarManager : MonoBehaviour
{
	public float timeFrame;
	public int populationSize;
	public GameObject carPrefab;

	public int[] layers = new int[3] { 5, 3, 2 };

	[Range(0.0001f, 1.0f)] public float mutationChance = 0.1f;
	[Range(0.0f, 1.0f)] public float mutationStrength = 0.5f;
	[Range(0.1f, 10.0f)] public float gameSpeed = 1.0f;

	public List<NeuralNetwork> neuralNetworks;

	[HideInInspector] public int generation;

	private string filePath;
	private List<Car> cars;
	private GameObject[] checkPoints;

	void Start()
	{

		if (populationSize % 2 != 0)
		{
			populationSize = 50;
		}

		filePath = Application.persistentDataPath + "Car Manager";

		checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");

		InitializeNetworks();
		LoadData();
		InvokeRepeating("CreateCars", 0.1f, timeFrame);
	}

	void OnApplicationQuit()
	{
		for (int i = 0; i < populationSize; i++)
		{
			neuralNetworks[i].Save();
		}

		SaveData();
	}

	public void CreateCars()
	{
		Time.timeScale = gameSpeed;

		if (cars != null)
		{
			for (int i = 0; i < cars.Count; i++)
			{
				Destroy(cars[i].gameObject);
			}

			SortNetworks();
		}

		cars = new List<Car>();

		for (int i = 0; i < populationSize; i++)
		{
			GameObject obj = Instantiate(carPrefab, new Vector3(0.0f, 2.28f, -16.0f), new Quaternion(0, 180, 0, 0));

			Car car = obj.GetComponent<Car>();
			car.neuralNetwork = neuralNetworks[i];
			cars.Add(car);

			SetCarColor(car, obj);
		}

		generation++;
	}

	public void SetCarColor(Car car, GameObject obj)
	{
		GameObject[] coloredCarParts = new GameObject[]
		{
			obj.transform.Find("Body").gameObject,
			obj.transform.Find("Hut").gameObject
		};

		#region Calculate Color
		Color carPartColor = Color.red;
		float blendValue;

		float fitness = car.neuralNetwork.fitness; 

		if (fitness <= (checkPoints.Length / 3))
		{
			blendValue = Mathf.Clamp01((fitness / (checkPoints.Length / 3)));

			carPartColor = Color.Lerp(Color.red, Color.blue, blendValue);
		}
		else if (fitness <= (2 * (checkPoints.Length / 3)) && fitness > (checkPoints.Length / 3))
		{
			blendValue = Mathf.Clamp01((fitness / (2 * (checkPoints.Length / 3))));

			carPartColor = Color.Lerp(Color.blue, Color.cyan, blendValue);
		}
		else if (fitness <= checkPoints.Length && fitness > (2 * (checkPoints.Length / 3)))
		{
			blendValue = Mathf.Clamp01((fitness / checkPoints.Length));

			carPartColor = Color.Lerp(Color.cyan, Color.green, blendValue);
		}
		#endregion

		foreach (GameObject carPart in coloredCarParts)
		{
			Renderer renderer = carPart.GetComponent<Renderer>();
			renderer.material.SetColor("_Color", carPartColor);
		}
	}

	public void InitializeNetworks()
	{
		neuralNetworks = new List<NeuralNetwork>();

		for (int i = 0; i < populationSize; i++)
		{
			string filePath = Application.persistentDataPath + "Neural Network" + i;

			NeuralNetwork neuralNetwork = new NeuralNetwork(layers, filePath);
			neuralNetwork.Load();
			neuralNetworks.Add(neuralNetwork);
		}
	}

	public void SortNetworks()
	{
		for (int i = 0; i < populationSize; i++)
		{
			cars[i].UpdateFitness();
		}

		neuralNetworks.Sort();
		neuralNetworks[populationSize - 1].Save();

		for (int i = 0; i < populationSize / 2; i++)
		{
			neuralNetworks[i] = neuralNetworks[i + populationSize / 2].Copy(new NeuralNetwork(layers, 
			neuralNetworks[i + populationSize / 2].filePath));

			neuralNetworks[i].Mutate((int)(1 / mutationChance), mutationStrength);
		}
	}

	public void ResetNetworks()
	{
		CancelInvoke("CreateCars");

		for (int i = 0; i < populationSize; i++)
		{
			cars[i].neuralNetwork.Reset();
		}

		ResetData();

		InitializeNetworks();
		InvokeRepeating("CreateCars", 0.1f, timeFrame);
	}

	private void SaveData()
	{
		CarManagerData saveData = new CarManagerData
		{
			Generation = generation
		};

		ReadWriteFile.WriteToBinaryFile(filePath, saveData);
	}

	private void LoadData()
	{
		if (!File.Exists(filePath))
		{
			return;
		}

		CarManagerData saveData = ReadWriteFile.ReadFromBinaryFile<CarManagerData>(filePath);
		generation = saveData.Generation - 1;
	}

	private void ResetData()
	{
		if (!File.Exists(filePath))
		{
			return;
		}

		CarManagerData saveData = ReadWriteFile.ReadFromBinaryFile<CarManagerData>(filePath);
		saveData.Generation = -1;

		ReadWriteFile.WriteToBinaryFile(filePath, saveData);

		LoadData();
	}
}