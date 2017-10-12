// ************************************************************************ 
// File Name:   MathHelper.cs 
// Purpose:    	Helper functions for Math
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
#region Class: MathHelper
// ************************************************************************
public static class MathHelper 
{
	// ********************************************************************
	#region Helper Methods 
	// ********************************************************************
	public static int Cycle(int _value, int _lowerLimit, int _upperLimit)
	{
		if (_value >= _upperLimit)
		{
			return _value - _upperLimit;
		}
		if (_value < _lowerLimit)
		{
			return _value + _upperLimit;
		}
		return _value;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
