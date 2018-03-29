// ************************************************************************ 
// File Name:   ControlSchemeCondition.cs 
// Purpose:    	A Condition for checking the current control scheme
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
#region Class: ControlSchemeCondition
// ************************************************************************
[CreateAssetMenu(fileName = "ControlSchemeCondition", menuName = "Bounder/Conditions/ControlSchemeCondition", order = 1)]
public class ControlSchemeCondition : Condition 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[SerializeField]
	private ControlScheme m_controlScheme;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Condition Functions 
	// ********************************************************************
	public override void Register(bool _register) 
	{
		// TODO: Subscribe to events when scheme is changed
	}
	// ********************************************************************
	public override int GetProgress_Cumulative() 
	{
		bool schemeMatch = m_controlScheme.Contains(InputManager.controlScheme);
		return schemeMatch ? 1 : 0;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
