// ************************************************************************ 
// File Name:   EnumExtension.cs 
// Purpose:    	Extends the Enum class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: EnumExtension
// ************************************************************************
public static class EnumExtension 
{
	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	public static bool Contains(this Enum keys, Enum flag)
	{
        long keysVal = Convert.ToInt64(keys);
		long flagVal = Convert.ToInt64(flag);

		return (keysVal & flagVal) == flagVal;
	}
	// ********************************************************************
	public static long AddFlag(this Enum keys, Enum flag)
	{
		long keysVal = Convert.ToInt64(keys);
		long flagVal = Convert.ToInt64(flag);

		return (keysVal | flagVal);
	}
	// ********************************************************************
	public static long RemoveFlag(this Enum keys, Enum flag)
	{
		long keysVal = Convert.ToInt64(keys);
		long flagVal = Convert.ToInt64(flag);

		return (keysVal & (~flagVal));
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
