using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageSystem : MonoBehaviour {

    const int POOLSIZE = 20;
    private Message[] _pool = new Message[POOLSIZE];
    private int _poolIndex = 0;

    public Message Prefab;

    void Awake()
    {
        for (int i = 0; i < POOLSIZE; i++)
        {
            _pool[i] = Instantiate(Prefab, transform.position, transform.rotation) as Message;
            _pool[i].transform.parent = transform;
        }
    }

    #region Singleton
    static MessageSystem instance;

    public static MessageSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageSystem>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<MessageSystem>();
                }
            }
            return instance;
        }
    }
    #endregion

    void Update()
    {

    }

    public void DispatchMessage(string message)
    {
        _pool[_poolIndex++].Activate(message);
        if (_poolIndex >= POOLSIZE)
            _poolIndex = 0;
    }

    public void DispatchMessage(string message, float time)
    {
        StartCoroutine(DispatchAfterTime(message, time));
    }

    IEnumerator DispatchAfterTime(string message, float time)
    {
        yield return new WaitForSeconds(time);
        DispatchMessage(message);
    }
}
