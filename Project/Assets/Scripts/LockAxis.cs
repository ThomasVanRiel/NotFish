using UnityEngine;
using System.Collections;

public class LockAxis : MonoBehaviour
{

    public bool LockX;
    public float XPos;
    public bool LockY;
    public float YPos;
    public bool LockZ;
    public float ZPos;

    public bool LockRotationToIdentity;


    // Use this for initialization
    void Awake()
    {
        Debug.LogError("This script does not work");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
