using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject cameraHolder;

	private GameObject[] virtualCameras;
	private int cameraIndex;
	
    private void Start()
    {
		virtualCameras = new GameObject[cameraHolder.transform.childCount];

		for (int i = 0; i < virtualCameras.Length; i++)
		{
			virtualCameras[i] = cameraHolder.transform.GetChild(i).gameObject;
		}

		foreach (GameObject virtualCamera in virtualCameras)
		{
			virtualCamera.SetActive(false);
		}

		cameraIndex = 0;

		virtualCameras[cameraIndex].gameObject.SetActive(true);
    }
	
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			ChangeCameraPosition(-1);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			ChangeCameraPosition(1);
		}
	}

	private void ChangeCameraPosition(int input)
	{
		foreach (GameObject virtualCamera in virtualCameras)
		{
			virtualCamera.SetActive(false);
		}

		cameraIndex += input;

		if (cameraIndex < 0)
		{
			cameraIndex = virtualCameras.Length - 1;
		}
		else if (cameraIndex > virtualCameras.Length - 1)
		{
			cameraIndex = 0;
		}

		virtualCameras[cameraIndex].SetActive(true);
	}
}
