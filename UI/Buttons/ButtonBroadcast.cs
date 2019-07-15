// ************************************************************************ 
// File Name:   ButtonBroadcast.cs 
// Purpose:    	Sends out button press event
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ButtonPressEvent
// ************************************************************************
public class ButtonPressEvent : GameEvent 
{
	public string buttonName;
	public ButtonPressEvent(string _buttonName)
	{
		buttonName = _buttonName;
	}
}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ButtonBroadcast
// ************************************************************************
public class ButtonBroadcast : MonoBehaviour 
{
	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ButtonPressed()
	{
		Events.Raise( new ButtonPressEvent(name) );
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************
