using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectTiling : MonoBehaviour
{

    public GameObject Prefab;
    public GameObject WaterGroup;
    public float ObjectLength;
    public int MaxObjects;

    private List<GameObject> _objectPool;
    private int _poolIndex = 0;
	Vector3 _lastSpawnPos = Vector3.zero;
	float _offsetDistance = 0;


    // Use this for initialization
    void Awake()
    {
        _objectPool = new List<GameObject>(MaxObjects);
        if (MaxObjects % 2 == 0)
            ++MaxObjects;

		_lastSpawnPos = transform.position;
		_lastSpawnPos -= Vector3.forward * ObjectLength * 3;
        for (int i = 0; i < MaxObjects; i++)
        {
			_lastSpawnPos += Vector3.forward * ObjectLength;
			var instance = Instantiate(Prefab, _lastSpawnPos, Quaternion.identity) as GameObject;
            instance.name = "Water" + i;
            instance.transform.parent = WaterGroup.transform;
            _objectPool.Add(instance);
        }
		_offsetDistance = _lastSpawnPos.z - transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
//        if (_displacement > ObjectLength)
//        {
////            for (int i = 1; i <= 3; i++)
////            {
////                //Ugly code, but at least it works (feel free to change!)
////				Vector3 oldPos = new Vector3(transform.position.x, transform.position.y, 0);
////				Vector3 v2 = Vector3.forward * (Mathf.Floor((transform.position.z + ObjectLength / 2.0f) / ObjectLength) * ObjectLength + Mathf.FloorToInt(MaxObjects - ((float)MaxObjects / 2.0f)) * ObjectLength  + i * ObjectLength - ObjectLength * 3);
////
////                Vector3 pos = oldPos + v2;
////                _objectPool[_poolIndex++].transform.position = pos;
////                if (_poolIndex >= MaxObjects)
////                {
////                    _poolIndex = 0;
////                }
////            }
////
////            _lastPos = transform.position;
//        }
		while((transform.position.z + _offsetDistance) - _lastSpawnPos.z > ObjectLength)
		{
			_lastSpawnPos += Vector3.forward * ObjectLength;
			_objectPool[_poolIndex++].transform.position = _lastSpawnPos;
			if (_poolIndex >= MaxObjects)
				_poolIndex = 0;

		}
    }
}
