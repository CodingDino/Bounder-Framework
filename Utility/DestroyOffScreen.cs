// ************************************************************************ 
// File Name:   DestroyOffScreen.cs 
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
// Attributes 
// ************************************************************************ 


// ************************************************************************ 
// Class: DestroyOffScreen
// ************************************************************************ 
public class DestroyOffScreen : MonoBehaviour {


    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	[SerializeField]
	private Vector2 m_min = Vector2.zero;
	[SerializeField]
	private Vector2 m_max = Vector2.zero;
	[SerializeField]
	private float m_outsideDuration = 5.0f;

	private bool m_isOutside = false;
	private float m_outsideStartTime = 0.0f;
	
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {
		if (   transform.position.x < m_min.x
		    || transform.position.y < m_min.y
		    || transform.position.x > m_max.x
		    || transform.position.y > m_max.y )
		{
			if (!m_isOutside)
			{
				m_isOutside = true;
				m_outsideStartTime = Time.time;
			}

			if (Time.time >= m_outsideStartTime + m_outsideDuration)
			{
				Destroy(gameObject);
			}
		}
		else
			m_isOutside = false;
	}
}