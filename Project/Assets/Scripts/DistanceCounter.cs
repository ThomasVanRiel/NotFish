using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistanceCounter : MonoBehaviour {

    private float _DistanceTravelled = 0;
    private Vector3 prevPos;

    public Text UIText;

	// Use this for initialization
	void Start () {
        prevPos = transform.position;
        UIText.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        _DistanceTravelled += (transform.position - prevPos).magnitude;

        string text = System.Math.Round(_DistanceTravelled, 2).ToString();
        text = _DistanceTravelled.ToString("#0.00");
        if (text.IndexOf(".") < 0)
        {
            
        }
        UIText.text = text + "m";
        prevPos = transform.position;
	}

    public void Reset()
    {
        _DistanceTravelled = 0;
    }
}
