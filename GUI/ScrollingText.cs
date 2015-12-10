// ************************************************************************ 
// File Name:   ScrollingText.cs 
// Purpose:    	Text scrolls across a scissored area, looping.
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
// Class: ScrollingText
// ************************************************************************ 
public class ScrollingText : MonoBehaviour {


	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private int m_numRepeats = 0;
	[SerializeField]
	private bool m_repeatWhenEmpty = true;
	[SerializeField]
	private bool m_autoRemove = false;
	[SerializeField]
	private TextMesh m_textMesh;
	[SerializeField]
	private float m_speed = 1;
	[SerializeField]
	private float m_scissorMin = -4;
	[SerializeField]
	private float m_scissorMax = 4;
	[SerializeField]
	private List<string> m_messages;


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private int m_currentNumRepeats = 0;

	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
		if (m_messages.Count > 0)
			m_textMesh.text = m_messages[0];
		else
			m_textMesh.text = "";
	}


	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update () {
		// Move current message left
		m_textMesh.transform.position -= new Vector3( m_speed * Time.deltaTime, 0, 0);
		
		// Check if the message is off screen
		if (m_textMesh.GetComponent<Renderer>().bounds.max.x < m_scissorMin)
		{
			UseNextMessage();
		}
	}
	
	
	// ********************************************************************
	// Function:	UseNextMessage()
	// Purpose:		Updates to the next message
	// ********************************************************************
	private void UseNextMessage()
	{
		bool loadNew = false;

		if (m_currentNumRepeats > m_numRepeats && m_numRepeats != 0)
			loadNew = true;

		if (!(m_repeatWhenEmpty && m_messages.Count <= 1))
			loadNew = true;
				    
		if (m_textMesh.text == "")
			loadNew = true;

		if (m_textMesh.text != m_messages[0])
			loadNew = true;

		if (!loadNew)
			++m_currentNumRepeats;

		if (loadNew) 
		{
			if (!m_autoRemove)
				m_messages.Add (m_messages[0]);
			m_messages.RemoveAt (0);
			
			m_currentNumRepeats = 0;
			
			if ( m_messages.Count > 0)
				m_textMesh.text = m_messages[0];
			else
				m_textMesh.text = "";
		}
		
		// Move off screen to the right
		float width = m_textMesh.GetComponent<Renderer>().bounds.max.x - m_textMesh.GetComponent<Renderer>().bounds.min.x;
		m_textMesh.transform.position = new Vector3(m_scissorMax + width/2, 
		                                            m_textMesh.transform.position.y, 
		                                            m_textMesh.transform.position.z);
	}


	// ********************************************************************
	// Function:	AddMessage()
	// Purpose:		Adds a message to the message queue
	// ********************************************************************
	public void AddMessage(string newMessage)
	{
		m_messages.Add(newMessage);
		if (m_textMesh.text == "")
			UseNextMessage();
	}
	
	
	// ********************************************************************
	// Function:	RemoveMessage()
	// Purpose:		Removes a specific message from the queue
	// ********************************************************************
	public void RemoveMessage(string newMessage)
	{
		m_messages.Remove (newMessage);
	}
}
