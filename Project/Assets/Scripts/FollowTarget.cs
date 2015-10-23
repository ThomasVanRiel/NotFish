using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowTarget : MonoBehaviour
{
	public GameObject Target;

	public bool LockHeight = true;
	
	public List<PlayerInfo> TargetArr;
	public Vector3 ForcedOffset = Vector3.zero;
	public float SmoothingMultiplier = 10;
	Vector3 _offset = Vector3.zero;
	Vector3 _velocity = Vector3.zero;

	bool _hasStarted = false;

	GameManager GM;
	Transform _transf;


	void Awake()
	{
		GM = GameManager.Instance;
		_transf = transform;
	}
	void Start ()
	{
	}

	void Update ()
	{
		if (GM.GameState != GameStates.InGame && GM.GameState != GameStates.EndGame)
			return;
		// First time initialization only
		if (!_hasStarted) {
			_offset = _transf.position - CalculateAverage();
			_hasStarted = true;
		}
		// Smoothly follow target
		Vector3 newPos = Vector3.SmoothDamp(_transf.position, CalculateAverage() + _offset + ForcedOffset, ref _velocity, SmoothingMultiplier * Time.smoothDeltaTime);
		// Lock Y-axis
		if (LockHeight) 
			newPos.y = _transf.position.y;
		// Set new position
		_transf.position = newPos;
	}

	Vector3 CalculateAverage()
	{
		// If no entries in List
		if (TargetArr == null || TargetArr.Count <= 0) {
			if (Target != null) 
				return Target.transform.position;
			return Vector3.zero;
		}
		// Average the list
		Vector3 output = Vector3.zero;
//		foreach(PlayerInfo t in TargetArr)
//		{
//			output += t.transform.position;
//		}
//		return output / TargetArr.Count;		
		foreach(PlayerInfo t in TargetArr)
		{
			if (t.transform.position.z > output.z) 
				output = t.transform.position;
		}
		//Debug.Log(output);
		return output;
	}
}
