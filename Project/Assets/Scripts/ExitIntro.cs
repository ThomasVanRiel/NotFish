using UnityEngine;
using System.Collections;

public class ExitIntro : MonoBehaviour
{

    public float LerpFactor;
    public Vector3 Target;
    public GameObject[] ContinueTexts;
    private bool _active = false;
    public float TextApearTime;

    /* GAME MANAGER */
    GameManager GM;

    void Awake()
    {
        GM = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        TextApearTime -= Time.deltaTime;
        if (TextApearTime <= 0)
        {
            if (!_active)
            {

                foreach (var item in ContinueTexts)
                {
                    item.SetActive(true);
                }
            }
        }


        if (Input.GetButtonDown("Submit"))
        {
            _active = true;
            foreach (var item in ContinueTexts)
            {
                item.SetActive(false);
            }

        }

        if (_active)
        {
            transform.position = Vector3.Slerp(transform.position, Target, LerpFactor);

            if (Vector3.Distance(transform.position, Target) < 0.1)
            {
                transform.position = Target;
                GM.GameState = GameStates.SetupGame;


                this.enabled = false;
            }
        }
    }
}
