using UnityEngine;
using System.Collections;

public class Hoop : MonoBehaviour
{

	public float ImpulseForce = 2000;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			other.GetComponent<Player>().SpeedBoost(ImpulseForce);
		}
	}
}
