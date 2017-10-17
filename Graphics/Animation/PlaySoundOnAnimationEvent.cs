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
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("Audio info to play")]
	private List<AudioInfo> m_audioInfo = new List<AudioInfo>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string,AudioInfo> m_soundMap = new Dictionary<string, AudioInfo>();
	private Dictionary<string,AudioObject> m_activeSounds = new Dictionary<string, AudioObject>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		for (int i = 0; i < m_audioInfo.Count; ++i)
		{
			AudioInfo sound = m_audioInfo[i];
			string id = sound.GetID();
			if (m_soundMap.ContainsKey(id))
				Debug.LogError("Duplicate ID found: "+id);
			else
				m_soundMap[id] = sound;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void PlaySound (string _id) 
	{
		if (!m_soundMap.ContainsKey(_id))
		{
			Debug.LogError("No data found for ID: "+_id);
			return;
		}

		AudioInfo sound = m_soundMap[_id];

		AudioObject audio = AudioManager.Play(sound);
		m_activeSounds[_id] = audio;
	}
	// ********************************************************************
	private void StopSound (string _id) 
	{
		if (!m_soundMap.ContainsKey(_id))
		{
			Debug.LogError("No data found for ID: "+_id);
			return;
		}

		if (!m_activeSounds.ContainsKey(_id))
		{
			// NOTE: No error message as we just want this to be a no-op
			return;
		}

		AudioObject audio = m_activeSounds[_id];
		if (   audio != null
		    && audio.gameObject.activeSelf 
		    && audio.audioSource.isPlaying 
		    && audio.audioClip.name == m_soundMap[_id].clip.name)
		{
			audio.Fade(false);
		}
		m_activeSounds.Remove(_id);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
