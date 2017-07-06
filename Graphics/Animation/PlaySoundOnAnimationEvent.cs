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
	#region Class: SoundData
	// ********************************************************************
	[System.Serializable]
	public class SoundData
	{
		[Tooltip("Clip you want to play")]
		public AudioClip clip = null;
		[Tooltip("Audio info for the clip to use")]
		public AudioInfo info = new AudioInfo();
		[Tooltip("Should the audio be parented to this object?")]
		public bool parent = false;
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("Parameters you want to change.")]
	private List<SoundData> m_sounds = new List<SoundData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string,SoundData> m_soundMap = new Dictionary<string, SoundData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Start()
	{
		for (int i = 0; i < m_sounds.Count; ++i)
		{
			SoundData sound = m_sounds[i];
			if (m_soundMap.ContainsKey(sound.clip.name))
				Debug.LogError("Duplicate ID found: "+sound.clip.name);
			else
				m_soundMap[sound.clip.name] = sound;
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

		SoundData sound = m_soundMap[_id];

		AudioObject audio = AudioManager.Play(sound.clip, sound.info);
		if (sound.parent)
			audio.transform.SetParent(transform);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
