using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudSpawner : MonoBehaviour
{
    public GameObject CloudGroup;

    public GameObject[] Clouds;
    public float SpawnIntervalFactor = 4f;
    public float positionFactor = 5f;
    public float MinScale = 1;
    public float MaxScale = 1;
    public float Offset = 20f;
    public float MinDistance = 0;
    public float MaxDistance = 0;

    public float DepthWidthRelation = 1;

    private float _traveledDistance = 0;
    private float _previouszPos;
    private int _poolIndex = 0;
    private List<GameObject> _pool;


    // Use this for initialization
    void Awake()
    {

        _pool = new List<GameObject>((int)((Offset * 2) / SpawnIntervalFactor));

        _previouszPos = transform.position.z;
        for (float i = -Offset; i < Offset; i += SpawnIntervalFactor)
        {
            //Debug.Log("spawning initial cloud");
            Vector3 pos = transform.position;
            pos.z += i;
            SpawnCloud(pos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _traveledDistance += transform.position.z - _previouszPos;

        //spawn them clouds ahead of camera
        if (_traveledDistance > SpawnIntervalFactor)
        {
            Vector3 pos = transform.position;
            pos.z += Offset;
            NewCloud(pos);
            _traveledDistance = 0;

        }
        _previouszPos = transform.position.z;
    }

    void NewCloud(Vector3 pos)
    {
        float depthPos = Random.Range(-MaxDistance, MinDistance); //Away from camera
        pos.x += depthPos;
        pos.z += Random.Range(-positionFactor, positionFactor);//SideWays
        pos.y += Random.Range(-positionFactor, positionFactor);
        pos.x += Random.Range(-positionFactor, positionFactor);

        Quaternion randRot = Random.rotation;
        if (depthPos / (-MaxDistance) > 0.5f)
        {
            randRot = Quaternion.identity;
        }

        _pool[_poolIndex].transform.position = pos;
        _pool[_poolIndex].GetComponent<CloudMovement>().UpdateOriginalPosition();
        _pool[_poolIndex].transform.rotation = randRot;
        _pool[_poolIndex].transform.localScale = Vector3.one * Random.Range(MinScale, MaxScale) + Vector3.one * depthPos / (-MaxDistance) * DepthWidthRelation + Vector3.forward * depthPos / (-MaxDistance) * DepthWidthRelation * 2;

        ++_poolIndex;

        if (_poolIndex >= _pool.Count)
            _poolIndex = 0;
    }

    private void SpawnCloud(Vector3 pos)
    {
        float depthPos = Random.Range(-MaxDistance, MinDistance); //Away from camera
        pos.x += depthPos;
        pos.z += Random.Range(-positionFactor, positionFactor);//SideWays
        pos.y += Random.Range(-positionFactor, positionFactor);
        pos.x += Random.Range(-positionFactor, positionFactor);

        Quaternion randRot = Random.rotation;
        if (depthPos/(-MaxDistance) > 0.5f)
        {
            randRot = Quaternion.identity;
        }

        GameObject cloud = Instantiate(Clouds[Random.Range(0, Clouds.Length)], pos, randRot) as GameObject;

        cloud.transform.localScale = Vector3.one * Random.Range(MinScale, MaxScale) + Vector3.one * depthPos / (-MaxDistance) * DepthWidthRelation + Vector3.forward * depthPos / (-MaxDistance) * DepthWidthRelation * 2;

        cloud.transform.parent = CloudGroup.transform;

        _pool.Add(cloud);
    }
}
