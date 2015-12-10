// ************************************************************************ 
// File Name:   InputAxisMovement2D.cs 
// Purpose:    	Accepts two axis inputs and passes it to the entity.
// Project:		
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


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
// Class: InputAxisMovement2D
// ************************************************************************ 
public class InputAxisMovement2D : MonoBehaviour {


    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	[SerializeField]
	private Entity m_entity = null;
	[SerializeField]
	private string m_axisHorizontal = "Horizontal";
	[SerializeField]
	private string m_axisVertical = "Vertical";
	
	
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
		float up = Input.GetAxis(m_axisVertical);
		
		// Build direction vector based on input
		Vector3 direction = new Vector3(right, up, 0.0f);
		
		// Move in the set direction
		m_entity.Move(direction);
		
		// Set facing
		if (right != 0 || up != 0 )
		{
			m_entity.TurnToFaceDirection(direction);
		}
	}
}