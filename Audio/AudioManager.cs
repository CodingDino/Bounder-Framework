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
		EFFECTS = 0,
		MUSIC,
		DIALOG,
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
		public int numChannels;
		public float volume;
		public float pitch;
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
		public static bool Play(string _track, AudioInfo _info, bool _overrideChannelLimit = false)
		{
			if (!_overrideChannelLimit && !ChannelAvailable(_info.category))
				return false;

			AudioClip clip = Resources.Load<AudioClip>(instance.m_audioFolder+"/"+_info.category.ToString()+"/"+_track);

			return Play(clip,_info,_overrideChannelLimit);
		}
		// ****************************************************************
		public static bool Play(AudioClip _clip, AudioInfo _info, bool _overrideChannelLimit = false)
		{
			if (!_overrideChannelLimit && !ChannelAvailable(_info.category))
				return false;
			
			GameObject audioGameObject = GameObject.Instantiate(instance.m_audioObjectPrefab.gameObject);
			AudioObject audioObject = audioGameObject.GetComponent<AudioObject>();
			audioObject.audioInfo = _info;

			return Play(audioObject, _overrideChannelLimit);
		}
		// ****************************************************************
		public static bool ChannelAvailable(AudioCategory _category)
		{
			return instance.m_audioObjects[_category].Count < instance.m_audioCategorySettings_Internal[_category].numChannels;
		}
		// ****************************************************************

//		// ****************************************************************
//		public static void FadeOut() { instance.m_fadeAudio.FadeOut(); }
//		// ****************************************************************
//		// ****************************************************************
//		public static void FadeIn() { instance.m_fadeAudio.FadeIn(); }
//		// ****************************************************************
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


		// ****************************************************************
		#endregion
		// ****************************************************************
	}
	#endregion
	// ********************************************************************
}
// ************************************************************************
