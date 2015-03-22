using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetMaterial : MonoBehaviour
{
	Material material;
	Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
		material = GetComponent<Renderer>().material;
		animator.SetBool (materialName(), true);
	}

	string materialName()
	{
		string name = "";
		for(int k = 0; k < material.name.Length; k++) // "[Color] (Instance)" ==> "[Color]"
		{
			if(char.IsLetter (material.name[k]))
				name += material.name[k];
			else break;
		}
		return name;
	}

	public GameObject getCube()
	{
		return gameObject;
	}








}