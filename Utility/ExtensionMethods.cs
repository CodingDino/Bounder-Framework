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
}
