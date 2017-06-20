// ************************************************************************ 
// File Name:   PanelEnums.cs 
// Purpose:    	Enums used in the Panel system
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
#endregion
// ************************************************************************


// ************************************************************************ 
#region Enum: PanelState
// ************************************************************************
public enum PanelState
{
	HIDING		= 1<<0,	// 1
	HIDDEN 		= 1<<1,	// 2
	SHOWING		= 1<<2,	// 4
	SHOWN		= 1<<3,	// 8
	// ==========
	VISIBLE 	= SHOWING | SHOWN | HIDING,
	TRANSITION 	= SHOWING | HIDING,
	LEGAL		= HIDING | HIDDEN | SHOWING | SHOWN
}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Enum: PanelLimitOverride
// ************************************************************************
public enum PanelLimitOverride
{
	REPLACE = 0,	// Hide existing panel (but do not uninit) and show this one
	WAIT,			// Do not hide existing panel, but insert this one into the stack under it to be shown when it hides.
	ADD,			// Do not hide existing panel, but also show this one
	NONE			// Do not hide existing panel, do NOT show this one
}
#endregion
// ************************************************************************

