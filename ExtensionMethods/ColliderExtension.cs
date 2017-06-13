// ************************************************************************ 
// File Name:   ColliderExtension.cs 
// Purpose:    	Extends the Color class
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
#region Class: ColliderExtension
// ************************************************************************
public static class ColliderExtension 
{
	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static bool IsColliding(this Collider2D _collider)
	{
		Collider2D[] contacts = new Collider2D[1];
		_collider.GetContacts(contacts);
		return contacts != null && contacts.Length > 0;
	}
	// ********************************************************************
	public static Collider2D[] GetContacts(this Collider2D _collider, int _numContacts)
	{
		Collider2D[] contacts = new Collider2D[_numContacts];
		_collider.GetContacts(contacts);
		return contacts;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
