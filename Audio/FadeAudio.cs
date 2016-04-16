﻿// ************************************************************************ 
// File Name:   FadeAudio.cs 
// Purpose:    	Fades an audio source in or out.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework { 

	// ********************************************************************* 
	// Class: FadeAudio
	// ********************************************************************
	public class FadeAudio : MonoBehaviour 
	{
		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		private AudioSource m_audioSource = null;
		[SerializeField]
		private bool m_startsAudible = false;
		[SerializeField]
		private bool m_fadeOnAwake = false;
		[SerializeField]
		private float m_fadeSpeed = 1.0f;
		[SerializeField]
		private float m_minVolume = 0;
		[SerializeField]
		private float m_maxVolume = 1.0f;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Data Members
		// ****************************************************************
		private bool m_fadingIn = false;
		private bool m_fadingOut = false;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Properties
		// ****************************************************************
		public bool isFullyAudible {
			get { 
				if (m_audioSource != null && m_audioSource.volume == m_maxVolume)
					return true;
				else return false;
			}
		}
		public bool isInaudible {
			get { 
				if (m_audioSource != null && m_audioSource.volume == m_minVolume)
					return true;
				else return false;
			}
		}
		public float fadeSpeed {
			get { return m_fadeSpeed; }
			set { m_fadeSpeed = value; }
		}
		public float minVolume {
			get { return m_minVolume; }
			set { m_minVolume = value; }
		}
		public float maxVolume {
			get { return m_maxVolume; }
			set { m_maxVolume = value; }
		}
		public AudioSource audioSource {
			get { return m_audioSource; }
		}
		#endregion
		// ****************************************************************

		 
		// ****************************************************************
		#region Unity Methods
		// ****************************************************************
		void Start () {
			if (m_audioSource == null) return;

			if (m_startsAudible)
			{
				m_audioSource.volume = m_maxVolume;
				if (m_fadeOnAwake)
					FadeIn();
			}
			else
			{
				m_audioSource.volume = m_minVolume;
				if (m_fadeOnAwake)
					FadeOut();
			}
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Public Methods
		// ****************************************************************
		public Coroutine FadeIn() { return StartCoroutine(_FadeIn()); }
		public IEnumerator _FadeIn()
		{
			if (m_fadingIn || m_fadingOut)
				yield break;
			m_fadingIn = true;

			while (m_audioSource.volume < m_maxVolume)
			{
				yield return null;
				m_audioSource.volume += m_fadeSpeed * Time.deltaTime;
			}
			m_audioSource.volume = m_maxVolume;

			m_fadingIn = false;
		}
		// ****************************************************************


		// ****************************************************************
		public Coroutine FadeOut() { return StartCoroutine(_FadeOut()); }
		public IEnumerator _FadeOut()
		{
			if (m_fadingIn || m_fadingOut)
				yield break;
			m_fadingOut = true;

			while (m_audioSource.volume > m_minVolume)
			{
				yield return null;
				m_audioSource.volume -= m_fadeSpeed * Time.deltaTime;
			}
			m_audioSource.volume = m_minVolume;

			m_fadingOut = false;
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


	}
}