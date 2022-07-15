using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI generationText;

	private CarManager carManager;

	void Start()
	{
		carManager = FindObjectOfType<CarManager>();
	}

	void Update()
	{
		if (carManager.generation <= 0)
		{
			generationText.text = " Generation: " + 0;
			return;
		}

		generationText.text = " Generation: " + carManager.generation;
	}
}