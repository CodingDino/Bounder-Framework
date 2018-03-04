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
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: Panel
// ************************************************************************
[RequireComponent(typeof(Animator))]
public class Panel : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private string m_group = "";
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private PanelState m_state = PanelState.HIDDEN;
	private List<Button> m_disabledButtons = new List<Button>();
	private GraphicRaycaster m_raycaster = null;
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
			if (m_raycaster != null) 
				m_raycaster.enabled = value; 
		} 
		get 
		{ 
			if (m_raycaster != null) 
				return m_raycaster.enabled; 
			else 
				return false; 
		} 
	}
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
	public void Initialise (PanelData _data)  
	{ 
		m_raycaster = GetComponent<GraphicRaycaster>();
		PanelState startingState = _data != null ? _data.startingState : PanelState.HIDDEN;
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
		StartCoroutine(UpdateState());

		// Diable buttons
		Button[] allButtons = GetComponentsInChildren<Button>();
		for (int i = 0; i < allButtons.Length; ++i)
		{
			if (allButtons[i].enabled)
			{
				allButtons[i].enabled = false;
				m_disabledButtons.Add(allButtons[i]);
			}
		}

		_Hide();
	}
	// ********************************************************************
	public void Show ()  
	{ 
		if (m_state == PanelState.SHOWN || m_state == PanelState.SHOWING )
			return;

		ChangeState(PanelState.SHOWING);
		StartCoroutine(UpdateState());

		for (int i = 0; i < m_disabledButtons.Count; ++i)
		{
			m_disabledButtons[i].enabled = true;
		}
		m_disabledButtons.Clear();

		_Show();
	}
	// ********************************************************************
	public void Close ()
	{
		PanelManager.ClosePanel(name);
	}
	// ********************************************************************
	public IEnumerator CloseAfterDelay (float _delay)
	{
		yield return new WaitForSeconds(_delay);
		PanelManager.ClosePanel(name);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private IEnumerator UpdateState()
	{
		while (IsTransitioning())
		{
			PanelState nextState = (PanelState)((int)m_state << 1);
			if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(nextState.ToString()))
				ChangeState(nextState);
			else yield return null;
		}
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
		GetComponent<Animator>().SetInteger("PanelState",(int)m_state);
		if (_instant)
			GetComponent<Animator>().SetTrigger("InstantChange");
		gameObject.SetActive(IsVisible());
		interactable = m_state == PanelState.SHOWN;
		_ChangeState(_newState, oldState);
		if (OnPanelStateChanged != null)
			OnPanelStateChanged(name, _newState, oldState);
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
	protected virtual void _Show() {}
	// ********************************************************************
	protected virtual void _ChangeState(PanelState _newState, PanelState _oldState) {}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
