using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatePiece : MonoBehaviour
{	
	public float fixingTime = 0.5f;
	bool extraTimeRunning = false;
	PieceMove pieceMove;
	Animator[] animators;


	void Start()
	{
		pieceMove = GetComponent<PieceMove>();
		animators = GetComponentsInChildren<Animator>();
		foreach(Animator animator in animators)
			animator.speed = 1 / fixingTime;
	}

	void Update()
	{
		if(!pieceMove.pieceBlockedDown())
			fade (false);
	}

	public void startFading()
	{
		if(pieceMove.enabled && !extraTimeRunning)
			StartCoroutine(extraTime());
	}

	IEnumerator extraTime()
	{
		extraTimeRunning = true;
		fade (true);
		//Debug.Log ("extraTime started");
		yield return new WaitForSeconds (fixingTime);
		if(pieceMove.pieceBlockedDown("check"))
		{
			fade (false);
			pieceMove.disablePieceMove ();
		}
		extraTimeRunning = false;
	}

	public void fade(bool b)
	{
		foreach (Animator anim in animators)
			anim.SetBool ("Fade", b);
	}








}