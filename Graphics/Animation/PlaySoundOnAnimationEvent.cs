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
		[Tooltip("ID for this audio info (defaults to clip name)")]
		public string id = "";
		[Tooltip("Audio info for the clip to use")]
		public AudioInfo info = new AudioInfo();
		[Tooltip("Should the audio be parented to this object?")]
		public bool parent = false;

		public string GetID()
		{
			return id.NullOrEmpty() ? clip.name : id;
		}
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
	private Dictionary<string,AudioObject> m_activeSounds = new Dictionary<string, AudioObject>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		for (int i = 0; i < m_sounds.Count; ++i)
		{
			SoundData sound = m_sounds[i];
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

		SoundData sound = m_soundMap[_id];

		AudioObject audio = AudioManager.Play(sound.clip, sound.info);
		m_activeSounds[_id] = audio;
		if (sound.parent)
			audio.transform.SetParent(transform);
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
		if (audio.gameObject.activeSelf && audio.audioSource.isPlaying && audio.audioClip.name == m_soundMap[_id].clip.name)
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
