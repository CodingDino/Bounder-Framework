﻿// ************************************************************************ 
// File Name:   VectorExtension.cs 
// Purpose:    	Extends the Vector2 and Vector3 classes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: VectorExtension
// ************************************************************************
public static class VectorExtension 
{
	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static Vector2 Randomise(this Vector2 _this)
	{
		_this.x = Random.Range(-1.0f,1.0f);
		_this.y = Random.Range(-1.0f,1.0f);

		_this.Normalize();

		return _this;
	}
	// ********************************************************************
	public static Vector3 Randomise(this Vector3 _this)
	{
		_this.x = Random.Range(-1.0f,1.0f);
		_this.y = Random.Range(-1.0f,1.0f);
		_this.z = Random.Range(-1.0f,1.0f);

		_this.Normalize();

		return _this;
	}
	// ********************************************************************
	public static Vector2 Convolve(this Vector2 _this, Vector2 _other)
	{
		_this.x *= _other.x;
		_this.y *= _other.y;

		return _this;
	}
	// ********************************************************************
	public static Vector3 Convolve(this Vector3 _this, Vector3 _other)
	{
		_this.x *= _other.x;
		_this.y *= _other.y;
		_this.z *= _other.z;

		return _this;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
