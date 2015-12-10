// ************************************************************************ 
// File Name:   ColliderList.cs 
// Purpose:    	Tracks collisions and maintains a list.
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
// Attributes 
// ************************************************************************ 
[RequireComponent(typeof(Collider2D))]


// ************************************************************************ 
// Class: ColliderList
// ************************************************************************ 
public class ColliderList : MonoBehaviour {


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private List<Collider2D> m_colliders = new List<Collider2D>();


	// ********************************************************************
	// Properties
	// ********************************************************************
	public List<Collider2D> colliders {
		get { return m_colliders; }
	}
	public bool isColliding {
		get { if (m_colliders.Count > 0) return true; else return false; }
	}

	
	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, when .
	// ********************************************************************
	void LateUpdate () {
		m_colliders = new List<Collider2D>();
	}

	
//	// ********************************************************************
//	// Function:	OnCollisionEnter2D()
//	// Purpose:		Called when this collider encounters another.
//	// ********************************************************************
//	void OnCollisionEnter2D(Collision2D collision) {
//		if (!m_colliders.Contains(collision.collider))
//		{
//			m_colliders.Add (collision.collider);
//		}
//	}
	
	
//	// ********************************************************************
//	// Function:	OnTriggerEnter2D()
//	// Purpose:		Called when this collider encounters another.
//	// ********************************************************************
//	void OnTriggerEnter2D(Collider2D otherCollider) {
//		if (!m_colliders.Contains(otherCollider))
//		{
//			m_colliders.Add (otherCollider);
//		}
//	}
//	
//	
//	// ********************************************************************
//	// Function:	OnCollisionExit2D()
//	// Purpose:		Called a collider stops colliding with this one.
//	// ********************************************************************
//	void OnCollisionExit2D(Collision2D collision) {
//		if (m_colliders.Contains(collision.collider))
//		{
//			m_colliders.Remove(collision.collider);
//		}
//	}
//	
//	
//	// ********************************************************************
//	// Function:	OnTriggerExit2D()
//	// Purpose:		Called a collider stops colliding with this one.
//	// ********************************************************************
//	void OnTriggerExit2D(Collider2D otherCollider) {
//		if (m_colliders.Contains(otherCollider))
//		{
//			m_colliders.Remove(otherCollider);
//		}
//	}
	
	
	// ********************************************************************
	// Function:	OnCollisionStay2D()
	// Purpose:		Called a collider is colliding with this one.
	// ********************************************************************
	void OnCollisionStay2D(Collision2D collision) {
		if (!m_colliders.Contains(collision.collider))
		{
			m_colliders.Add (collision.collider);
		}
	}
	
	
	// ********************************************************************
	// Function:	OnTriggerStay2D()
	// Purpose:		Called a collider is colliding with this one.
	// ********************************************************************
	void OnTriggerStay2D(Collider2D otherCollider) {
		if (!m_colliders.Contains(otherCollider))
		{
			m_colliders.Add (otherCollider);
		}
	}
}
