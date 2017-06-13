// ************************************************************************ 
// File Name:   DestroyOnAnimationEvent.cs 
// Purpose:    	
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



// ************************************************************************ 
// Class: DestroyOnAnimationEvent
// ************************************************************************ 
public class DestroyOnAnimationEvent : MonoBehaviour {

	public enum Action {
		DESTROY,
		DISABLE
	};

	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	[SerializeField]
	private Action m_action = Action.DESTROY;
	[SerializeField]
	private GameObject m_toActOn = null;

	public void DestroyOnEvent () 
	{
		if (m_toActOn == null)
			m_toActOn = gameObject;

		if (m_action == Action.DESTROY)
			Destroy(m_toActOn);
		else if (m_action == Action.DISABLE)
			m_toActOn.SetActive(false);
	}
}