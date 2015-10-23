using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    private AudioSource _audioSource;
    private bool _soundInitiated = false;
    public float Frequency = 10;
    public float LowerRange = 100f;
    public float Upperrange = 500f;
    public float PitchVariation = 0.2f;
    public float PitchOffset = 0.1f;
    private float _random = 0;
    private float _clipLength = 0;
    //public AudioClip Clip;// = null;
	// Use this for initialization
	void Start () {
        //initiate audio source
        _audioSource = GetComponent<AudioSource>();
        float r = Random.Range(-PitchVariation, PitchVariation);
        _audioSource.pitch += r;
        //_audioSource.clip = Clip;
        //set random start
        _random = Random.Range(LowerRange/Frequency, Upperrange/Frequency);
        _random += PitchOffset;
        _audioSource.Pause();
        _clipLength = _audioSource.clip.length;
    }

	
	// Update is called once per frame
	void Update () {
        if (Time.time > _random && _soundInitiated == false)
        {
            PlaySound();
            _soundInitiated = true;
        }
	
	}

    void PlaySound()
    {
        _audioSource.Play();
        float lower = LowerRange / Frequency;
        if (lower < _clipLength)
            lower = _clipLength;
        float upper = Upperrange / Frequency;
        if (upper < _clipLength)
            upper = _clipLength;
        Invoke("PlaySound", Random.Range(lower,upper));

    }
}
