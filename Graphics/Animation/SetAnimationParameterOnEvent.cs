// ************************************************************************ 
// File Name:   SetAnimationParameterOnEvent.cs 
// Purpose:    	Listens for game events for setting animation paramters
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
#region Class: SetAnimationParameterOnEvent
// ************************************************************************
[RequireComponent(typeof(Animator))]
public class SetAnimationParameterOnEvent : MonoBehaviour 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("ID used in the events sent to this script")]
	private string m_id;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Animator m_animator;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		m_animator = GetComponent<Animator>();
	}
	// ********************************************************************
	void OnEnable()
	{
		Events.AddListener<AnimatorControllerParameterData>(OnEvent);
	}
	// ********************************************************************
	void OnDisable()
	{
		Events.RemoveListener<AnimatorControllerParameterData>(OnEvent);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	void OnEvent(AnimatorControllerParameterData _event)
	{
		if (_event.id == m_id)
			_event.Apply(m_animator);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
