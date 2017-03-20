﻿// ************************************************************************ 
// File Name:   StartingAnimationParameters.cs 
// Purpose:    	
// Project:		
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
#region Class: StartingAnimationParameters
// ************************************************************************
public class StartingAnimationParameters : MonoBehaviour 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("Parameters you want to set.")]
	private List<AnimatorControllerParameterData> m_parameters = new List<AnimatorControllerParameterData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void Start () 
	{
		for (int i = 0; i < m_parameters.Count; ++i)
		{
			AnimatorControllerParameterData param = m_parameters[i];
			switch(param.parameterType)
			{
			case AnimatorControllerParameterType.Trigger:
				param.animator.SetTrigger(param.parameter);
				break;
			case AnimatorControllerParameterType.Bool:
				param.animator.SetBool(param.parameter, param.parameterValueBool);
				break;
			case AnimatorControllerParameterType.Int:
				param.animator.SetInteger(param.parameter, param.parameterValueInt);
				break;
			case AnimatorControllerParameterType.Float:
				param.animator.SetFloat(param.parameter, param.parameterValueFloat);
				break;
			}
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
