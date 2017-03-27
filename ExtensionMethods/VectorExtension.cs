// ************************************************************************ 
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
		float magnitude = _this.magnitude;

		_this.x = Random.Range(-1.0f,1.0f);
		_this.y = Random.Range(-1.0f,1.0f);

		_this.Normalize();

		_this *= magnitude;

		return _this;
	}
	// ********************************************************************
	public static Vector3 Randomise(this Vector3 _this)
	{
		float magnitude = _this.magnitude;

		_this.x = Random.Range(-1.0f,1.0f);
		_this.y = Random.Range(-1.0f,1.0f);
		_this.z = Random.Range(-1.0f,1.0f);

		_this.Normalize();

		_this *= magnitude;

		return _this;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
