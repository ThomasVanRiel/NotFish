using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextFadeInUI : MonoBehaviour
{

    public float FadeTime;
    private Text _text;
    private Color _color;
    private float _targetAlpha;

    void Awake()
    {
        _text = GetComponent<Text>();
        _color = _text.color;
        _targetAlpha = _color.a;
        _color.a = 0;
        _text.color = _color;
    }

    // Use this for initialization
    void OnEnable()
    {

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
