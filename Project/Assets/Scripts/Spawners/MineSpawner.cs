using UnityEngine;
using System.Collections;

public class MineSpawner : MonoBehaviour
{
    const int POOLSIZE = 10;

    public GameObject Prefab;
    public GameObject MineFolder;
    public float SpawnInterval;
    private float _traveledDistance = 0;
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
            _pool[i] = Instantiate(Prefab, Vector3.back * 100, Quaternion.identity) as GameObject;
            _pool[i].transform.parent = MineFolder.transform;
        }

        _previousZPos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PlayersArr.Count >= 1)
        {
            _traveledDistance += transform.position.z - _previousZPos;

            if (_traveledDistance > SpawnInterval)
            {
                _traveledDistance = 0;

                Vector3 spawnPos = transform.position + Vector3.forward * ZOffset;
                spawnPos.y += Random.Range(-PositionDeviation.y, PositionDeviation.y);
                spawnPos.z += Random.Range(-PositionDeviation.x, PositionDeviation.x);
//				Debug.Log(spawnPos);
                _pool[_poolIndex].transform.position = spawnPos;
				MineBehaviour mb = _pool[_poolIndex++].GetComponent<MineBehaviour>();
				if (mb != null)
					mb.Reset();

				if (_poolIndex >= POOLSIZE)
					_poolIndex = 0;
            }
            _previousZPos = transform.position.z;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < POOLSIZE; i++)
        {
            _pool[i].GetComponent<MineBehaviour>().DeActivate();
        }
    }
}
