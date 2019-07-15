// ************************************************************************ 
// File Name:   MessagePanel.cs 
// Purpose:    	Simple pop up with a title, message, and one or two buttons
// Project:		Armorued Engines
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bounder.Framework;
using UnityEngine.UI;
using Fiftytwo;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: MessagePanelData
// ************************************************************************
[System.Serializable]
public class MessagePanelData : PanelData 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public string titleText;
	public string messageText;
	public string positiveButtonText;
	public string negativeButtonText;
	public MessagePanel.ButtonPressed positiveCallback;
	public MessagePanel.ButtonPressed negativeCallback;
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: MessagePanel
// ************************************************************************
public class MessagePanel : Panel 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private Text m_titleText = null;
	[SerializeField]
	private Text m_messageText = null;
	[SerializeField]
	private Text m_positiveButtonText = null;
	[SerializeField]
	private Text m_negativeButtonText = null;
	[SerializeField]
	private GameObject m_positiveButton = null;
	[SerializeField]
	private GameObject m_negativeButton = null;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	public ButtonPressed m_positiveCallback = null;
	public ButtonPressed m_negativeCallback = null;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Events 
	// ********************************************************************
	public delegate void ButtonPressed();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Panel Methods 
	// ********************************************************************
	protected override void _Initialise(PanelData _data) 
	{
		MessagePanelData castData = _data as MessagePanelData;
		if (castData != null)
		{
			m_titleText.text = castData.titleText;
			m_messageText.text = castData.messageText;
			m_positiveButtonText.text = castData.positiveButtonText;
			m_negativeButtonText.text = castData.negativeButtonText;
			m_positiveCallback = castData.positiveCallback;
			m_negativeCallback = castData.negativeCallback;

			if (m_positiveCallback == null && m_negativeCallback == null)
			{
				m_positiveButton.SetActive(true);
				m_negativeButton.SetActive(false);
			}
			else
			{
				m_positiveButton.SetActive(m_positiveCallback != null);
				m_negativeButton.SetActive(m_negativeCallback != null);
			}
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void PositivePressed()
	{
		if (m_positiveCallback != null)
		{
			m_positiveCallback();
		}
		Close(); // close this panel
	}
	// ********************************************************************
	public void NegativePressed()
	{
		if (m_negativeCallback != null)
		{
			m_negativeCallback();
		}
		Close(); // close this panel
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
