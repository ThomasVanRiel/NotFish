using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TextFadeIn : MonoBehaviour
{

    public float FadeTime;
    private TextMesh _text;
    private Color _color;
    private float _targetAlpha;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<TextMesh>();
        _color = _text.color;
        _targetAlpha = _color.a;
        _color.a = 0;
        _text.color = _color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_color.a < _targetAlpha)
        {
            _color.a += (_targetAlpha / FadeTime) * Time.deltaTime;
            _text.color = _color;
        }
    }
}
