using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* GAMESTATES */
public enum GameStates
{
	Intro,
	SetupGame,
	CountDown,
	InGame,
	EndGame
}
/* PLAYERS */
public class PlayerInfo
{
	public GameObject gameObject;
	public Transform transform;
	public Rigidbody rigidbody;
	public Player script;

	// Constructor
	public PlayerInfo (GameObject player)
	{
		gameObject = player;
		transform = player.transform;
		rigidbody = player.GetComponent<Rigidbody> ();
		script = player.GetComponent<Player> ();
	}
}

public class CameraInfo
{
	public GameObject gameObject;
	public Transform transform;
	public Rigidbody rigidbody;
	public Camera camera;
	public FollowTarget followScript;
	public ExitIntro introScript;

	// Constructor
	public CameraInfo (GameObject cam)
	{
		gameObject = cam;
		transform = cam.transform;
		rigidbody = cam.GetComponent<Rigidbody> ();
		camera = cam.GetComponent<Camera> ();
		followScript = cam.GetComponent<FollowTarget> ();
		introScript = cam.GetComponent<ExitIntro>();
	}
}

public struct PlayerCombination
{
	public HSBColor color;
	public int mesh;

	public PlayerCombination (HSBColor col, int mes)
	{
		color = col;
		mesh = mes;
	}
}

public class GameManager : MonoBehaviour
{	
	/* SINGLETON */
	#region Singleton
	static GameManager instance;
	
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<GameManager> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
//					obj.hideFlags = HideFlags.HideAndDontSave;
					instance = obj.AddComponent<GameManager> ();
				}
			}
			return instance;
		}
	}
	#endregion

	public Object PlayerPrefab;
	public List<PlayerInfo> PlayersArr = new List<PlayerInfo> ();
	public PlayerInfo WinningPlayer;
	public CountDownObject CountDownText;
    public Tutorial TutorialObject;
	List<string> _namesArr;
	List<PlayerCombination> _combinations;
	int MeshAmount = 1;
	bool _buttonDown = false;
	public CameraInfo Cam;
	MessageSystem MS;
	public float WaterLevel = 0;

	/* TIMING */
	float _processTimer = 0;
	public float CountdownTime = 3;

    #region LongArrays
	KeyCode[] _validCodesArr = {
		KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D,
		KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
		KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
		KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
		KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
		KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
		KeyCode.Y, KeyCode.Z
	};
    #endregion

	GameStates _gameState = GameStates.Intro;

	public GameStates GameState {
		get {
			return _gameState;
		}
		set {
			switch (value) {
			case GameStates.Intro:
				break;
			case GameStates.SetupGame:	
				StartSetup ();
				break;
			case GameStates.CountDown:
				StartCountdown ();
				break;
			case GameStates.InGame:
				break;
			case GameStates.EndGame:
				StartEndGame ();
				break;
			}
//			Debug.Log(value);
			_processTimer = 0;
			_gameState = value;
		}
	}

	void Awake ()
	{
		MS = MessageSystem.Instance;

		PlayerPrefab = Resources.Load ("Player");
        CountDownText = GameObject.Find("CountDown").GetComponent<CountDownObject>();
        TutorialObject = GameObject.Find("Tutorial").GetComponent<Tutorial>();
        TutorialObject.gameObject.SetActive(false);
		if (!CountDownText) {
			Debug.LogError ("GameManager::Awake() > Are you sure there is a countdownobject called CountDown in the scene?");
		}
	}
	void OnLevelWasLoaded()
	{
		Debug.Log("OnLevelWasLoaded");
//		if (PlayerPrefs.GetInt("LevelRestart") == 1) {
//			GameState = GameStates.SetupGame;
//			Cam.introScript.enabled = false;
//			Cam.transform.position = Cam.introScript.Target;
//		}
	}
	void  OnApplicationQuit() {
		PlayerPrefs.SetInt("LevelRestart", 0);
	}
	

	void StartSetup ()
	{
        TutorialObject.gameObject.SetActive(true);


		TextAsset _names = (TextAsset)Resources.Load ("Names", typeof(TextAsset));
		_namesArr = new List<string> (_names.text.Split ('\n'));
		_namesArr.Sort ();
		_namesArr.RemoveRange (0, _namesArr.FindLastIndex (FindEmpty) + 1);

		// Generate combinations
		_combinations = new List<PlayerCombination> ();
		for (int c = 0; c < 26; ++c) {
			HSBColor col = new HSBColor((float)c / 26.0f, 110.0f/256.0f, 170.0f/256.0f);
			for (int i = 0; i < MeshAmount; ++i) {
				_combinations.Add (new PlayerCombination (col, i));
			}
		}
	}

	void StartCountdown ()
	{
		Cam.followScript.TargetArr = PlayersArr;

		_namesArr.Clear ();
		_combinations.Clear ();
	}

	void StartEndGame ()
	{
		if (PlayersArr.Count == 1) {
			WinningPlayer = PlayersArr [0];
			MS.DispatchMessage (WinningPlayer.script.Name + " won.", .5f);
		} else if (PlayersArr.Count == 0) {
			MS.DispatchMessage ("Everyone died. Sucker.");
		} else {
			MS.DispatchMessage ("Woops, game ended too early.");
		}
	}
	
	static bool FindEmpty (string s)
	{
		if (s == "")
			return true;
		return false;
	}


	// Update is called once per frame
	void Update ()
	{
		// Timer
		_processTimer += Time.deltaTime;

		// Game behaviour processing
		switch (GameState) {
		case GameStates.Intro:
			ProcessIntro ();
			break;
		case GameStates.SetupGame:
			ProcessSetupGame ();
			break;
		case GameStates.CountDown:
			ProcessCountDown ();
			break;
		case GameStates.InGame:
			break;
		case GameStates.EndGame:
			break;
		default:
			Debug.Log ("Unknown GameState");
			break;
		}
		if (GameState == GameStates.InGame){
			if (Input.GetKeyDown(KeyCode.Escape)) {
                foreach (var item in PlayersArr)
                {
                    Destroy(item.gameObject);
                }
                PlayersArr.Clear();
                GameState = GameStates.SetupGame;
                TutorialObject.gameObject.SetActive(true);
                Cam.gameObject.GetComponent<DistanceCounter>().Reset();
                GameObject.Find("MineSpawner").GetComponent<MineSpawner>().Clear();

            }
		}
        if (GameState == GameStates.EndGame)
        {
            if (Input.GetButtonDown("Submit"))
            {
                foreach (var item in PlayersArr)
                {
                    Destroy(item.gameObject);
                }
                PlayersArr.Clear();
                GameState = GameStates.SetupGame;
                TutorialObject.gameObject.SetActive(true);
                Cam.gameObject.GetComponent<DistanceCounter>().Reset();
                GameObject.Find("MineSpawner").GetComponent<MineSpawner>().Clear();

            }
        }

	}

	void ProcessIntro ()
	{
	}

	void ProcessSetupGame ()
	{
		if (Input.GetKeyDown (KeyCode.Return) && PlayersArr.Count >= 2) {
			_processTimer = 0;
			CountDownText.StartCountDown (CountdownTime);
			GameState = GameStates.CountDown;
            TutorialObject.gameObject.SetActive(false);
		}
	}

	void ProcessCountDown ()
	{
		if (_processTimer > CountdownTime) {
            Cam.gameObject.GetComponent<DistanceCounter>().Reset();
            Cam.gameObject.GetComponent<DistanceCounter>().enabled = true;
			GameState = GameStates.InGame;
		}
	}

	/* PLAYERS */
	public void RemovePlayer (GameObject p)
	{
		if (PlayersArr == null || PlayersArr.Count <= 0)
			return;

		foreach (PlayerInfo pi in PlayersArr) {
			if (pi.gameObject == p) {
				PlayersArr.Remove (pi);
				return;
			}
		}

		if (GameState == GameStates.InGame && PlayersArr.Count <= 1)
			GameState = GameStates.EndGame;
	}


	/* REGISTER KEYS AND SPAWN PLAYERS */
	void OnGUI ()
	{
		Event e = Event.current;
		// Receive input events to generate new players when setting up game
		if (e.isKey && e.keyCode != KeyCode.None && e.type == EventType.KeyDown && GameState == GameStates.SetupGame && !_buttonDown) {
			KeyCode key = e.keyCode;
			if (IsKeyUsable (key)) {
//                Debug.Log("Spawn");
				SpawnPlayer (key);
			}
		}
	}

	bool IsKeyUsable (KeyCode key)
	{
		if (CheckKey (key)) {
			foreach (PlayerInfo pi in PlayersArr) {
				if (pi.script.Key == key) {
					return false;
				}
			}
			return true;
		}
		return false;
	}

	bool CheckKey (KeyCode key)
	{
		//		Debug.Log(key);
		foreach (KeyCode k in _validCodesArr) {
			if (k == key)
				return true;
		}
		return false;
	}

	void SpawnPlayer (KeyCode key)
	{
		if (_combinations.Count <= 0) {
			Debug.Log ("Add more appearance combinations.");
			return;
		}
		GameObject obj = Instantiate (PlayerPrefab) as GameObject;
		PlayerInfo newPlayer = new PlayerInfo (obj);
		newPlayer.script.Key = key;
		// Random name
		int start = _namesArr.FindIndex (x => x.StartsWith (key.ToString ()));
		int end = _namesArr.FindLastIndex (x => x.StartsWith (key.ToString ()));
		newPlayer.script.Name = _namesArr [Random.Range (start, end)];
		newPlayer.gameObject.name = "Player " + key.ToString ();
		// Random appearance
		PlayerCombination newCombination = _combinations [Random.Range (0, _combinations.Count)];
		_combinations.Remove (newCombination);
		newPlayer.script.AppearanceColor = newCombination.color.ToColor();
		newPlayer.script.AppearanceMesh = newCombination.mesh;


		newPlayer.script.SwimmingHeight = -3;
		float randomHeight = Random.Range(-1.0f, -5.0f);
		float randomZ = Random.Range (-.2f, .2f);
		newPlayer.transform.position = new Vector3 (0, randomHeight, Cam.gameObject.transform.position.z + randomZ);
		newPlayer.gameObject.SetActive (true);
		PlayersArr.Add (newPlayer);

		string message = newPlayer.script.Name + " locked in.";
		MessageSystem.Instance.DispatchMessage (message);
	}

	public void SetCamera (GameObject cam)
	{
		Cam = new CameraInfo (cam);
	}

}
