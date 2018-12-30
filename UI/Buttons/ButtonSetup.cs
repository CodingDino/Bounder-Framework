// ************************************************************************ 
// File Name:   ButtonSetup.cs 
// Purpose:    	Setup a button's text, icon, background, animations, etc
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ButtonSetup
// ************************************************************************ 
public class ButtonSetup : MonoBehaviour
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private Text m_text;
	[SerializeField]
	private Image m_icon;
	[SerializeField]
	private Animator m_animator;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void SetText(string _text)
	{
		m_text.text = _text;
	}
	// ********************************************************************
	public void SetIcon(Sprite _icon)
	{
		m_icon.sprite = _icon;
		m_icon.enabled = _icon != null;
	}
	// ********************************************************************
	public void SetAnimation(RuntimeAnimatorController _animation)
	{
		m_animator.runtimeAnimatorController = _animation;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************