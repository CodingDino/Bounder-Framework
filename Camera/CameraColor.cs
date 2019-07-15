// ************************************************************************ 
// File Name:   CameraColor.cs 
// Purpose:    	Change the color of the camera.
// Project:		Bounder Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************ 
using UnityEngine;
using System.Collections;
#endregion
// ************************************************************************


// ************************************************************************
namespace Bounder.Framework 
{ 

// ************************************************************************ 
#region Class: CameraColor
// ************************************************************************
public class ChangeCameraColorEvent : GameEvent
{
	public Color color = Color.white;
	public float duration;
}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: CameraColor
// ************************************************************************
[RequireComponent(typeof(Camera))]
public class CameraColor : MonoBehaviour 
{
	
	// ********************************************************************
	#region Private Data Members
	// ********************************************************************
	private Camera thisCamera;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake () 
	{
		thisCamera = GetComponent<Camera>();
	}
	// ********************************************************************
	void OnEnable()
	{
		Events.AddListener<ChangeCameraColorEvent>(HandleColorChangeEvent);
	}
	// ********************************************************************
	void OnDisable()
	{
		Events.RemoveListener<ChangeCameraColorEvent>(HandleColorChangeEvent);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
	
	
	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public void ChangeColor (Color _newColor, float _transitionTime = 0) 
	{
		if (thisCamera == null)
		{
			Debug.LogError("CameraColor - attempt to change camera before initialization.");
			return;
		}
		StartCoroutine(ChangeColor_CR(_newColor, _transitionTime));
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods
	// ********************************************************************
	private IEnumerator ChangeColor_CR (Color _newColor, float _transitionTime) 
	{
		float startTime = Time.time;
		Color originalColor = thisCamera.backgroundColor;
		while (Time.time < startTime + _transitionTime)
		{
			float timePassed = Time.time - startTime;
			thisCamera.backgroundColor = Color.Lerp(originalColor,_newColor,timePassed/_transitionTime);
			yield return null;
		}
		thisCamera.backgroundColor = _newColor;
	}
	// ********************************************************************
	private void HandleColorChangeEvent(ChangeCameraColorEvent _event)
	{
		ChangeColor(_event.color, _event.duration);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}
// ************************************************************************
