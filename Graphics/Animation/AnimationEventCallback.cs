// ************************************************************************ 
// File Name:   SendEventOnAnimationEvent.cs 
// Purpose:    	
// Project:		
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
#region Class: SendEventOnAnimationEvent
// ************************************************************************
public class AnimationEventCallback : MonoBehaviour 
{

	// ********************************************************************
	#region Delegates 
	// ********************************************************************
	public delegate void AnimationEvent();
	public event AnimationEvent OnAnimationEvent;
	public delegate void AnimationEventString(string _value);
	public event AnimationEventString OnAnimationEventString;
	public delegate void AnimationEventInt(int _value);
	public event AnimationEventInt OnAnimationEventInt;
	public delegate void AnimationEventFloat(float _value);
	public event AnimationEventFloat OnAnimationEventFloat;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void Callback () 
	{
		if (OnAnimationEvent != null)
			OnAnimationEvent();
	}
	// ********************************************************************
	private void StringCallback (string _value) 
	{
		if (OnAnimationEventString != null)
			OnAnimationEventString(_value);
	}
	// ********************************************************************
	private void IntCallback (int _value) 
	{
		if (OnAnimationEventInt != null)
			OnAnimationEventInt(_value);
	}
	// ********************************************************************
	private void FloatCallback (float _value) 
	{
		if (OnAnimationEventFloat != null)
			OnAnimationEventFloat(_value);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
