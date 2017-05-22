// ************************************************************************ 
// File Name:   ListExtension.cs 
// Purpose:    	Extends the List class
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
#region Class: ListExtension
// ************************************************************************
public static class ListExtension 
{
	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static T Front<T>(this List<T> _self)
	{
		if (_self.Count > 0)
			return _self[0];
		else
			return default(T);
	}
	// ********************************************************************
	public static T Back<T>(this List<T> _self)
	{
		if (_self.Count > 0)
			return _self[_self.Count-1];
		else
			return default(T);
	}
	// ********************************************************************
	public static T Random<T>(this List<T> _self)
	{
		if (_self.Count > 0)
			return _self[Random.Range(0,_self.Count)];
		else
			return default(T);
	}
	// ********************************************************************
	public static void AddAtFront<T>(this List<T> _self, T _toAdd)
	{
		if (_self.Count > 0)
			_self.Insert(0,_toAdd);
		else
			_self.Add(_toAdd);
	}
	// ********************************************************************
	public static List<T> Copy<T>(this List<T> _self)
	{
		List<T> copy = new List<T>();
		for (int i = 0; i < _self.Count; ++i)
		{
			copy.Add(_self[i]);
		}
		return copy;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
