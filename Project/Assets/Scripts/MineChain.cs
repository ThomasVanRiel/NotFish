using UnityEngine;
using System.Collections;

public class MineChain : MonoBehaviour
{
	MineBehaviour MB;

	// Use this for initialization
	void Start ()
	{
		MB = transform.parent.GetComponent<MineBehaviour>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && MB.State == MineState.IDLE) {
			MB.State = MineState.RELEASED;
		}
	}
}
