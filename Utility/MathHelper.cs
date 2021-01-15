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
#region Enums 
// ************************************************************************
public enum ComparisonOperation
{
    EQUAL,
    NOT_EQUAL,
    LESS_THAN,
    LESS_THAN_EQUAL_TO,
    GREATER_THAN,
    GREATER_THAN_EQUAL_TO
}
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
    public static bool RunComparison(ComparisonOperation _comparison, float _first, float _second)
    {
        switch (_comparison)
        {
            case ComparisonOperation.EQUAL:
                return _first == _second;
            case ComparisonOperation.NOT_EQUAL:
                return _first != _second;
            case ComparisonOperation.GREATER_THAN:
                return _first > _second;
            case ComparisonOperation.GREATER_THAN_EQUAL_TO:
                return _first >= _second;
            case ComparisonOperation.LESS_THAN:
                return _first < _second;
            case ComparisonOperation.LESS_THAN_EQUAL_TO:
                return _first <= _second;
            default:
                Debug.LogError("Invalid comparison operation provided: " + _comparison);
                return false;
        }
    }
    // ********************************************************************
    #endregion
    // ********************************************************************

}
#endregion
// ************************************************************************
