using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class PieceMove : MonoBehaviour
{
	public float fallSpeed = 1;
	public float horizontalSpeed = 15f;
	public float downwardSpeed = 25f;
	public float rotateSpeed = 3f;

	float fallTimer = 0f;
	float timerHorizontal = 0f;
	float timerVertical = 0f;
	int rotationCounter = 0;
	bool m_blockedDown;
	bool m_blockedLeft;
	bool m_blockedRight;

	GameController gameController;
	StopFalling[] stopFalling;
	StopFalling centerCube;
	StopFalling orangeCenterCube2;
	AnimatePiece animatePiece;

	void Awake()
	{
		if(transform.position.x > 6f || transform.position.x < -6f) // in next or hold box
			this.enabled = false;
	}

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		stopFalling = GetComponentsInChildren<StopFalling>();
		animatePiece = GetComponent<AnimatePiece>();

		GetCenterCube[] getCenterCube = GetComponentsInChildren<GetCenterCube>();
		if(getCenterCube.Length != 0)
		{
			centerCube = getCenterCube[0].getCenterCubeStopFalling ();
			if(gameObject.name[0] == 'I') // Orange I
		 		orangeCenterCube2 = getCenterCube[1].getCenterCubeStopFalling ();
		}
	}

	void Update()
	{
		fall ();
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		horizontal(h);
		vertical(v);
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow))
			rotate (v);
		if(Input.GetKeyDown (KeyCode.Space))
			hardDrop();

		if(pieceBlockedDown())
			animatePiece.startFading ();
	}

	void fall()
	{
		fallTimer += Time.deltaTime;
		if(fallTimer >= 1 / fallSpeed)
		{
			if(pieceBlockedDown ())
				return;
			transform.Translate (Vector3.down, Space.World);
			fallTimer = 0f;
			checkBlocked();
		}
	}

	void hardDrop()
	{
		while(!pieceBlockedDown ())
		{
			fallTimer += 1/fallSpeed;
			fall ();
			ScoreManager.score += 2;
		}
		disablePieceMove ();
	}

	void horizontal(float h)
	{
		timerHorizontal += Time.deltaTime;
		if (h == 0f) // Press buttons quickly
			timerHorizontal += horizontalSpeed;

		// Prevent moving into blocks
		if(h == -1f && pieceBlockedLeft())
			h = 0f; 
		if(h == 1f && pieceBlockedRight ())
			h = 0f;

		if(h != 0 && timerHorizontal >= 1 / horizontalSpeed)
		{
			transform.Translate(h, 0, 0, Space.World);
			timerHorizontal = 0f;
			checkBlocked();
		}
	}

	void vertical(float v)
	{
		timerVertical += Time.deltaTime;

		if (v == 0f)
			timerVertical += downwardSpeed;

		if(pieceBlockedDown())
			return;

		if(v == -1f && timerVertical >= 1 / downwardSpeed)
		{
			transform.Translate (Vector3.down, Space.World);
			timerVertical = 0f;
			checkBlocked ();
			ScoreManager.score += 1;
		}
	}


	void rotate(float v)
	{
		Vector3 oldPos = transform.position;

		rotationCounter++;
		transform.rotation = Quaternion.Euler (0, 0, -90f * rotationCounter);
		checkBlocked();
		rotateClamp();

		if(undo())
		{
			Debug.Log ("undo");
			rotationCounter--;
			transform.rotation = Quaternion.Euler (0, 0, -90f * rotationCounter);
			transform.position = oldPos;
			checkBlocked();
		}
	}

	void rotateClamp()
	{
		if(centerCube == null)
			return;

		int direction = 0;
		int up = 0;
		if(centerCube.blockedLeft())
			direction = 1;
		if(centerCube.blockedRight())
			direction = -1;
		if(centerCube.blockedDown ())
			up = 1;

		if(orangeCenterCube2 != null)
		{
			if(orangeCenterCube2.blockedLeft())
				direction = 1;
			if(orangeCenterCube2.blockedRight ())
				direction = -1;
			if(orangeCenterCube2.blockedDown ())
				up = 1;
		}

//		Debug.Log ("(" + direction + ", " + up + ")");
		transform.Translate (direction, up, 0, Space.World);
		if(undo () && gameObject.name[0] == 'I') // 'I' piece can move 2 spaces for clamp
			transform.Translate (direction, up, 0, Space.World);

		checkBlocked();
	}

	public void checkBlocked()
	{
		pieceBlockedDown("check");
		pieceBlockedLeft("check");
		pieceBlockedRight("check");
	}

	public bool pieceBlockedDown(string check = "")
	{
		if(check != "")
		{
			m_blockedDown = false;
			foreach(StopFalling sf in stopFalling)
				if(sf.blockedDown())
					m_blockedDown = true;
		}
		return m_blockedDown;
	}

	public bool pieceBlockedLeft(string check = "")
	{
		if(check != "")
		{
			m_blockedLeft = false;
			foreach(StopFalling sf in stopFalling)
				if(sf.blockedLeft())
					m_blockedLeft = true;
		}
		return m_blockedLeft;
	}

	public bool pieceBlockedRight(string check = "")
	{
		if(check != "")
		{
			m_blockedRight = false;
			foreach(StopFalling sf in stopFalling)
				if(sf.blockedRight())
					m_blockedRight = true;
		}
		return m_blockedRight;
	}

	public bool undo()
	{
		foreach(StopFalling sf in stopFalling)
			if(sf.inAnotherBlock ())
				return true;

		return false;
	}

	public void disablePieceMove()
	{
		while(gameObject.transform.childCount > 0) // Detach and organize cubes
		{
			Transform child = transform.GetChild (0);
			child.GetComponent<StopFalling>().enabled = false;
			child.tag = "PlacedPiece";
			child.GetComponent<Collider>().enabled = true;

			child.SetParent (gameController.getCubeParent(child.transform).transform, true);
			gameController.checkForClear();
		}

		if(gameController.gameOver () == false)
			gameController.spawnPiece();
		Destroy (gameObject);
		gameController.resetHoldTimer();
	}

}


