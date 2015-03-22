using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnPiece : MonoBehaviour
{
	public GameObject[] pieces;
	public GameObject spawnPoint;
	public GameObject nextPreview;
	public GameObject holdPreview;

	int[] bag = new int[7];
	GameObject nextPiece;
	GameObject currentPiece;
	GameObject holdPiece;
	int currentPieceNumber;
	int holdPieceNumber = -1;
	bool canHoldAgain = true;
	Queue randomBag = new Queue();
	bool firstPiece = true;
	float delayTime = 0f;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.RightShift))
			hold();
	}

	void setNextBag()
	{
		int n = 0; // number of successful picks
		while (n < 7)
		{
			int rand = Random.Range (0, pieces.Length);
			if(firstPiece && pieces.Length >= 4)
			{
				rand = Random.Range (0, 4);
				firstPiece = false;
			}

			bool copy = false;
			for(int k = 0; k < n; k++)
				if(bag[k] == rand){
					copy = true;
					break;}
			if(copy) continue;

			bag[n] = rand;
			n++;
			randomBag.Enqueue(rand);
		}
	}

	IEnumerator spawnPiece()
	{
		if(randomBag.Count < 7)
			setNextBag();
		yield return new WaitForSeconds(delayTime);
		delayTime = 0f;

		int random = (int)randomBag.Dequeue();
		
		Vector3 spawnPos = spawnPoint.transform.position;
		Quaternion spawnRot = spawnPoint.transform.rotation;
		adjustSpawn(ref spawnPos, random);

		currentPiece = (GameObject)Instantiate (pieces[random], spawnPos, spawnRot);
		currentPieceNumber = random;
		displayNext();
	}

	void adjustSpawn(ref Vector3 pos, int n)
	{
		switch(pieces[n].name[0])
		{
		case 'S': 
			pos.y += 1; 
			break;
		case 'I':
			pos.x += .5f;
			pos.y += .5f;
			break;
		case 'O':
			pos.x += .5f;
			pos.y += .5f;
			break;
		}
	}

	void adjustPreview(ref Vector3 pos, int n)
	{
		switch(pieces[n].name[0])
		{
		case 'S': 
			pos.y += 1f; 
			break;
		case 'I':
			pos.y += 1f;
			break;
		case 'O':
			pos.y += .5f;
			break;
		}
	}

	void displayNext()
	{
		Destroy(nextPiece);
		Vector3 nextPos = nextPreview.transform.position;
		Quaternion nextRot = nextPreview.transform.rotation;
		adjustPreview(ref nextPos, (int)randomBag.Peek());
		nextPiece = (GameObject)Instantiate(pieces[(int)randomBag.Peek()], nextPos, nextRot);
	}

	void hold()
	{	
		if(!canHoldAgain)
			return;
		Destroy(holdPiece);
		int lastHoldNumber = holdPieceNumber;
		holdPieceNumber = currentPieceNumber;
		// Adjust hold preview for shapes
		Vector3 holdPos = holdPreview.transform.position;
		Quaternion holdRot = holdPreview.transform.rotation;
		adjustPreview(ref holdPos, currentPieceNumber);

		holdPiece = (GameObject)Instantiate(pieces[currentPieceNumber], holdPos, holdRot);
		Destroy(currentPiece);

		// Spawn new piece
		if(lastHoldNumber == -1) // first piece in hold
			spawn();
		else
		{
			Vector3 spawnPos = spawnPoint.transform.position;
			Quaternion spawnRot = spawnPoint.transform.rotation;
			adjustSpawn(ref spawnPos, lastHoldNumber);
			
			currentPiece = (GameObject)Instantiate (pieces[lastHoldNumber], spawnPos, spawnRot);
			currentPieceNumber = lastHoldNumber;
			displayNext();
		}
		canHoldAgain = false;
	}

	public void delaySpawn(float delay){delayTime = delay;}
	public void spawn(){StartCoroutine(spawnPiece ());}
	public void setCanHoldAgain() {canHoldAgain = true;}
}