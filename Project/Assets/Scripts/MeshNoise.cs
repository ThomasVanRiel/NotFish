using UnityEngine;
using System.Collections;

public class MeshNoise : MonoBehaviour {

    private Vector3[] _originalVerts;

	// Use this for initialization
	void Start () {
        _originalVerts = new Vector3[GetComponent<MeshFilter>().mesh.vertexCount];
        _originalVerts = GetComponent<MeshFilter>().mesh.vertices;
	}
	
	// Update is called once per frame
	void Update () {
        var verts = GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            float noiseMod = 0.05f;
            verts[i] = _originalVerts[i] + new Vector3(Random.Range(0, noiseMod), Random.Range(0, noiseMod), Random.Range(0, noiseMod));
        }

        GetComponent<MeshFilter>().mesh.vertices = verts;
	}
}
