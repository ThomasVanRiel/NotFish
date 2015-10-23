using UnityEngine;
using System.Collections;

public class RegisterCamera : MonoBehaviour
{
	void Awake ()
	{
		// Register this gameobject with the GameManager
		GameManager.Instance.SetCamera(gameObject);
	}
}
