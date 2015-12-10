// ************************************************************************ 
// File Name:   InputPlatformMovement2D.cs 
// Purpose:    	Accepts one axis input and jumping, and passes it to the 
//				entity.
// Project:		
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 
// TODO: Crouch, Run, Punch, Kick


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 


// ************************************************************************ 
// Class: InputPlatformMovement2D
// ************************************************************************ 
public class InputPlatformMovement2D : MonoBehaviour {


    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	[SerializeField]
	private Entity m_entity = null;
	[SerializeField]
	private ColliderList m_groundTrigger = null;
	[SerializeField]
	private string m_axisHorizontal = "Horizontal";
	[SerializeField]
	private string m_buttonJump = "Jump";
	[SerializeField]
	private float m_jumpVelocity = 100;
	
	
    // ********************************************************************
    // Properties 
    // ********************************************************************
	
	
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
		if (m_entity == null)
			return;
		
		// Get input from controller/keyboard
		float right = Input.GetAxis(m_axisHorizontal);
		bool jump = Input.GetButtonDown (m_buttonJump);
		
		// Build direction vector based on input
		Vector3 direction = new Vector3(right, 0.0f, 0.0f);
		
		// Move in the set direction
		m_entity.MoveX(direction.x);
		if (jump && m_groundTrigger != null && m_groundTrigger.isColliding)
			m_entity.MoveY(1.0f,m_jumpVelocity);
		
		// Set facing
		if (right != 0)
		{
			m_entity.TurnToFaceDirectionInstant(direction);
		}



	}
}