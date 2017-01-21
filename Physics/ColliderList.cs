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
	private List<Collision2D> m_collisions = new List<Collision2D>();


	// ********************************************************************
	// Properties
	// ********************************************************************
	public List<Collider2D> colliders {
		get { return m_colliders; }
	}
	public bool isColliding {
		get { if (m_colliders.Count > 0) return true; else return false; }
	}

	public bool IsCollidingWithTag(string _tag)
	{
		if (!isColliding)
			return false;

		for (int i = 0; i < m_colliders.Count; ++i)
		{
			if (m_colliders[i].gameObject.tag == _tag )
				return true;
		}

		return false;
	}

	public GameObject GetColliderWithTag(string _tag)
	{
		if (!isColliding)
			return null;

		for (int i = 0; i < m_colliders.Count; ++i)
		{
			if (m_colliders[i].gameObject.tag == _tag )
				return m_colliders[i].gameObject;
		}

		return null;
	}

	public Collision2D GetCollisionWithTag(string _tag)
	{
		if (!isColliding)
			return null;

		for (int i = 0; i < m_collisions.Count; ++i)
		{
			if (m_collisions[i].collider.gameObject.tag == _tag )
				return m_collisions[i];
		}

		return null;
	}

	
	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, when .
	// ********************************************************************
	void LateUpdate () {
		m_colliders = new List<Collider2D>();
		m_collisions = new List<Collision2D>();
	}
	
	
	// ********************************************************************
	// Function:	OnCollisionStay2D()
	// Purpose:		Called a collider is colliding with this one.
	// ********************************************************************
	void OnCollisionStay2D(Collision2D collision) {
		if (!m_colliders.Contains(collision.collider))
		{
			m_colliders.Add (collision.collider);
			m_collisions.Add (collision);
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
