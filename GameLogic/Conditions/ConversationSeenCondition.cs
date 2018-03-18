// ************************************************************************ 
// File Name:   ConversationSeenCondition.cs 
// Purpose:    	A Condition for checking whether a conversation has been seen
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
#region Class: ConversationSeenCondition
// ************************************************************************
[CreateAssetMenu(fileName = "ConversationSeenCondition", menuName = "Bounder/Conditions/ConversationSeenCondition", order = 1)]
public class ConversationSeenCondition : Condition 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[SerializeField]
	private string m_conversationID;
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
			DialoguePanel.OnConversationSeen += ConversationSeen;
		else
			DialoguePanel.OnConversationSeen -= ConversationSeen;
	}
	// ********************************************************************
	public override int GetProgress_Cumulative() 
	{
		PlayerProfile profile = ProfileManager.GetProfile<PlayerProfile>();
		if (profile == null)
		{
			Debug.LogError("ConversationSeenCondition.GetProgress(): Could not find profile.");
			return 0;
		}

		bool seen = profile.conversationsSeen.Contains(m_conversationID);
		return seen != m_invert ? 1 : 0;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Functions 
	// ********************************************************************
	private void ConversationSeen(DialogueConversation _conversation)
	{
		if (!m_invert && _conversation.name == m_conversationID)
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
