// ************************************************************************ 
// File Name:   Button.cs 
// Purpose:     Detect button mouse over and click 
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: Button 
// ************************************************************************ 
public class BounderButton : MonoBehaviour 
{
	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	protected bool m_enabled = true;
	[SerializeField]
	protected ButtonImplementation m_implementation = null;
	
	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	protected enum ButtonState 
	{ 
		NORMAL, HOVER, CLICKED, DISABLED 
	};
	protected ButtonState m_state;
	protected bool m_hoveredThisFrame = false;
	protected bool m_hoveredLastFrame = false;
	protected bool m_clickedThisFrame = false;
	protected bool m_clickedLastFrame = false;
	
	
	// ********************************************************************
	// Properties 
	// ********************************************************************
	public bool buttonEnabled
	{
		get { return m_enabled; }
	}


	// ********************************************************************
	// Events 
	// ********************************************************************
	public delegate void ButtonPressedAction(BounderButton button);
	public static event ButtonPressedAction OnButtonPressed;
	
	
	// ********************************************************************
	// Function:	Start()
	// Purpose:     Run when new instance of the object is created.
	// ********************************************************************
	void Start () 
	{	
		Initialize();
	}
	
	
	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, after other functions.
	// ********************************************************************
	void LateUpdate () 
	{
		ProcessStates();
	}
	
	
	// ********************************************************************
	// Function:	OnMouseOver()
	// Purpose:		Called every frame while the mouse is over the Collider 
	// ********************************************************************
	void OnMouseOver()
	{
		ProcessMouseOver();
	}


	// ********************************************************************
	// Function:	Initialize()
	// Purpose:     Initialize the button.
	// ********************************************************************
	protected virtual void Initialize()
	{
		// Set up enabled/disabled
		if (m_enabled) 
			EnableButton();
		else 
			DisableButton();
	}
	
	
	// ********************************************************************
	// Function:	ProcessStates()
	// Purpose:		Checks states for the last frame and processes them
	// ********************************************************************
	protected virtual void ProcessStates()
	{
		if (!m_enabled) return;
		
		// Hover Enter
		if (m_hoveredThisFrame 
		    && !m_hoveredLastFrame)
		{
			OnHoverEnter();
			if (!m_clickedThisFrame) 
				SetState(ButtonState.HOVER);
			if (m_clickedThisFrame) 
				SetState(ButtonState.CLICKED);
		}
		
		// Hover Stay
		else if (m_hoveredThisFrame 
		         && m_hoveredLastFrame)
		{
			OnHoverStay();
			if (!m_clickedThisFrame) 
				SetState(ButtonState.HOVER);
		}
		
		// Hover Exit
		else if (!m_hoveredThisFrame 
		         && m_hoveredLastFrame)
		{
			OnHoverExit();
			SetState(ButtonState.NORMAL);
		}
		
		// Click start
		if (m_clickedThisFrame 
		    && !m_clickedLastFrame)
		{
			SetState(ButtonState.CLICKED);
		}
		
		// Click release
		if (!m_clickedThisFrame 
		    && m_clickedLastFrame)
		{
			if (m_hoveredThisFrame)
			{
				OnClick();
				SetState(ButtonState.HOVER);
			}
			else
				SetState(ButtonState.NORMAL);
			
		}
		
		// Reset hover and click test
		m_hoveredLastFrame = m_hoveredThisFrame;
		m_hoveredThisFrame = false;
		m_clickedLastFrame = m_clickedThisFrame;
		m_clickedThisFrame = false;
	}


	// ********************************************************************
	// Function:	ProcessMouseOver()
	// Purpose:		Called on mouse over, processes state
	// ********************************************************************
	protected virtual void ProcessMouseOver()
	{
		if (!m_enabled) return;
		
		m_hoveredThisFrame = true;
		if (Input.GetMouseButton(0))
			m_clickedThisFrame = true;
	}

	
	// ********************************************************************
	// Function:	DisableButton()
	// Purpose:		Disables the button
	// ********************************************************************
	public void DisableButton()
	{
		SetState (ButtonState.DISABLED);
	}
	
	
	// ********************************************************************
	// Function:	EnableButton()
	// Purpose:		Enables the button
	// ********************************************************************
	public void EnableButton()
	{
		SetState (ButtonState.NORMAL);
	}
	
	
	// ********************************************************************
	// Function:	SetState()
	// Purpose:		Sets the button to the supplied state and performs
	//				any necessary actions
	// ********************************************************************
	protected void SetState(ButtonState _state)
	{
		// If the state is already the current state, don't do anything
		if (m_state == _state) return;
		
		// Check for disabling or re-enabling
		if (_state == ButtonState.DISABLED)
		{
			m_enabled = false;
			m_hoveredThisFrame = false;
			m_hoveredLastFrame = false;
			m_clickedLastFrame = false;
			m_clickedThisFrame = false;
		}
		else if (m_state == ButtonState.DISABLED)
			m_enabled = true;

		OnStateChange(_state);
		
		// Record the new state
		m_state = _state;
	}
	
	
	// ********************************************************************
	// Function:	OnStateChange()
	// Purpose:		Performs actions on state change
	// ********************************************************************
	protected virtual void OnStateChange(ButtonState _state)
	{

	}


	// ********************************************************************
	// Function:	OnHoverEnter()
	// Purpose:		Called when the button is first hovered over
	// ********************************************************************
	protected virtual void OnHoverEnter()
	{
		if (!m_enabled) return;
		if (m_implementation != null) m_implementation.OnHoverEnter();
	}
	
	
	// ********************************************************************
	// Function:	OnHoverExit()
	// Purpose:		Called when the button is no longer hovered over
	// ********************************************************************
	protected virtual void OnHoverExit()
	{
		if (!m_enabled) return;
		if (m_implementation != null) m_implementation.OnHoverExit();
	}
	
	
	// ********************************************************************
	// Function:	OnHoverStay()
	// Purpose:		Called every frame the button is hovered over
	// ********************************************************************
	protected virtual void OnHoverStay()
	{
		if (!m_enabled) return;
		if (m_implementation != null) m_implementation.OnHoverStay();
	}
	
	
	// ********************************************************************
	// Function:	OnClick()
	// Purpose:		Called when the button is clicked
	// ********************************************************************
	protected virtual void OnClick()
	{
		if (!m_enabled) return;
		if (m_implementation != null) m_implementation.OnClick();
		
		if (OnButtonPressed != null) OnButtonPressed(this);
	}
}
