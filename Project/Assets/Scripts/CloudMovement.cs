using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {

    public float HeightDifference;
    private Vector3 _origPos;
    private float _angle;

	// Use this for initialization
	void Awake () {
        _origPos = transform.position;
        _angle = Random.Range(0, 360);
	}
	
	// Update is called once per frame
	void Update () {
        _angle += Time.deltaTime / 2;
        _angle = Mathf.Repeat(_angle, 360);
        transform.position = _origPos + Vector3.up * Mathf.Sin(_angle) * HeightDifference;
	}

    public void UpdateOriginalPosition()
    {
        _origPos = transform.position;
    }
}
