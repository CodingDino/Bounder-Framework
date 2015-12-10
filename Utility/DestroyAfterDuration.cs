// ************************************************************************ 
// File Name:   DestroyAfterDuration.cs 
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
// Class: DestroyAfterDuration
// ************************************************************************ 
public class DestroyAfterDuration : MonoBehaviour {


    // ********************************************************************
    // Data Members 
    // ********************************************************************
	[SerializeField]
	private float m_duration = 0;
	private float m_startTime = 0;
	
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Start () {
		m_startTime = Time.time;
	}
	
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {
		if (Time.time >= m_startTime + m_duration)
		{
			Destroy (gameObject);
		}
	}
}