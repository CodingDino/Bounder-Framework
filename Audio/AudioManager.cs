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
	#region Enum: AudioType
	// ************************************************************************
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
	#region Class: AudioCategorySettings
	// ************************************************************************
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
		private AudioObject m_audioObjectPrefab;
		[SerializeField]
		private AudioCategorySettings[] m_audioCategorySettings;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Data Members
		// ****************************************************************
		private Dictionary<AudioCategory,AudioCategorySettings> m_audioCategorySettings_Internal = new Dictionary<AudioCategory,AudioCategorySettings>();
		private Dictionary<AudioCategory,List<AudioObject>> m_audioObjects = new Dictionary<AudioCategory,List<AudioObject>>();
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Unity Methods
		// ****************************************************************
		void Start()
		// ****************************************************************
		{
			for (int i = 0; i < m_audioCategorySettings.Length; ++i)
			{
				m_audioCategorySettings_Internal[m_audioCategorySettings[i].category] = m_audioCategorySettings[i];
				m_audioObjects[m_audioCategorySettings[i].category] = new List<AudioObject>();
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
		public static bool Play(AudioObject _audioObject, bool _overrideChannelLimit = false)
		{
			if (!_overrideChannelLimit && !ChannelAvailable(_audioObject.audioInfo.category))
				return false;
			
			_audioObject.Apply();
			_audioObject.audioSource.Play();
			instance.m_audioObjects[_audioObject.audioInfo.category].Add(_audioObject);

			return true;
		}
		// ****************************************************************
		public static bool Play(string _track, AudioCategory _category, bool _overrideChannelLimit = false)
		{
			return Play(_track, new AudioInfo(_category), _overrideChannelLimit);
		}
		// ****************************************************************
		public static bool Play(string _track, AudioInfo _info, bool _overrideChannelLimit = false)
		{
			if (!_overrideChannelLimit && !ChannelAvailable(_info.category))
				return false;

			AudioClip clip = Resources.Load<AudioClip>(instance.m_audioFolder+"/"+_info.category.ToString()+"/"+_track);

			return Play(clip,_info,_overrideChannelLimit);
		}
		// ****************************************************************
		public static bool Play(AudioClip _clip, AudioCategory _category, bool _overrideChannelLimit = false)
		{
			return Play(_clip, new AudioInfo(_category), _overrideChannelLimit);
		}
		// ****************************************************************
		public static bool Play(AudioClip _clip, AudioInfo _info, bool _overrideChannelLimit = false)
		{
			if (!_overrideChannelLimit && !ChannelAvailable(_info.category))
				return false;

			// TODO: USE OBJECT POOLING - THIS WILL MAKE UNLIMITED AUDIO OBJECTS!
			GameObject audioGameObject = GameObject.Instantiate(instance.m_audioObjectPrefab.gameObject);
			AudioObject audioObject = audioGameObject.GetComponent<AudioObject>();
			audioObject.audioInfo = _info;
			audioObject.audioClip = _clip;

			return Play(audioObject, _overrideChannelLimit);
		}
		// ****************************************************************
		public static bool ChannelAvailable(AudioCategory _category)
		{
			return instance.m_audioObjects[_category].Count < instance.m_audioCategorySettings_Internal[_category].numChannels;
		}
		// ****************************************************************

//
//		// ****************************************************************
//		public static void FadeOut() { instance.m_fadeAudio.FadeOut(); }
//		// ****************************************************************
//
//
//		// ****************************************************************
//		public static void FadeIn() { instance.m_fadeAudio.FadeIn(); }
//		// ****************************************************************
//
//
//		// ****************************************************************
//		public static void PlayMusic(string clipResourceLocation) 
//		{
//			AudioClip clip = (AudioClip) Resources.Load(instance.m_audioFolder+"/"+clipResourceLocation);
//			PlayMusic(clip);
//		}
//		// ****************************************************************
//
//
//		// ****************************************************************
//		public static IEnumerator PlayMusic(AudioClip clip) 
//		{
//			Debug.Log ("Queuing clip "+clip.name);
//			// fade current audio out if clip does not match, then when faded out, switch clips and fade back in
//			if (instance.m_fadeAudio.audioSource.clip != clip)
//			{
//				yield return instance.m_fadeAudio.FadeOut();
//
//				Debug.Log ("Playing clip "+clip.name);
//				instance.m_fadeAudio.audioSource.clip = clip;
//				instance.m_fadeAudio.audioSource.Play();
//				instance.m_fadeAudio.FadeIn();
//			}
//		}
//		// ****************************************************************
//
//
//		// ****************************************************************
//		public static void SetVolume(float _volume) 
//		{
//			if (instance.m_fadeAudio.isFullyAudible)
//				instance.m_fadeAudio.audioSource.volume = _volume;
//			instance.m_fadeAudio.maxVolume = _volume;
//		}
//		// ****************************************************************


		// ****************************************************************
		#endregion
		// ****************************************************************
	}
	#endregion
	// ********************************************************************
}
// ************************************************************************