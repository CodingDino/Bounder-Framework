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

	public void DestroyOnEvent () 
	{
		if (m_action == Action.DESTROY)
			Destroy(gameObject);
		else if (m_action == Action.DISABLE)
			gameObject.SetActive(false);
	}
}