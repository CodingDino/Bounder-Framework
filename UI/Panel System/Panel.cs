// ************************************************************************ 
// File Name:   Panel.cs 
// Purpose:    	A self contained UI element
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: Panel
// ************************************************************************
[RequireComponent(typeof(CanvasGroup))]
public class Panel : MonoBehaviour 
{
    // ********************************************************************
    #region Exposed Data Members 
    // ********************************************************************
    [Header("General Panel Settings")]
    [SerializeField]
	private string m_group = "";
	[SerializeField]
	private bool m_initOnStart = false;
	[SerializeField]
	private PanelState m_initialState = PanelState.HIDDEN;
	[SerializeField]
	private Button m_firstSelected = null;
	[SerializeField]
	private bool m_closeOnCancel = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private PanelState m_state = PanelState.HIDDEN;
    #endregion
    // ********************************************************************

    // ********************************************************************
    #region Protected Data Members 
    // ********************************************************************
    protected CanvasGroup m_canvasGroup = null;
    protected Animator m_animator = null; // Deprecated, do not use!
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region  Events 
    // ********************************************************************
    public delegate void PanelStateChanged(string _panelName, PanelState _newState, PanelState _oldState);
	public static event PanelStateChanged OnPanelStateChanged;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public string group { get { return m_group; } set { m_group = value; } }
	public PanelState state { get { return m_state; } }
	public bool interactable { 
		set 
		{ 
			if (m_canvasGroup)
            {
                m_canvasGroup.interactable = value;
                m_canvasGroup.blocksRaycasts = value;
            }
		} 
		get
        {
			if (m_canvasGroup)
				return m_canvasGroup.interactable;
			else
				return false;
		} 
	}
	public bool closeOnCancel { get { return m_closeOnCancel; } }
	public Button firstSelected { 
		get { return m_firstSelected; } 
		set { 
			//Debug.LogWarning("Setting first selected for panel "+name+" to button: "+value.name);
			m_firstSelected = value; 
		}
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void Start ()  
	{ 
		if (m_initOnStart)
		{
			PanelManager.AddPanel(this);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************



	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public PanelData CreatePanelData(JSON _JSON)
	{
		return _CreatePanelData(_JSON);
	}
	// ********************************************************************
	public bool IsTransitioning() 
	{ 
		return PanelState.TRANSITION.Contains(m_state); 
	}
	// ********************************************************************
	public bool IsVisible() 
	{ 
		return PanelState.VISIBLE.Contains(m_state); 
	}
	// ********************************************************************
	public void Initialise (PanelData _data = null)
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
		m_animator = GetComponent<Animator>();
        PanelState startingState = _data != null ? _data.startingState : m_initialState;
		ChangeState(startingState, true, true);
		_Initialise(_data);
	}
	// ********************************************************************
	public void Uninitialise () 
	{ 
		_Uninitialise();
	}
	// ********************************************************************
	public void Hide ()  
	{ 
		if (m_state == PanelState.HIDDEN || m_state == PanelState.HIDING )
			return;
		
		ChangeState(PanelState.HIDING);
		StartCoroutine(Hide_CR());

		_Hide();
	}
	// ********************************************************************
	public void Show ()  
	{ 
		if (m_state == PanelState.SHOWN || m_state == PanelState.SHOWING )
			return;

		ChangeState(PanelState.SHOWING);
		StartCoroutine(Show_CR());

		_Show();
	}
	// ********************************************************************
	public void Close ()
	{
		PanelManager.ClosePanel(this);
	}
	// ********************************************************************
	public IEnumerator CloseAfterDelay (float _delay)
	{
		yield return new WaitForSeconds(_delay);
		PanelManager.ClosePanel(name);
	}
	// ********************************************************************
	public void SelectButton(Button _button)
	{
		if (_button != null)
		{
			_button.Select();
			_button.OnSelect(null);
			firstSelected = _button;
		}
	}
	// ********************************************************************
	public void FocusChanged(bool _isFocus)
	{
		if (_isFocus && InputManager.useDirectionalUINavigation)
			SelectButton(m_firstSelected);
		_OnFocusChange(_isFocus);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private IEnumerator Show_CR()
    {
        PanelState nextState = (PanelState)((int)m_state << 1);
		yield return StartCoroutine(_Show_CR());
		ChangeState(nextState);
    }
    // ********************************************************************
    private IEnumerator Hide_CR()
    {
        PanelState nextState = (PanelState)((int)m_state << 1);
        yield return StartCoroutine(_Hide_CR());
        ChangeState(nextState);
    }
    // ********************************************************************
    private void ChangeState (PanelState _newState, bool _forceApply = false, bool _instant = false)
	{
		if (_newState == m_state && !_forceApply)
			return;
		if (!PanelState.LEGAL.Contains(_newState))
		{
			Debug.LogError("Panel.ChangeState("+_newState+") - unrecognised state");
			return;
		}
		PanelState oldState = m_state;
		m_state = _newState;
		gameObject.SetActive(true);
		if (m_animator)
            m_animator.SetInteger("PanelState",(int)m_state);
		if (_instant && m_animator)
            m_animator.SetTrigger("InstantChange");
		gameObject.SetActive(IsVisible());
		interactable = m_state == PanelState.SHOWN;

		// Set initial selected object
		if (m_state == PanelState.SHOWN && firstSelected != null && InputManager.useDirectionalUINavigation)
		{
			SelectButton(firstSelected);
		}

		// Listen for control scheme changes
		if (m_state == PanelState.SHOWN)
        	Events.AddListener<ControlSchemeChangedEvent>(OnControlSchemeChanged);
		else if (m_state == PanelState.HIDDEN)
        	Events.RemoveListener<ControlSchemeChangedEvent>(OnControlSchemeChanged);

		_ChangeState(_newState, oldState);
		if (OnPanelStateChanged != null)
			OnPanelStateChanged(name, _newState, oldState);
	}
	// ********************************************************************
	private void OnControlSchemeChanged(ControlSchemeChangedEvent _event)
	{
		if (InputManager.useDirectionalUINavigation && EventSystem.current.currentSelectedGameObject == null)
		{
			SelectButton(firstSelected);
		}
		else if (!InputManager.useDirectionalUINavigation)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Inherited Methods 
	// ********************************************************************
	protected virtual PanelData _CreatePanelData(JSON _JSON)
	{
		return new PanelData(_JSON);
	}
	// ********************************************************************
	protected virtual void _Initialise(PanelData _data) {}
	// ********************************************************************
	protected virtual void _Uninitialise() {}
	// ********************************************************************
	protected virtual void _Hide() {}
	// ********************************************************************
	protected virtual void _Show() { }
    // ********************************************************************
    protected virtual IEnumerator _Show_CR()
    {
        // Deprecated behaviour, remove!
        if (m_animator)
        {
            PanelState nextState = (PanelState)((int)m_state << 1);
            while (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(nextState.ToString()))
                yield return null;
        }

        // Default show/hide behaviour just instantly shows, to be overriden by child classes
        m_canvasGroup.alpha = 1.0f;

        yield break;
    }
    // ********************************************************************
    protected virtual IEnumerator _Hide_CR()
    {
        // Deprecated behaviour, remove!
        if (m_animator)
        {
            PanelState nextState = (PanelState)((int)m_state << 1);
            while (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(nextState.ToString()))
                yield return null;
        }

        // Default show/hide behaviour just instantly hides, to be overriden by child classes
        m_canvasGroup.alpha = 0.0f;

        yield break;
    }
    // ********************************************************************
    protected virtual void _ChangeState(PanelState _newState, PanelState _oldState) {}
	// ********************************************************************
	protected virtual void _OnFocusChange(bool _isFocus) {}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
