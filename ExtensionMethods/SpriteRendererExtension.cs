// ************************************************************************ 
// File Name:   SpriteRendererExtension.cs 
// Purpose:    	Extends the Sprite class
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
#region Class: SpriteRendererExtension
// ************************************************************************
public static class SpriteRendererExtension 
{

	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	public static bool IsVisibleToCamera (this SpriteRenderer _sprite, Camera _camera) 
	{

		Vector3 minScreenPoint = new Vector3(0,0,_sprite.transform.position.z);
		Vector3 maxScreenPoint = new Vector3(Screen.width,Screen.height,_sprite.transform.position.z);
		Vector3 minWorldPoint = _camera.ScreenToWorldPoint(minScreenPoint);
		Vector3 maxWorldPoint = _camera.ScreenToWorldPoint(maxScreenPoint);

		return _sprite.bounds.max.x >= minWorldPoint.x
			&& _sprite.bounds.max.y >= minWorldPoint.y
			&& _sprite.bounds.min.x <= maxWorldPoint.x
			&& _sprite.bounds.min.y <= maxWorldPoint.y;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
