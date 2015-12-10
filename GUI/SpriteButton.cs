// ************************************************************************ 
// File Name:   SpriteButton.cs 
// Purpose:    	Control a sprite based on button behaviour
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
// Class: SpriteButton
// ************************************************************************ 
public class SpriteButton : BounderButton {


	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	
	// Textures and display
	[SerializeField]
	private SpriteRenderer m_spriteRenderer;
	[SerializeField]
	private Sprite m_spriteNormal;
	[SerializeField]
	private Sprite m_spriteHover;
	[SerializeField]
	private Sprite m_spriteClicked;
	[SerializeField]
	private Sprite m_spriteDisabled;
	
	// Effects
	[SerializeField]
	private Vector2 m_offsetClick = Vector2.zero;
	[SerializeField]
	private float m_enlargeHover = 1.0f;
	[SerializeField]
	private float m_enlargeClick = 1.0f;
	
	// Text
	[SerializeField]
	private TextMesh m_textMesh;
	[SerializeField]
	private string m_text;
	[SerializeField]
	private Color m_fontColorNormal;
	[SerializeField]
	private Color m_fontColorHover;
	[SerializeField]
	private Color m_fontColorClicked;
	[SerializeField]
	private Color m_fontColorDisabled;


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Vector2 m_posNormal;
	private Vector2 m_posClicked;
	
	
	// ********************************************************************
	// Function:	Initialize()
	// Purpose:     Initialize the button.
	// ********************************************************************
	protected override void Initialize()
	{
		// Set up y coordintes
		m_posNormal = m_spriteRenderer.transform.position;
		m_posClicked = m_posNormal + m_offsetClick;

		// Set up text
		if (m_textMesh != null) m_textMesh.text = m_text;

		base.Initialize();
	}


	// ********************************************************************
	// Function:	OnStateChange()
	// Purpose:		Performs actions on state change
	// ********************************************************************
	protected override void OnStateChange(ButtonState _state)
	{
		// Check for click offset adjustments
		if (_state == ButtonState.CLICKED)
		{
			// Record current normal position and recalculate clicked position
			m_posNormal = m_spriteRenderer.transform.position;
			m_posClicked = m_posNormal + m_offsetClick;
			// Set to new position
			m_spriteRenderer.transform.position = 
				new Vector3 (m_posClicked.x,
				             m_posClicked.y,
				             m_spriteRenderer.transform.position.z);
		}
		else if (m_state == ButtonState.CLICKED)
		{
			// Record current normal position and recalculate clicked position
			m_posClicked = m_spriteRenderer.transform.position;
			m_posNormal = m_posClicked - m_offsetClick;
			// Set to new position
			m_spriteRenderer.transform.position = 
				new Vector3 (m_posNormal.x,
				             m_posNormal.y,
				             m_spriteRenderer.transform.position.z);
		}
		
		// Check for click enlarge adjustments
		if (_state == ButtonState.CLICKED)
		{
			m_spriteRenderer.transform.localScale = 
				new Vector3 (m_spriteRenderer.transform.localScale.x * m_enlargeClick,
				             m_spriteRenderer.transform.localScale.y * m_enlargeClick,
				             m_spriteRenderer.transform.localScale.z);
		}
		else if (m_state == ButtonState.CLICKED)
		{
			m_spriteRenderer.transform.localScale = 
				new Vector3 (m_spriteRenderer.transform.localScale.x / m_enlargeClick,
				             m_spriteRenderer.transform.localScale.y / m_enlargeClick,
				             m_spriteRenderer.transform.localScale.z);
		}
		
		// Check for hover enlarge adjustments
		if (_state == ButtonState.HOVER)
		{
			m_spriteRenderer.transform.localScale = 
				new Vector3 (m_spriteRenderer.transform.localScale.x * m_enlargeHover,
				             m_spriteRenderer.transform.localScale.y * m_enlargeHover,
				             m_spriteRenderer.transform.localScale.z);
		}
		else if (m_state == ButtonState.HOVER)
		{
			m_spriteRenderer.transform.localScale = 
				new Vector3 (m_spriteRenderer.transform.localScale.x / m_enlargeHover,
				             m_spriteRenderer.transform.localScale.y / m_enlargeHover,
				             m_spriteRenderer.transform.localScale.z);
		}
		
		// Set up sprites and fonts
		switch (_state)
		{
		case ButtonState.NORMAL:
			m_spriteRenderer.sprite = m_spriteNormal;
			if (m_textMesh != null) m_textMesh.color = m_fontColorNormal;
			break;
		case ButtonState.HOVER:
			m_spriteRenderer.sprite = m_spriteHover;
			if (m_textMesh != null) m_textMesh.color = m_fontColorHover;
			break;
		case ButtonState.CLICKED:
			m_spriteRenderer.sprite = m_spriteClicked;
			if (m_textMesh != null) m_textMesh.color = m_fontColorClicked;
			break;
		case ButtonState.DISABLED:
			m_spriteRenderer.sprite = m_spriteDisabled;
			if (m_textMesh != null) m_textMesh.color = m_fontColorDisabled;
			break;
		}
	}
}