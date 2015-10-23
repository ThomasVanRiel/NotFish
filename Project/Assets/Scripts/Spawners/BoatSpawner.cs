using UnityEngine;
using System.Collections;

public class BoatSpawner : MonoBehaviour
{
    const int POOLSIZE = 3;

    public GameObject Prefab;
    public GameObject ShipFolder;
    public float SpawnInterval;
    private float _traveledDistance = 2000;
    private float _previousZPos;
    public float ZOffset;

    public Vector2 PositionDeviation;

    private GameObject[] _pool = new GameObject[POOLSIZE];
    private int _poolIndex;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < POOLSIZE; i++)
        {
            _pool[i] = Instantiate(Prefab, Vector3.back * 100 + Vector3.up * 33, Quaternion.identity) as GameObject;
            _pool[i].transform.parent = ShipFolder.transform;
        }

        _previousZPos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        _traveledDistance += transform.position.z - _previousZPos;

        if (_traveledDistance > SpawnInterval)
        {
            _traveledDistance = 0;

            Vector3 spawnPos = transform.position + Vector3.forward * ZOffset;
            spawnPos.y += Random.Range(-PositionDeviation.y, PositionDeviation.y);
            spawnPos.z += Random.Range(-PositionDeviation.x, PositionDeviation.x);


            _pool[_poolIndex].transform.position = spawnPos;
            _pool[_poolIndex].transform.rotation = Quaternion.Euler(-90, Mathf.RoundToInt(Random.Range(0, 2)) * 180, 0);
            //_pool[_poolIndex].transform.rotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);

            if (_poolIndex >= POOLSIZE)
                _poolIndex = 0;
        }
        _previousZPos = transform.position.z;
    }

}
