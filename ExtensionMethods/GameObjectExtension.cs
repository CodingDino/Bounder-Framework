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
	public static void SetLayerRecursive(this GameObject _object, string _layerName)
	{
		SetLayerRecursive(_object, LayerMask.NameToLayer(_layerName));
	}
	// ********************************************************************
	public static void SetLayerRecursive(this GameObject _object, int _layer)
	{
		_object.layer = _layer;
		for (int i = 0; i < _object.transform.childCount; ++i)
		{
			_object.transform.GetChild(i).gameObject.SetLayerRecursive(_layer);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
