// ************************************************************************ 
// File Name:   GameObjectExtension.cs 
// Purpose:    	Extends the GameObject class
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
#region Class: GameObjectExtension
// ************************************************************************
public static class GameObjectExtension 
{
	// ********************************************************************
	#region Extension Methods 
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
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
