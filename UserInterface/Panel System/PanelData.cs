﻿// ************************************************************************ 
// File Name:   PanelData.cs 
// Purpose:    	Data to be passed to a Panel
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
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: PanelData
// ************************************************************************
[System.Serializable]
public class PanelData 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public PanelState startingState = PanelState.HIDDEN;
	public PanelLimitOverride limitOverride = PanelLimitOverride.REPLACE;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Constructors 
	// ********************************************************************
	public PanelData(PanelState _startingState = PanelState.HIDDEN,
	                 PanelLimitOverride _limitOverride = PanelLimitOverride.REPLACE)
	{
		startingState = _startingState;
		limitOverride = _limitOverride;
	}
	// ********************************************************************
	public PanelData(JSON _JSON)
	{
		_JSON["startingState"].GetEnum<PanelState>(ref startingState);
		_JSON["limitOverride"].GetEnum<PanelLimitOverride>(ref limitOverride);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************