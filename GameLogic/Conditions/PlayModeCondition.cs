// ************************************************************************ 
// File Name:   PlayModeCondition.cs 
// Purpose:    	A Condition for checking the current play mode
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2019 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: PlayModeCondition
// ************************************************************************
[CreateAssetMenu(fileName = "PlayModeCondition", menuName = "Bounder/Conditions/PlayModeCondition", order = 1)]
public class PlayModeCondition : Condition 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
		// TODO: target play mode
	[SerializeField]
	private bool m_invert = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Functions 
	// ********************************************************************
	public override void Register(bool _register) 
	{
		// TODO: register for play mode changes
	}
	// ********************************************************************
	public override bool Evaluate() 
	{
		return true; // TODO: Check play mode vs target
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************
