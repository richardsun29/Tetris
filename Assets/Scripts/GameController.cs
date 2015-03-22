using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject[] rows;
	public Canvas canvas;
	public float gameOverDelay = 2f;
	ClearLine clearLine;
	SpawnPiece spawnpiece;
	Animator anim;
	ScoreManager scoreManager;
	float gameOverTimer = 0f;
	int currentRowNumber;
	bool m_gameOver = false;

	void Start()
	{
		clearLine = GetComponent<ClearLine>();
		spawnpiece = GetComponent<SpawnPiece>();
		anim = canvas.GetComponent<Animator>();
		scoreManager = GetComponent<ScoreManager>();
		spawnPiece();
	}

	void Update()
	{
		if(m_gameOver)
		{
			gameOverTimer += Time.deltaTime;
			if(Input.GetKeyDown(KeyCode.R) && gameOverTimer >= gameOverDelay)
				Application.LoadLevel(Application.loadedLevel);
		}
	}

	public GameObject getCubeParent(Transform childTrans)
	{
		int rowNumber = (int)(childTrans.position.y + 9.1); // flooring error check
		//Debug.Log (rowNumber);
		if(rowNumber < 20) // top out
		{
			currentRowNumber = rowNumber;
			return rows[rowNumber];
		}
		else 
			setGameOver ();
		return rows[20];
	}

	void setGameOver()
	{
		m_gameOver = true;
		anim.SetBool("Game Over", true);
	}

	public bool gameOver(){return m_gameOver;}
	public void spawnPiece(){spawnpiece.spawn ();}
	public void checkForClear(){clearLine.clear(currentRowNumber);}
	public void resetHoldTimer(){spawnpiece.setCanHoldAgain();}
}
