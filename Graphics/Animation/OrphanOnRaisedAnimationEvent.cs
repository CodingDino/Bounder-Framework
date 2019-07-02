// ************************************************************************ 
// File Name:   OrphanOnRaisedAnimationEvent.cs 
// Purpose:    	Separates object from parent on event
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: OrphanOnRaisedAnimationEvent
// ************************************************************************
public class OrphanOnRaisedAnimationEvent : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private string m_id = "";
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void OnEnable()
	{
		Events.AddListener<RaisedAnimationEvent>(Orphan);
	}
	// ********************************************************************
	void OnDisable()
	{
		Events.RemoveListener<RaisedAnimationEvent>(Orphan);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void Orphan (RaisedAnimationEvent _event) 
	{
		if (_event.id == m_id)
		{
			transform.SetParent(null, true);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
