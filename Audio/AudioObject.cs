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
using UnityEngine;
using UnityEngine.Audio;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework 
{ 

	// ********************************************************************
	#region Class: AudioObject
	// ********************************************************************
	public class AudioObject : MonoBehaviour 
	{

		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		private AudioSource m_audioSource;
		[SerializeField]
		private AudioClip m_audioClip;
		[SerializeField]
		private AudioInfo m_audioInfo = new AudioInfo();
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Properties
		// ****************************************************************
		public AudioSource audioSource {
			get { return m_audioSource; }
			set { m_audioSource = value; }
		}
		// ****************************************************************
		public AudioClip audioClip {
			get { return m_audioClip; }
			set { m_audioClip = value; }
		}
		// ****************************************************************
		public AudioInfo audioInfo {
			get { return m_audioInfo; }
			set { m_audioInfo = value; }
		}
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Public Methods
		// ****************************************************************
		public void Apply()
		{
			m_audioSource.clip = m_audioClip;

			// Apply Info
			if (m_audioInfo != null)
			{
				m_audioSource.volume = m_audioInfo.volume + Random.Range(-m_audioInfo.volumeFuzz,m_audioInfo.volumeFuzz);
				m_audioSource.pitch = m_audioInfo.pitch + Random.Range(-m_audioInfo.pitchFuzz,m_audioInfo.pitchFuzz);
				m_audioSource.loop = m_audioInfo.shouldLoop;
			}

			// Apply Audio Mixer
			AudioCategorySettings settings = AudioManager.GetAudioSettings(m_audioInfo.category);
			m_audioSource.outputAudioMixerGroup = settings.group;
		}
		// ****************************************************************
		#endregion
		// ****************************************************************

	}
	#endregion
	// ********************************************************************

}
// ************************************************************************