// ************************************************************************ 
// File Name:   WalkAnimationTrigger.cs 
// Purpose:    	Changes sprite's animation if the entity is moving.
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
// Class: MatchSpriteToFacing
// ************************************************************************ 
public class WalkAnimationTrigger : MonoBehaviour {
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private Animator m_animator = null;
	[SerializeField]
	private Entity m_entity = null;
	[SerializeField]
	private float m_movementDeadzone = 0.1f;
	[SerializeField]
	private float m_timeout = 0.1f;
	[SerializeField]
	private string m_walkTrigger = "Walk";


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private float m_lastX = 0;
	private float m_diffX = 0;
	private float m_lastCommand = 0;
	
	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
		if (m_entity)
			m_lastX = m_entity.transform.position.x;
	}
	
	
	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update () {
		if (m_animator == null || m_entity == null)
			return;

		float thisX = m_entity.transform.position.x;
		m_diffX = Mathf.Abs(m_lastX - thisX);

		if (Time.time < m_lastCommand + m_timeout)
			return;

		if (m_entity.isMoving && m_diffX > m_movementDeadzone)
		{
			m_animator.SetBool(m_walkTrigger, true);
		}
		else
		{
			m_animator.SetBool(m_walkTrigger, false);
		}
		
		m_lastCommand = Time.time;
		m_lastX = thisX;
	}
}
