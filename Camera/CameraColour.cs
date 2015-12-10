// ************************************************************************ 
// File Name:   CameraColour.cs 
// Purpose:    	Change the colour of the camera.
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
// Class: CameraColour
// ************************************************************************
public class CameraColour : MonoBehaviour {
	
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
	// Function:	ChangeColour()
	// Purpose:		Changes the colour over time.
	// ********************************************************************
	public void ChangeColour (Color _newColour, float _transitionTime = 0) 
	{
		if (thisCamera == null)
		{
			Debug.LogError("CameraColour - attempt to change camera before initialization.");
			return;
		}

		if (_transitionTime == 0)
		{
			thisCamera.backgroundColor = _newColour;
		}
		else
		{
			StartCoroutine(ChangeColourOverTime(_newColour, _transitionTime));
		}
	}
	
	
	// ********************************************************************
	// Function:	ChangeColour()
	// Purpose:		Changes the colour over time.
	// ********************************************************************
	public IEnumerator ChangeColourOverTime (Color _newColour, float _transitionTime) 
	{
		if (thisCamera == null)
		{
			Debug.LogError("CameraColour - attempt to change camera before initialization.");
			yield break;
		}

		float startTime = Time.time;
		Color originalColour = thisCamera.backgroundColor;
		Color colorDiff = _newColour - originalColour;
		while (Time.time < startTime + _transitionTime)
		{
			float timePassed = Time.time - startTime;
			Color colorChange = colorDiff * (timePassed / _transitionTime);
			thisCamera.backgroundColor = originalColour + colorChange;
			yield return null;
		}
		thisCamera.backgroundColor = _newColour;

	}
}
