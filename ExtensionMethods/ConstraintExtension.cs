// ************************************************************************ 
// File Name:   ConstraintExtension.cs 
// Purpose:    	Extends the various contraint classes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2019 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using UnityEngine.Animations;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ConstraintExtension
// ************************************************************************
public static class ConstraintExtension 
{
	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static void ClearSources(this ParentConstraint _constraint)
	{
		for (int i = _constraint.sourceCount - 1; i >= 0; --i)
		{
			_constraint.RemoveSource(i);
		}
	}
	// ********************************************************************
	public static void AddSourceTransform(this ParentConstraint _constraint, Transform _transform)
	{
		if (_transform != null)
		{
			ConstraintSource source = new ConstraintSource();
			source.sourceTransform = _transform;
			source.weight = 100;
			_constraint.AddSource(source);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
