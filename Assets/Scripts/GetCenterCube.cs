using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetCenterCube : MonoBehaviour
{
	StopFalling centerCubeStopFalling;

	void Awake()
	{
		centerCubeStopFalling = GetComponent<StopFalling>();
	}

	public StopFalling getCenterCubeStopFalling()
	{
		return centerCubeStopFalling;
	}
}