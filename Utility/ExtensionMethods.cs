// ************************************************************************ 
// File Name:   ExtensionMethods.cs 
// Purpose:    	Loads in the train from player data
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System;
using System.Collections;


// ************************************************************************ 
// Class: ExtensionMethods
// ************************************************************************ 
public static class ExtensionMethods {
	
	
	// ********************************************************************
	// Function:	ResetTransform()
	// Purpose:		Sets the transform to it's default state
	// ********************************************************************
	public static void ResetTransform(this Transform _trans)
	{
		_trans.position = Vector3.zero;
		_trans.localRotation = Quaternion.identity;
		_trans.localScale = new Vector3(1, 1, 1);
	}


	// ********************************************************************
	// Function:	DestroyChildren()
	// Purpose:		Destroys all the transform's children
	// ********************************************************************
	public static void DestroyChildren(this Transform _trans)
	{
		for (int i = 0; i < _trans.childCount; ++i)
		{
			GameObject.Destroy (_trans.GetChild(i).gameObject);
		}
	}


	// ********************************************************************
	// Function:	SetPosX(), SetPosY(), SetPosZ()
	// Purpose:		Sets just the position x, y, or z value
	// ********************************************************************
	public static void SetPosX(this Transform _trans, float _pos)
	{
		_trans.position = new Vector3(_pos,
		                              _trans.position.y,
		                              _trans.position.z);
	}
	public static void SetPosY(this Transform _trans, float _pos)
	{
		_trans.position = new Vector3(_trans.position.x,
		                              _pos,
		                              _trans.position.z);
	}
	public static void SetPosZ(this Transform _trans, float _pos)
	{
		_trans.position = new Vector3(_trans.position.x,
		                              _trans.position.y,
		                              _pos);
	}
	
	
	// ********************************************************************
	// Function:	SetLocalPosX(), SetLocalPosY(), SetLocalPosZ()
	// Purpose:		Sets just the local position x, y, or z value
	// ********************************************************************
	public static void SetLocalPosX(this Transform _trans, float _pos)
	{
		_trans.localPosition = new Vector3(_pos,
		                                   _trans.localPosition.y,
		                                   _trans.localPosition.z);
	}
	public static void SetLocalPosY(this Transform _trans, float _pos)
	{
		_trans.localPosition = new Vector3(_trans.localPosition.x,
		                                   _pos,
		                                   _trans.localPosition.z);
	}
	public static void SetLocalPosZ(this Transform _trans, float _pos)
	{
		_trans.localPosition = new Vector3(_trans.localPosition.x,
		                                   _trans.localPosition.y,
		                                   _pos);
	}


	// ********************************************************************
	// Function:	CompleteCoroutineImmediately()
	// Purpose:		Completes a coroutine immediately
	// ********************************************************************
	public static void CompleteCoroutineImmediately(IEnumerator _coroutine)
	{
		bool finished = false;
		while(!finished)
			finished = !_coroutine.MoveNext();
	}


	// ********************************************************************
	// Function:	NullOrEmpty()
	// Purpose:		Check if a string is either null or empty.
	// ********************************************************************
	public static bool NullOrEmpty(this string _string)
	{
		return String.IsNullOrEmpty(_string);
	}

	// ********************************************************************
	// Function:	WorldToCanvasPoint()
	// Purpose:		Transforms a world point to the canvas space
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
	// Function:	ScreenToCanvasPoint()
	// Purpose:		Transforms a screen point to the canvas space
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
	// Function:	ToHex()
	// Purpose:		Outputs a Color as a hex string
	// ********************************************************************
	public static string ToHex(this Color _color)
	{
		string z = "#";
		string alpha = "0123456789ABCDEF";
		for (int i = 0; i < 3; ++i)
		{
			int dec = (int)(_color[i] * 255f);
			string a = ""+alpha[dec / 16];
			string b = ""+alpha[dec % 16];
			z += a + b;
		}
		return z;
	}


	// ********************************************************************
	// Function:	GetPath()
	// Purpose:		Returns object Hierarchy path as string
	// ********************************************************************
	public static string GetPath(this GameObject _object)
	{
		string path = _object.name;
		Transform transform = _object.transform;
		while (transform.parent != null)
		{
			transform = transform.parent;
			path = transform.name + "/" + path;
		}

		return path;
	}


	// ********************************************************************
	// Function:	HasParameterOfType()
	// Purpose:		Checks if the animator has the provided parameter
	// ********************************************************************
	public static bool HasParameterOfType(this Animator _self, string _name, AnimatorControllerParameterType _type)
	{
		var parameters = _self.parameters;
		for (int i = 0; i < parameters.Length; ++i)
		{
			if (parameters[i].type == _type && parameters[i].name == _name)
			{
				return true;
			}
		}
		return false;
	}


}
