using UnityEngine;
using System.Collections;

public enum MineState
{
	IDLE,
	EXPLODING,
	INACTIVE,
	RELEASED,
	BOBBING
}

public class MineBehaviour : MonoBehaviour
{
	public MineState State = MineState.IDLE;
	public float ExplodeTime;
	public float ExplodeSpeed;
	private SkinnedMeshRenderer _renderer;
	float _velocity = .5f;
	private float _counter = 0;
	float _bobTimer = 0;
	
	GameManager GM;
	Transform _transf;
	
	// Use this for initialization
	void Awake ()
	{
		GM = GameManager.Instance;
		_transf = transform;
		
		//Instantiate material
		_renderer = GetComponentInChildren<SkinnedMeshRenderer> ();
		_renderer.material = new Material (_renderer.sharedMaterial);
		_renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		switch (State) {
		case MineState.IDLE:
			break;
		case MineState.EXPLODING:
			DoExplode ();
			break;
		case MineState.INACTIVE:
			break;
		case MineState.RELEASED:
			ProcessReleaseMovement ();
			break;
		case MineState.BOBBING:
			ProcessBobbingMovement ();
			break;
		default:
			break;
		}
	}
	
	void DoExplode ()
	{
		_counter += Time.deltaTime;
		
		if (_counter >= ExplodeTime) {
			State = MineState.INACTIVE;
			_renderer.enabled = false;
			_counter = 0;
			_renderer.material.SetFloat ("_Explosion", 0);
			return;
		}
		
		float progress = _counter / ExplodeTime;
		_renderer.material.SetFloat ("_Explosion", progress);
	}
	
	void ProcessReleaseMovement ()
	{
		// Move up
		if (_transf.position.y < GM.WaterLevel) {
			Vector3 newPos = transform.position;
			newPos += Vector3.up * _velocity * Time.deltaTime + Vector3.forward * Mathf.Sin (Time.time * 3.5f) * .005f;
//			Debug.Log (newPos);
			transform.position = newPos;
		}
		// Bobbing
		else
		{
			State = MineState.BOBBING;
		}
	}
	void ProcessBobbingMovement()
	{
		Vector3 newPos = transform.position;
		_bobTimer += Time.deltaTime;
		newPos.y = Mathf.Sin (_bobTimer) * .07f;
		transform.position = newPos;
	}
	
	public void Reset ()
	{
		State = MineState.IDLE;
		_renderer.enabled = true;
	}
	
	void OnTriggerEnter (Collider other)
	{
        if (GameManager.Instance.PlayersArr.Count > 1)
        {
			if (State == MineState.IDLE || State == MineState.RELEASED || State == MineState.BOBBING)
            {
                GetComponent<AudioSource>().Play();
                other.GetComponent<Player>().DIE();
				State = MineState.EXPLODING;
            }
        }
	}
	public void DeActivate()
	{
		State = MineState.INACTIVE;
		_renderer.enabled = false;
        _bobTimer = 0;
		_counter = 0;
		_renderer.material.SetFloat("_Explosion", 0);
	}
}