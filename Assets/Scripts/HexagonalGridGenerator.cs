using UnityEngine;
using System.Collections.Generic;

public class HexagonalGridGenerator : MonoBehaviour
{
	#region Variables

	[SerializeField] private Transform hexagonPrefab;

	[SerializeField] private int gridWidth = 11;
	[SerializeField] private int gridHeight = 11;

	[SerializeField] private float gap = 0.0f;

	private List<Transform> hexagons;

	private float hexagonWidth = 1.732f;
	private float hexagonHeight = 2.0f;

	private Vector3 startPosition;

	#endregion

	private void Start()
	{
		hexagons = new List<Transform>();

		AddGap();
		CalculateStartPosition();
		CreateGrid();
	}

	private void AddGap()
	{
		hexagonWidth += hexagonWidth * gap;
		hexagonHeight += hexagonHeight * gap;
	}

	private void CalculateStartPosition()
	{
		float offset = 0.0f;

		if (gridHeight / 2 % 2 != 0)
		{
			offset = hexagonWidth / 2;
		}

		float x = -hexagonWidth * (gridWidth / 2) - offset;
		float z = hexagonHeight * 0.75f * (gridHeight / 2);

		startPosition = new Vector3(x, 0, z);
	}

	private void CreateGrid()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				Transform hexagon = Instantiate(hexagonPrefab) as Transform;

				Vector2 gridPosition = new Vector2(x, y);

				hexagon.position = CalculateWorldPosition(gridPosition);
				hexagon.transform.SetParent(transform);
				hexagon.name = "Hexagon " + x + " | " + y;

				hexagons.Add(hexagon);
			}
		}
	}

	private Vector3 CalculateWorldPosition(Vector2 gridPosition)
	{
		float offset = 0.0f;

		if (gridPosition.y % 2 != 0)
		{
			offset = hexagonWidth / 2;
		}

		float x = startPosition.x + gridPosition.x * hexagonWidth + offset;
		float z = startPosition.z + gridPosition.y * hexagonHeight * 0.75f;

		return new Vector3(x, 0, z);
	}
}