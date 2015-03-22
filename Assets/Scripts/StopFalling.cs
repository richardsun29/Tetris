using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StopFalling : MonoBehaviour
{
	void Start()
	{
		gameObject.GetComponent<Collider>().enabled = false;
	}
	
	public bool blockedLeft()
	{
		Vector3 left = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
		return Physics.CheckSphere(left, .45f);
	}

	public bool blockedRight()
	{
		Vector3 right = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
		return Physics.CheckSphere(right, .45f);
	}

	public bool blockedDown()
	{
		Vector3 down = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
		return Physics.CheckSphere(down, .45f);
	}

	public bool inAnotherBlock()
	{
		return Physics.CheckSphere(transform.position, .45f);
	}

}





