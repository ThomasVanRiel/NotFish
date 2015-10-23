using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class Message : MonoBehaviour
{

    private bool _isActive = false;
    private TextMesh _mesh;
    private Color _color;
    private float _life = 0;

    public Vector3 Velocity;
    public float LifeTime = 2;
    public float StartAlpha = 1;

    // Use this for initialization
    void Awake()
    {
        _mesh = GetComponent<TextMesh>();
        _color = _mesh.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            transform.position += Velocity  * Time.deltaTime;
            _color.a -= (StartAlpha / LifeTime) * Time.deltaTime;
            _mesh.color = _color;

            _life += Time.deltaTime;
            if (_life > LifeTime)
                _isActive = false;
        }

    }

    public void Activate(string message)
    {
        transform.position = transform.parent.position;
        _mesh.text = message;
        _color.a = StartAlpha;
        _mesh.color = _color;
        _life = 0;
        _isActive = true;
    }
}
