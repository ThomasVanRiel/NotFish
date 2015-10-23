using UnityEngine;
using System.Collections;

public class SpawnIndicator : MonoBehaviour {

    public GameObject Prefab;
    public Vector3 IndicatorPos;
    

	// Use this for initialization
	void Start () {
        var instance = Instantiate(Prefab, IndicatorPos + Vector3.forward * transform.position.z, Quaternion.Euler(0, 270, 0)) as GameObject;
        instance.name = "Indicator_" + GetComponent<Player>().Key.ToString();
        instance.GetComponent<Link>().SetTarget(gameObject);

        instance.GetComponent<TextMesh>().text = GetComponent<Player>().Key.ToString().ToUpper();

        instance.transform.parent = GameObject.Find("IndicatorFolder").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
