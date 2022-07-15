using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.CompareTag("Hexagon"))
				{
					//Do Level Building stuff 
				}
			}
		}
    }
}