using UnityEngine;
using System.Collections;

public class PlaneBehaviour : MonoBehaviour {

    public Vector3 Velocity;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + Velocity * Time.deltaTime;
	}
}
