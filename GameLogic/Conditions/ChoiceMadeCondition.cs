// ************************************************************************ 
// File Name:   ChoiceMadeCondition.cs 
// Purpose:    	A Condition for checking whether a dialogue choice has been made
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ChoiceMadeCondition
// ************************************************************************
[CreateAssetMenu(fileName = "ChoiceMadeCondition", menuName = "Bounder/Conditions/ChoiceMadeCondition", order = 1)]
public class ChoiceMadeCondition : Condition 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[SerializeField]
	private string m_choiceID;
	[SerializeField]
	private bool m_invert = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Condition Functions 
	// ********************************************************************
	public override void Register(bool _register) 
	{
		if (_register)
			DialoguePanel.OnChoiceMade += ChoiceMade;
		else
			DialoguePanel.OnChoiceMade -= ChoiceMade;
	}
	// ********************************************************************
	public override int GetProgress_Cumulative() 
	{
		PlayerProfile profile = ProfileManager.GetProfile<PlayerProfile>();
		if (profile == null)
		{
			Debug.LogError("ChoiceMadeCondition.GetProgress(): Could not find profile.");
			return 0;
		}

		bool choiceMade = profile.choicesMade.Contains(m_choiceID);
		return choiceMade != m_invert ? 1 : 0;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Functions 
	// ********************************************************************
	private void ChoiceMade(DialogueLink _link)
	{
		if (!m_invert &&_link.linkedFrame.name == m_choiceID)
		{
			AddProgress();
			Trigger();
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
