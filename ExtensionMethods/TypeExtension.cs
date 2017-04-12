// ************************************************************************ 
// File Name:   TypeExtension.cs 
// Purpose:    	Extends the Type class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: TypeExtension
// ************************************************************************
public static class TypeExtension 
{

	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static Type[] GetSubTypes(this Type _type)
	{
		Type[] allTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
		Type[] validTypes = (from System.Type type in allTypes where type.IsSubclassOf(_type) select type).ToArray();
		return validTypes;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
