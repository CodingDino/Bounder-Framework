// ************************************************************************ 
// File Name:   AudioObject.cs 
// Purpose:    	Holds settings for a particular audio source & clip
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework 
{ 

// ************************************************************************
#region Class: AudioObject
// ************************************************************************
[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour 
{

	// ********************************************************************
	#region Exposed Data Members
	// ********************************************************************
	[SerializeField]
	private AudioClip m_audioClip;
	[SerializeField]
	private AudioInfo m_audioInfo = new AudioInfo();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members
	// ********************************************************************
	private AudioSource m_audioSource;
	private bool m_fading;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties
	// ********************************************************************
	public AudioSource audioSource {
		get { return m_audioSource; }
		set { m_audioSource = value; }
	}
	// ********************************************************************
	public AudioClip audioClip {
		get { return m_audioClip; }
		set { m_audioClip = value; }
	}
	// ********************************************************************
	public AudioInfo audioInfo {
		get { return m_audioInfo; }
		set { m_audioInfo = value; }
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
	}
	// ********************************************************************
	void Update()
	{
		if (!m_audioSource.isPlaying)
			gameObject.SetActive(false); // Triggers object pool to recycle
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public void Apply()
	{
		m_audioSource.clip = m_audioClip;

		// Apply Info
		if (m_audioInfo != null)
		{
			m_audioSource.volume = m_audioInfo.volume + Random.Range(-m_audioInfo.volumeFuzz,m_audioInfo.volumeFuzz);
			m_audioSource.pitch = m_audioInfo.pitch + Random.Range(-m_audioInfo.pitchFuzz,m_audioInfo.pitchFuzz);
			m_audioSource.loop = m_audioInfo.loop;
		}

		// Apply Audio Mixer
		AudioCategorySettings settings = AudioManager.GetAudioSettings(m_audioInfo.category);
		m_audioSource.outputAudioMixerGroup = settings.group;
	}
	// ********************************************************************
	public Coroutine Fade(bool _on) { return StartCoroutine(_Fade(_on)); }
	public IEnumerator _Fade(bool _on)
	{
		if (m_fading)
			yield break;
		m_fading = true;

		float targetVolume = _on ? m_audioInfo.volume + Random.Range(-m_audioInfo.volumeFuzz,m_audioInfo.volumeFuzz) : 0;
		float startVolume = m_audioSource.volume;
		float startTime = Time.time;

		while (Time.time < startTime + m_audioInfo.fadeDuration)
		{
			m_audioSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime)/ m_audioInfo.fadeDuration);
			yield return null;
		}

		m_audioSource.volume = targetVolume;

		if (!_on)
		{
			m_audioSource.Stop();
			gameObject.SetActive(false); // Triggers object pool to recycle
		}

		m_fading = false;
	}
	// ********************************************************************

	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}
// ************************************************************************