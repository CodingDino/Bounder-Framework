// ************************************************************************ 
// File Name:   AudioManager.cs 
// Purpose:    	Controls audio sources for the game.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework 
{ 
	// ********************************************************************
	#region Enum: AudioCategory
	// ********************************************************************
	public enum AudioCategory
	{
		INVALID = -1,
		// ---
		Master = 0, // NOTE: Must be camel case for Unity interaction
		EFFECTS,
		MUSIC,
		DIALOGUE,
		// ---
		NUM
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Enum: AudioChannelOverride
	// ********************************************************************
	public enum AudioChannelOverride
	{
		INVALID = -1,
		// ---
		NONE = 0,
		ADD,
		REPLACE,
		// ---
		NUM
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Class: AudioCategorySettings
	// ********************************************************************
	[System.Serializable]
	public class AudioCategorySettings
	{
		public AudioCategory category;
		public AudioMixerGroup group;
		public int numChannels;
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Class: AudioManager
	// ********************************************************************
	public class AudioManager : Singleton<AudioManager>
	{

		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		private string m_audioFolder;
		[SerializeField]
		private GameObject m_audioObjectPrefab;
		[SerializeField]
		private AudioCategorySettings[] m_audioCategorySettings;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Data Members
		// ****************************************************************
		private Dictionary<AudioCategory,AudioCategorySettings> m_audioCategorySettings_Internal = new Dictionary<AudioCategory,AudioCategorySettings>();
		private Dictionary<AudioCategory,ObjectPool> m_audioObjects = new Dictionary<AudioCategory,ObjectPool>();
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region MonoBehaviour Methods
		// ****************************************************************
		void Start()
		{
			for (int i = 0; i < m_audioCategorySettings.Length; ++i)
			{
				m_audioCategorySettings_Internal[m_audioCategorySettings[i].category] = m_audioCategorySettings[i];
				m_audioObjects[m_audioCategorySettings[i].category] = new ObjectPool(m_audioObjectPrefab);
			}
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Public Methods
		// ****************************************************************
		public static AudioCategorySettings GetAudioSettings(AudioCategory _category) 
		{ 
			return instance.m_audioCategorySettings_Internal[_category];
		}
		// ****************************************************************
		public static AudioObject Play(string _track, 
	                                   AudioCategory _category)
		{
			return Play(_track, new AudioInfo(_category));
		}
		// ****************************************************************
		public static AudioObject Play(string _track, 
	                                   AudioInfo _info)
		{
			if (_info.overrideChannelLimit == AudioChannelOverride.NONE && !ChannelAvailable(_info.category))
				return null;

			string filepath = instance.m_audioFolder+_info.category.ToString()+"/"+_track;
			AudioClip clip = Resources.Load<AudioClip>(filepath);

			if (clip == null)
			{
				Debug.LogError("Attempt to load non-existant audio file: "+filepath);
				return null;
			}

			return Play(clip,_info);
		}
		// ****************************************************************
		public static AudioObject Play(AudioClip _clip, 
	                                   AudioCategory _category)
		{
			return Play(_clip, new AudioInfo(_category));
		}
		// ****************************************************************
		public static AudioObject Play(AudioClip _clip, 
	                                   AudioInfo _info)
		{
			if (_info.overrideChannelLimit == AudioChannelOverride.NONE && !ChannelAvailable(_info.category))
				return null;

			bool replacing = !ChannelAvailable(_info.category) && _info.overrideChannelLimit == AudioChannelOverride.REPLACE;
			if (replacing)
			{
				AudioObject oldObject = instance.m_audioObjects[_info.category].FirstActive.GetComponent<AudioObject>();
				oldObject.Fade(false);
			}

			GameObject audioGameObject = instance.m_audioObjects[_info.category].RequestObject();
			audioGameObject.transform.SetParent(instance.transform);
			AudioObject audioObject = audioGameObject.GetComponent<AudioObject>();
			audioObject.audioInfo = _info;
			audioObject.audioClip = _clip;

			audioObject.Apply();
			audioObject.audioSource.Play();

			if (_info.fadeDuration != 0)
			{
				audioObject.audioSource.volume=0;
				audioObject.Fade(true);
			}

			return audioObject;
		}
		// ****************************************************************
		public static bool ChannelAvailable(AudioCategory _category)
		{
			return instance.m_audioObjects[_category].Count < instance.m_audioCategorySettings_Internal[_category].numChannels;
		}
		// ****************************************************************
		#endregion
		// ****************************************************************
	}
	#endregion
	// ********************************************************************
}
// ************************************************************************