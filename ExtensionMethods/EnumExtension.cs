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
		ulong keysVal = Convert.ToUInt64(keys);
		ulong flagVal = Convert.ToUInt64(flag);

		return (keysVal & flagVal) == flagVal;
	}
	// ********************************************************************
	public static ulong AddFlag(this Enum keys, Enum flag)
	{
		ulong keysVal = Convert.ToUInt64(keys);
		ulong flagVal = Convert.ToUInt64(flag);

		return (keysVal | flagVal);
	}
	// ********************************************************************
	public static ulong RemoveFlag(this Enum keys, Enum flag)
	{
		ulong keysVal = Convert.ToUInt64(keys);
		ulong flagVal = Convert.ToUInt64(flag);

		return (keysVal & (~flagVal));
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
