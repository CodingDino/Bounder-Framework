// ************************************************************************ 
// File Name:   RaiseAnimationEvent.cs 
// Purpose:    	Raise a GameEvent for animation events
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
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: Animation Events
// ************************************************************************
[System.Serializable]
public class RaisedAnimationEvent : GameEvent
{
	public string id;
	public string valueString;
	public int valueInt;
	public float valueFloat;

	public RaisedAnimationEvent(string _id)
	{
		id = _id;
	}
	public RaisedAnimationEvent(string _id, string _value)
	{
		id = _id;
		valueString = _value;
	}
	public RaisedAnimationEvent(string _id, int _value)
	{
		id = _id;
		valueInt = _value;
	}
	public RaisedAnimationEvent(string _id, float _value)
	{
		id = _id;
		valueFloat = _value;
	}
}
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: RaiseAnimationEvent
// ************************************************************************
public class RaiseAnimationEvent : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private RaisedAnimationEvent[] m_events;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void Callback (string _id) 
	{
		for (int i = 0; i < m_events.Length; ++i)
		{
			if (m_events[i].id == _id)
			{
				Events.Raise(m_events[i]);
				break;
			}
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
