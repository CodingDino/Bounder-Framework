// ************************************************************************ 
// File Name:   CameraColor.cs 
// Purpose:    	Change the color of the camera.
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 
[RequireComponent(typeof(Camera))]


// ************************************************************************ 
// Class: CameraColor
// ************************************************************************
public class CameraColor : MonoBehaviour {
	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Camera thisCamera;


	// ********************************************************************
	// Function:	Awake()
	// Purpose:		Called when the script instance is being loaded
	// ********************************************************************
	void Awake () 
	{
		thisCamera = GetComponent<Camera>();
	}
	
	
	// ********************************************************************
	// Function:	ChangeColor()
	// Purpose:		Changes the color over time.
	// ********************************************************************
	public void ChangeColor (Color _newColor, float _transitionTime = 0) 
	{
		if (thisCamera == null)
		{
			Debug.LogError("CameraColor - attempt to change camera before initialization.");
			return;
		}

		if (_transitionTime == 0)
		{
			thisCamera.backgroundColor = _newColor;
		}
		else
		{
			StartCoroutine(ChangeColorOverTime(_newColor, _transitionTime));
		}
	}
	
	
	// ********************************************************************
	// Function:	ChangeColor()
	// Purpose:		Changes the color over time.
	// ********************************************************************
	public IEnumerator ChangeColorOverTime (Color _newColor, float _transitionTime) 
	{
		if (thisCamera == null)
		{
			Debug.LogError("CameraColor - attempt to change camera before initialization.");
			yield break;
		}

		float startTime = Time.time;
		Color originalColor = thisCamera.backgroundColor;
		Color colorDiff = _newColor - originalColor;
		while (Time.time < startTime + _transitionTime)
		{
			float timePassed = Time.time - startTime;
			Color colorChange = colorDiff * (timePassed / _transitionTime);
			thisCamera.backgroundColor = originalColor + colorChange;
			yield return null;
		}
		thisCamera.backgroundColor = _newColor;

	}
}
