// ************************************************************************ 
// File Name:   PlaySoundOnAnimationEvent.cs 
// Purpose:    	
// Project:		
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: PlaySoundOnAnimationEvent
// ************************************************************************
public class PlaySoundOnAnimationEvent : MonoBehaviour 
{
	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	[SerializeField]
	AudioClip m_soundToPlay = null;
	[SerializeField]
	AudioCategory m_audioCategory = AudioCategory.EFFECTS;
	#endregion
	// ********************************************************************

	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void PlaySound (string _sound) 
	{
		AudioManager.Play(_sound, m_audioCategory);
	}
	private void PlaySoundClip () 
	{
		AudioManager.Play(m_soundToPlay, m_audioCategory);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
