using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{

    public GameObject KeyInfo;
    public GameObject EnterInfo;

    // Use this for initialization
    void Start()
    {

    }

    void OnDisable()
    {
        EnterInfo.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {
        EnterInfo.SetActive(GameManager.Instance.PlayersArr.Count >= 2);
    }


}
