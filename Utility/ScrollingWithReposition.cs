// ************************************************************************ 
// File Name:   ScrollingWithReposition.cs 
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
// Class: ScrollingWithReposition
// ************************************************************************ 
public class ScrollingWithReposition : MonoBehaviour {
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private Renderer[] m_images = new Renderer[0];
	[SerializeField]
	private float m_speed = -1.0f;
	[SerializeField]
	private float m_maxY = 0f;
	[SerializeField]
	private float m_minY = 0f;
	[SerializeField]
	private bool m_useLocalHardLimits = false;
	[SerializeField]
	private float m_maxX = 0f;
	[SerializeField]
	private float m_minX = 0f;
	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	
	
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
		
		for (int i = 0; i < m_images.Length; ++i)
		{
			// Move objects over
			m_images[i].transform.position = m_images[i].transform.position + Vector3.right*m_speed*Time.deltaTime;
			
		}
		for (int i = 0; i < m_images.Length; ++i)
		{

			
			if (!m_useLocalHardLimits)
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
					float width = m_images[i].GetComponent<Renderer>().bounds.max.x - m_images[i].GetComponent<Renderer>().bounds.min.x;
					// Move off screen to the right
					m_images[i].transform.position = new Vector3(maxWorldPoint.x + width/2*(-1*direction), 
					                                             Random.Range(m_minY,m_maxY), 
					                                             m_images[i].transform.position.z);
				}
			}
			else
			{
				// Check if they need to be wrapped back around
				if (   m_images[i].transform.localPosition.x < m_minX )
				{
					// Move off screen to the right
					m_images[i].transform.localPosition = new Vector3(m_maxX, 
					                                                  Random.Range(m_minY,m_maxY), 
					                                                  m_images[i].transform.localPosition.z);
				}
				else if (m_images[i].transform.localPosition.x > m_maxX )
				{
					// Move off screen to the left
					m_images[i].transform.localPosition = new Vector3(m_minX, 
					                                                  Random.Range(m_minY,m_maxY), 
					                                                  m_images[i].transform.localPosition.z);
				}
			}
		}



		
	}
}