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
public class AnimationEventEmpty : GameEvent
{
	public string id;
}
// ************************************************************************
public class AnimationEventString : GameEvent
{
	public string id;
	public string value;
}
// ************************************************************************
public class AnimationEventInt : GameEvent
{
	public string id;
	public int value;
}
// ************************************************************************
public class AnimationEventFloat : GameEvent
{
	public string id;
	public float value;
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
	private string m_id;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void Callback () 
	{
		AnimationEventEmpty newEvent = new AnimationEventEmpty();
		newEvent.id = m_id;
		Events.Raise(newEvent);
	}
	// ********************************************************************
	private void StringCallback (string _value) 
	{
		AnimationEventString newEvent = new AnimationEventString();
		newEvent.id = m_id;
		newEvent.value = _value;
		Events.Raise(newEvent);
	}
	// ********************************************************************
	private void IntCallback (int _value) 
	{
		AnimationEventInt newEvent = new AnimationEventInt();
		newEvent.id = m_id;
		newEvent.value = _value;
		Events.Raise(newEvent);
	}
	// ********************************************************************
	private void FloatCallback (float _value) 
	{
		AnimationEventFloat newEvent = new AnimationEventFloat();
		newEvent.id = m_id;
		newEvent.value = _value;
		Events.Raise(newEvent);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
