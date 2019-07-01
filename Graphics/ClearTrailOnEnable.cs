// ************************************************************************ 
// File Name:   ClearTrailOnEnable.cs 
// Purpose:     Clears a trail renderer on enable
// Project:		Bounder Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ClearTrailOnEnable
// ************************************************************************ 
[RequireComponent(typeof(TrailRenderer))]
public class ClearTrailOnEnable : MonoBehaviour
{
	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void OnEnable()
	{
		StartCoroutine(TrailReset());
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private IEnumerator TrailReset()
	{
		TrailRenderer trail = GetComponent<TrailRenderer>(); 
		trail.Clear();

		// Hack to make sure the trail doesn't register being teleported 
		// to the object's new location
		trail.enabled = false;
		yield return null;
		trail.enabled = true;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************