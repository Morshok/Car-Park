using UnityEngine;

public class Car : MonoBehaviour
{
	public float speed;
	public float rotation;
	public float raycastDistance;
	public LayerMask raycastMask;

	private float[] input = new float[5];
	public NeuralNetwork neuralNetwork;

	public int score;
	public bool collided;

	void FixedUpdate()
	{
		if (!collided)
		{
			for (int i = 0; i < 5; i++)
			{
				Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;
				RaycastHit hit;
				Ray Ray = new Ray(transform.position, newVector);

				if (Physics.Raycast(Ray, out hit, raycastDistance, raycastMask))
				{
					input[i] = (raycastDistance - hit.distance) / raycastDistance;
				}
				else
				{
					input[i] = 0;
				}
			}

			float[] output = neuralNetwork.FeedForward(input);

			transform.Rotate(0, output[0] * rotation, 0, Space.World);
			transform.position += this.transform.right * output[1] * speed;
		}
	}

	void OnDrawGizmos()
	{
		for (int i = 0; i < 5; i++)
		{
			Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right * raycastDistance;
			Ray Ray = new Ray(transform.position, newVector);

			Gizmos.DrawRay(Ray);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
		{
			GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
			for (int i = 0; i < checkPoints.Length; i++)
			{
				if (collision.collider.gameObject == checkPoints[i] && i == (score + 1 + checkPoints.Length) % checkPoints.Length)
				{
					score++;
					break;
				}
			}
		}
		else if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Learner"))
		{
			collided = true;
		}
	}

	public void UpdateFitness()
	{
		neuralNetwork.fitness = score;
	}
}