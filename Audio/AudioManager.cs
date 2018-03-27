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

// ************************************************************************
#region Enum: AudioCategory
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
// ************************************************************************


// ************************************************************************
#region Enum: AudioChannelOverride
// ************************************************************************
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
// ************************************************************************


// ************************************************************************
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
// ************************************************************************


// ************************************************************************
#region Class: AudioManager
// ************************************************************************
public class AudioManager : Database<AudioClip>
{

	// ********************************************************************
	#region Exposed Data Members
	// ********************************************************************
	[SerializeField]
	private string m_audioFolder;
	[SerializeField]
	private GameObject m_audioObjectPrefab;
	[SerializeField]
	private AudioCategorySettings[] m_audioCategorySettings;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members
	// ********************************************************************
	private Dictionary<AudioCategory,AudioCategorySettings> m_audioCategorySettings_Internal = new Dictionary<AudioCategory,AudioCategorySettings>();
	private Dictionary<AudioCategory,ObjectPool> m_audioObjectPools = new Dictionary<AudioCategory,ObjectPool>();
	private Dictionary<AudioCategory,List<AudioObject>> m_nonPooledObjects = new Dictionary<AudioCategory,List<AudioObject>>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		for (int i = 0; i < m_audioCategorySettings.Length; ++i)
		{
			m_audioCategorySettings_Internal[m_audioCategorySettings[i].category] = m_audioCategorySettings[i];
			m_audioObjectPools[m_audioCategorySettings[i].category] = new ObjectPool(m_audioObjectPrefab);
			m_nonPooledObjects[m_audioCategorySettings[i].category] = new List<AudioObject>();
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public static AudioCategorySettings GetAudioSettings(AudioCategory _category) 
	{ 
			return (instance as AudioManager).m_audioCategorySettings_Internal[_category];
	}
	// ********************************************************************
	public static AudioObject Play(string _track, 
                                   AudioCategory _category)
	{
		return Play(_track, new AudioInfo(_category));
	}
	// ********************************************************************
	public static AudioObject Play(string _track, 
                                   AudioInfo _info)
	{
		if (_info.overrideChannelLimit == AudioChannelOverride.NONE && !ChannelAvailable(_info.category))
			return null;
		
		if (!HasData(_track))
		{
			Debug.LogError("Attempt to play unloaded audio file: "+_track);
			return null;
		}

		return Play(GetData(_track),_info);
	}
	// ********************************************************************
	public static AudioObject Play(AudioClip _clip, 
                                   AudioCategory _category)
	{
		return Play(new AudioInfo(_clip, _category));
	}
	// ********************************************************************
	public static AudioObject Play(AudioClip _clip, 
                                   AudioInfo _info)
	{
		_info.clip = _clip;
		return Play(_info);
	}
	// ********************************************************************
	public static AudioObject Play(AudioInfo _info)
	{
		if (_info.clip == null)
		{
			Debug.LogWarning("Attempt to play null audio clip");
			return null;
		}

		if (_info.overrideChannelLimit == AudioChannelOverride.NONE && !ChannelAvailable(_info.category))
			return null;

		bool replacing = !ChannelAvailable(_info.category) && _info.overrideChannelLimit == AudioChannelOverride.REPLACE;
		if (replacing)
		{
			AudioObject oldObject = (instance as AudioManager).m_audioObjectPools[_info.category].FirstActive.GetComponent<AudioObject>();

			// If we're trying to replace it with the same thing, don't.
			if (oldObject.audioClip == _info.clip)
			{
				if (oldObject.fading == true)
				{
					oldObject.StopFade();
					oldObject.Fade(true); // turn it back on if it was fading out
				}
				return null;
			}

			oldObject.Fade(false);
		}

		GameObject audioGameObject = (instance as AudioManager).m_audioObjectPools[_info.category].RequestObject();
		if (_info.parent != null)
			audioGameObject.transform.SetParent(_info.parent);
		else
			audioGameObject.transform.SetParent(instance.transform);
		audioGameObject.name = "AudioObject - "+_info.clip.name;
		AudioObject audioObject = audioGameObject.GetComponent<AudioObject>();
		audioObject.audioInfo = _info;

		audioObject.Apply();
		audioObject.audioSource.Play();

		if (_info.fadeDuration != 0)
		{
			audioObject.audioSource.volume=0;
			audioObject.Fade(true);
		}


		return audioObject;
	}
	// ********************************************************************
	public static void RegisterAudioObject(AudioObject _object)
	{
		(instance as AudioManager).m_nonPooledObjects[_object.audioInfo.category].Add(_object);
	}
	// ********************************************************************
	public static bool ChannelAvailable(AudioCategory _category)
	{
		int objectPoolObjects = (instance as AudioManager).m_audioObjectPools[_category].Count;
		int nonPooledObjects = (instance as AudioManager).m_nonPooledObjects[_category].Count;
		return objectPoolObjects + nonPooledObjects < (instance as AudioManager).m_audioCategorySettings_Internal[_category].numChannels;
	}
	// ********************************************************************
	public static List<AudioObject> GetActiveAudioForCategory(AudioCategory _category)
	{
		List<AudioObject> audioObjects = new List<AudioObject>();
		List<ObjectPoolObject> objectPoolObjects = (instance as AudioManager).m_audioObjectPools[_category].activeObjects;
		for (int i = 0; i < objectPoolObjects.Count; ++i)
		{
			audioObjects.Add(objectPoolObjects[i].GetComponent<AudioObject>());
		}
		audioObjects.AddRange( (instance as AudioManager).m_nonPooledObjects[_category]);
		return audioObjects;
	}
	// ********************************************************************
	public static void StopCategory(AudioCategory _category)
	{
		List<AudioObject> audioObjects = AudioManager.GetActiveAudioForCategory(_category);
		for (int i = 0; i < audioObjects.Count; ++i)
		{
			audioObjects[i].Stop();
		}
	}
	// ********************************************************************
	public static void FadeOutCategory(AudioCategory _category)
	{
		List<AudioObject> audioObjects = AudioManager.GetActiveAudioForCategory(_category);
		for (int i = 0; i < audioObjects.Count; ++i)
		{
			audioObjects[i].Fade(false);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************

}
// ************************************************************************