using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClearLine : MonoBehaviour
{
	GameController gameController;
	public float clearLineFlash = .5f;
	GameObject[] rows;
	
	int[] rowsToClear;
	int rowN = 0;
	bool clearLineRunning = false;
	SpawnPiece spawnPiece;

	void Start()
	{
		spawnPiece = GetComponent<SpawnPiece>();
		gameController = GetComponent<GameController>();
		rows = gameController.rows;
		rowsToClear = new int[4];
		for(int k = 0; k < 4; k++)
			rowsToClear[k] = -1;
	}

	public void clear(int rowNumber)
	{
		if(rows[rowNumber].transform.childCount != 10) // only clear line if full
			return;

		GetMaterial[] getMaterials = rows[rowNumber].GetComponentsInChildren<GetMaterial>();
		// Clear line
		for(int k = 0; k < 10; k++) 
			Destroy(getMaterials[k].getCube ());
		rowsToClear[rowN] = rowNumber;
		rowN++;
		if(!clearLineRunning)
			StartCoroutine (clearLine (rowNumber));
	}
	
	IEnumerator clearLine(int rowNumber)
	{
		clearLineRunning = true;
		spawnPiece.delaySpawn(clearLineFlash);
		yield return new WaitForSeconds(clearLineFlash);
		
		// Shift cubes down
		sortRowsToClear();
		int nLines = 0;
		for(int k = 0; k < 4; k++)
		{
			if(rowsToClear[k] == -1)
				continue;
			moveCubesDown(rowsToClear[k]);
			rowsToClear[k] = -1;
			nLines++;
		}
		addScore(nLines);
		// Reset counters
		rowN = 0;
		clearLineRunning = false;
//		Debug.Log ("line cleared");
	}
	
	void moveCubesDown(int rowNumber)
	{
		for(int k = rowNumber + 1; k < 20; k++) 
		{
			GetMaterial[] materials = rows[k].GetComponentsInChildren<GetMaterial>();
			for(int j = 0; j < materials.Length; j++)
			{
				GameObject cube = materials[j].getCube ();
				cube.transform.Translate (Vector3.down, Space.World);
				cube.transform.SetParent(gameController.getCubeParent(cube.transform).transform, true);
			}
		}
	}
	
	void sortRowsToClear()
	{
		for (;;)
		{
			bool flag = true;
			for (int k = 0; k < 3; k++)
			{
				if (rowsToClear[k] < rowsToClear[k + 1])
				{
					int temp = rowsToClear[k];
					rowsToClear[k] = rowsToClear[k + 1];
					rowsToClear[k + 1] = temp;
					flag = false;
				}
			}
			if (flag) break;
		}
	}

	void addScore(int nLines)
	{
		switch(nLines)
		{
		case 1: ScoreManager.score += 100; break;
		case 2: ScoreManager.score += 300; break;
		case 3: ScoreManager.score += 500; break;
		case 4: ScoreManager.score += 800; break;
		}
	}
	

}