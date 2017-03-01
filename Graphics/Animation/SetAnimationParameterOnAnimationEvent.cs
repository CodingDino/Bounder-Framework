// ************************************************************************ 
// File Name:   SetAnimationParameterOnAnimationEvent.cs 
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
#region Class: AnimatorControllerParameterData
// ************************************************************************
[System.Serializable]
public class AnimatorControllerParameterData
{
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
}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: SetAnimationParameterOnAnimationEvent
// ************************************************************************
public class SetAnimationParameterOnAnimationEvent : MonoBehaviour 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("Parameters you want to change.")]
	private List<AnimatorControllerParameterData> m_parameters = new List<AnimatorControllerParameterData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string,AnimatorControllerParameterData> m_paramMap = new Dictionary<string, AnimatorControllerParameterData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Start()
	{
		for (int i = 0; i < m_parameters.Count; ++i)
		{
			AnimatorControllerParameterData param = m_parameters[i];
			if (m_paramMap.ContainsKey(param.id))
				Debug.LogError("Duplicate ID found: "+param.id);
			else
				m_paramMap[param.id] = param;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	void SetParameter(string _id)
	{
		if (!m_paramMap.ContainsKey(_id))
		{
			Debug.LogError("No data found for ID: "+_id);
			return;
		}

		AnimatorControllerParameterData param = m_paramMap[_id];
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
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
