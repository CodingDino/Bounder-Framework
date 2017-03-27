// ************************************************************************ 
// File Name:   CanvasExtension.cs 
// Purpose:    	Extends the Canvas class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: CanvasExtension
// ************************************************************************
public static class CanvasExtension 
{
	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static Vector2 WorldToCanvasPoint(this Canvas _canvas, Camera _camera, Vector3 _worldPoint)
	{
		RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

		//Vector position (percentage from 0 to 1) considering camera size.
		//For example (0,0) is lower left, middle is (0.5,0.5)
		Vector2 canvasPoint = _camera.WorldToViewportPoint(_worldPoint);

		//Calculate position considering our percentage, using our canvas size
		//So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
		canvasPoint.x *= canvasRect.sizeDelta.x;
		canvasPoint.y *= canvasRect.sizeDelta.y;

		//The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
		//But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
		//We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
		//returned value will still be correct.
		canvasPoint.x -= canvasRect.sizeDelta.x * canvasRect.pivot.x;
		canvasPoint.y -= canvasRect.sizeDelta.y * canvasRect.pivot.y;

		return canvasPoint;
	}
	// ********************************************************************
	public static Vector2 ScreenToCanvasPoint(this Canvas _canvas, Camera _camera, Vector2 _screenPoint)
	{
		RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

		//Vector position (percentage from 0 to 1) considering camera size.
		//For example (0,0) is lower left, middle is (0.5,0.5)
		Vector2 canvasPoint = _camera.ScreenToViewportPoint(_screenPoint);

		//Calculate position considering our percentage, using our canvas size
		//So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
		canvasPoint.x *= canvasRect.sizeDelta.x;
		canvasPoint.y *= canvasRect.sizeDelta.y;

		//The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
		//But in reality its middle (0.5,0.5) by default, so we remove the amount considering canvas rectransform pivot.
		//We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
		//returned value will still be correct.
		canvasPoint.x -= canvasRect.sizeDelta.x * canvasRect.pivot.x;
		canvasPoint.y -= canvasRect.sizeDelta.y * canvasRect.pivot.y;

		return canvasPoint;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
