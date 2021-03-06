﻿// ************************************************************************ 
// File Name:   AudioObject.cs 
// Purpose:    	Holds settings for a particular audio source & clip
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{


    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using System.Collections;
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************

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
        private bool m_playOnEnable = false;
        [SerializeField]
        private AudioInfo m_audioInfo = new AudioInfo();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Data Members
        // ********************************************************************
        private AudioSource m_audioSource;
        private bool m_fading = false;
        private bool m_hasPlayed = false;
        private bool m_usingObjectPool = false;
        private bool m_paused = false;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Properties
        // ********************************************************************
        public AudioSource audioSource
        {
            get { return m_audioSource; }
            set { m_audioSource = value; }
        }
        // ********************************************************************
        public AudioClip audioClip
        {
            get { return m_audioInfo.clip; }
            set { m_audioInfo.clip = value; }
        }
        // ********************************************************************
        public AudioInfo audioInfo
        {
            get { return m_audioInfo; }
            set { m_audioInfo = value; }
        }
        // ********************************************************************
        public bool fading
        {
            get { return m_fading; }
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
        IEnumerator Start()
        {
            // Register self with AudioManager
            if (!m_usingObjectPool)
            {
                while (SystemManager.initialised == false)
                    yield return null;

                AudioManager.RegisterAudioObject(this);
            }
        }
        // ********************************************************************
        void OnDestroy()
        {
            if (!m_usingObjectPool)
            {
                AudioManager.RegisterAudioObject(this, false);
            }
        }
        // ********************************************************************
        void Update()
        {
            if (m_playOnEnable && !m_hasPlayed)
            {
                if (AudioManager.initialized)
                {
                    Apply();
                    m_audioSource.Play();
                    m_hasPlayed = true;
                }
            }
            else if (!m_audioSource.isPlaying && m_usingObjectPool && !AudioListener.pause && !m_paused)
                gameObject.SetActive(false); // Triggers object pool to recycle
        }
        // ********************************************************************
        void OnDisable()
        {
            m_fading = false;
            m_hasPlayed = false;
        }
        // ********************************************************************
        void OnApplicationFocus(bool hasFocus)
        {
            m_paused = !hasFocus;
        }
        // ********************************************************************
        void OnApplicationPause(bool pauseStatus)
        {
            m_paused = pauseStatus;
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Methods
        // ********************************************************************
        public void Apply()
        {
            // Apply Info
            if (m_audioInfo != null)
            {
                m_audioSource.clip = m_audioInfo.clip;
                m_audioSource.volume = m_audioInfo.volume + Random.Range(-m_audioInfo.volumeFuzz, m_audioInfo.volumeFuzz);
                m_audioSource.pitch = m_audioInfo.pitch + Random.Range(-m_audioInfo.pitchFuzz, m_audioInfo.pitchFuzz);
                m_audioSource.loop = m_audioInfo.loop;
            }

            // Apply Audio Mixer
            AudioCategorySettings settings = AudioManager.GetAudioSettings(m_audioInfo.category);
            m_audioSource.outputAudioMixerGroup = settings.group;

            m_usingObjectPool = GetComponent<ObjectPoolObject>() != null;
        }
        // ********************************************************************
        public Coroutine Fade(bool _on)
        {
            return StartCoroutine(Fade_CR(_on));
        }
        // ********************************************************************
        public void StopFade()
        {
            StopAllCoroutines();
            m_fading = false;
        }
        // ********************************************************************
        public void Stop()
        {
            StopAllCoroutines();
            m_audioSource.Stop();
            if (m_usingObjectPool)
                gameObject.SetActive(false); // Triggers object pool to recycle
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Methods
        // ********************************************************************
        private IEnumerator Fade_CR(bool _on)
        {
            if (m_fading)
                yield break;
            m_fading = true;

            float targetVolume = _on ? m_audioInfo.volume + Random.Range(-m_audioInfo.volumeFuzz, m_audioInfo.volumeFuzz) : 0;
            float startVolume = m_audioSource.volume;
            float startTime = Time.time;

            while (Time.time < startTime + m_audioInfo.fadeDuration)
            {
                m_audioSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / m_audioInfo.fadeDuration);
                yield return null;
            }

            m_audioSource.volume = targetVolume;

            if (!_on)
            {
                m_audioSource.Stop();
                if (m_usingObjectPool)
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