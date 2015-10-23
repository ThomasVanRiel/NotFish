using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TextFadeOut : MonoBehaviour {

    public float FadeAfter;
    public float FadeTime;
    private TextMesh _text;
    private Color _color;
    private float _initAlpha;
    private float _counter = 0;

	// Use this for initialization
	void Start () {
        _text = GetComponent<TextMesh>();
        _color = _text.color;
        _initAlpha = _color.a;
	}
	
	// Update is called once per frame
	void Update () {
        _counter += Time.deltaTime;

        if (_counter > FadeAfter)
        {
            _color.a -= (_initAlpha / FadeTime) * Time.deltaTime;
            _text.color = _color;
            if (_color.a <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
