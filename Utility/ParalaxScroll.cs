// ************************************************************************ 
// File Name:   ParalaxScroll.cs 
// Purpose:    	Control a paralax scrolling layer
// Project:		Armoured Engines
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
// Class: ParalaxScroll
// ************************************************************************ 
public class ParalaxScroll : MonoBehaviour {
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private Renderer[] m_images = new Renderer[2];
	[SerializeField]
	private float m_speed = 10.0f;
	[SerializeField]
	private float m_overlap = 0.01f;

    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	
	
    // ********************************************************************
    // Properties 
    // ********************************************************************

	
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {

		for (int i = 0; i < m_images.Length; ++i)
		{
			// Move objects over
			m_images[i].transform.position = m_images[i].transform.position + Vector3.right*m_speed*Time.deltaTime;

		}
		for (int i = 0; i < m_images.Length; ++i)
		{
			Vector3 minScreenPoint = new Vector3(0,0,m_images[i].transform.position.z);
			Vector3 maxScreenPoint = new Vector3(Screen.width,Screen.height,m_images[i].transform.position.z);
			Vector3 minWorldPoint = Camera.main.ScreenToWorldPoint(minScreenPoint);
			Vector3 maxWorldPoint = Camera.main.ScreenToWorldPoint(maxScreenPoint);
			
			// Check if they need to be wrapped back around
			if (   m_images[i].bounds.max.x < minWorldPoint.x
				|| m_images[i].bounds.min.x > maxWorldPoint.x )
			{
				// Determine direction and width
				float direction = m_speed / Mathf.Abs(m_speed);
				float width = m_images[i].GetComponent<Renderer>().bounds.max.x - m_images[i].GetComponent<Renderer>().bounds.min.x - m_overlap;
				// Apply
				int index = (i-1+m_images.Length)%m_images.Length;
				Vector3 lastInLinePosition = m_images[index].transform.position;
				m_images[i].transform.position = new Vector3(lastInLinePosition.x + width*(-1*direction), 
				                                             m_images[i].transform.position.y, 
				                                             m_images[i].transform.position.z);
			}
		}

	}
}