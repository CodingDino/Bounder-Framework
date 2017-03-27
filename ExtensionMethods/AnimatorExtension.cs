// ************************************************************************ 
// File Name:   AnimatorExtension.cs 
// Purpose:    	Extends the Animator class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: AnimatorExtension
// ************************************************************************
public static class AnimatorExtension 
{
	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	public static bool HasParameterOfType(this Animator _self, string _name, AnimatorControllerParameterType _type)
	{
		var parameters = _self.parameters;
		for (int i = 0; i < parameters.Length; ++i)
		{
			if (parameters[i].type == _type && parameters[i].name == _name)
			{
				return true;
			}
		}
		return false;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
