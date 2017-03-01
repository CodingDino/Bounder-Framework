// ************************************************************************ 
// File Name:   DropAnimationTrigger.cs 
// Purpose:    	Changes sprite's animation if the collider is not touching 
//				the ground.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: DropAnimationTrigger
// ************************************************************************ 
public class DropAnimationTrigger : MonoBehaviour {
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private Animator m_animator = null;
	[SerializeField]
	private ColliderList m_collider = null;
	[SerializeField]
	private float m_timeout = 0.1f;
	[SerializeField]
	private string m_dropTrigger = "Drop";
	
	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private float m_lastCommand = 0;
	
	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
	}
	
	
	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update () {
		if (m_animator == null || m_collider == null)
			return;
		
		if (Time.time < m_lastCommand + m_timeout)
			return;
		
		if (m_collider.isColliding)
		{
			m_animator.SetBool(m_dropTrigger, false);
		}
		else
		{
			m_animator.SetBool(m_dropTrigger, true);
		}
		
		m_lastCommand = Time.time;
	}
}

