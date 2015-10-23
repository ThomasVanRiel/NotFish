using UnityEngine;
using System.Collections;

public class Link : MonoBehaviour
{

    public bool LockHeight;
    private bool _isActive = false;
    public GameObject Target;
    private Vector3 _offset;

    // Use this for initialization
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            if (Target == null)
            {
                _isActive = false;
                Destroy(gameObject);
                return;
            }

            Vector3 newPos;
            newPos = Target.transform.position + _offset;
            if (LockHeight)
            {
                newPos.y = transform.position.y;
            }
            transform.position = newPos;
        }

    }

    public void SetTarget(GameObject target)
    {
        _isActive = true;

        Target = target;
        _offset = Target.transform.position - transform.position;

    }
}
