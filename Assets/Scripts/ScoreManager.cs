using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public static int score;
	Animator anim;
	Text text;
	
	void Awake ()
	{
		anim = GetComponentInParent<Animator>();
		text = GetComponent <Text> ();
		score = 0;
	}

	float timer = 0f;
	void Update ()
	{
		if(anim.GetBool("Game Over") == true)
		{
			if(timer >= 1f)
				text.text = "Score: " + score;
			else
				timer += Time.deltaTime;
		}
		else
			text.text = "Score: \n" + score;
	}




}