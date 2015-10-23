using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class CountDownObject : MonoBehaviour
{

    private float _timer = 0;
    private TextMesh _mesh;
    private bool _isActive = false;

    [Tooltip("Write an * where the number has to be, eg. [*]")]
    public string TextTemplate;
    public float TimeAfterComplete;
    public string CompleteText;

    // Use this for initialization
    void Awake()
    {
        _mesh = GetComponent<TextMesh>();
        GetComponent<MeshRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            _timer -= Time.deltaTime;

            if (_timer <= -TimeAfterComplete)
            {
                _isActive = false;
                GetComponent<MeshRenderer>().enabled = false;
                return;
            }
            else if (_timer <= 0)
            {
                _mesh.text = TextTemplate.Replace("*", CompleteText);
                return;
            }

            _mesh.text = TextTemplate.Replace("*", "" + Mathf.CeilToInt(_timer));

        }
    }

    public void StartCountDown(float time)
    {
        _timer = time;
        string text = TextTemplate;
        _mesh.text = text.Replace("*", "" + Mathf.CeilToInt(_timer));

        GetComponent<MeshRenderer>().enabled = true ;
        _isActive = true;
    }
}
