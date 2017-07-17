// ************************************************************************ 
// File Name:   AnimatorControllerParameterData.cs 
// Purpose:    	Information for applying a paramter to an animation
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: AnimatorControllerParameterData
// ************************************************************************ 
[System.Serializable]
public class AnimatorControllerParameterData
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[Tooltip("ID to be referenced in the calling animation event")]
	public string id;
	[Tooltip("Animator for the animation you want to activate.")]
	public Animator animator;
	[Tooltip("Parameter you want to change.")]
	public string parameter;
	[Tooltip("Parameter type.")]
	public AnimatorControllerParameterType parameterType;
	[Tooltip("Parameter value if bool")]
	public bool parameterValueBool;
	[Tooltip("Parameter value if int")]
	public int parameterValueInt;
	[Tooltip("Parameter value if float")]
	public float parameterValueFloat;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void Apply()
	{
		switch(parameterType)
		{
		case AnimatorControllerParameterType.Trigger:
			animator.SetTrigger(parameter);
			break;
		case AnimatorControllerParameterType.Bool:
			animator.SetBool(parameter, parameterValueBool);
			break;
		case AnimatorControllerParameterType.Int:
			animator.SetInteger(parameter, parameterValueInt);
			break;
		case AnimatorControllerParameterType.Float:
			animator.SetFloat(parameter, parameterValueFloat);
			break;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
