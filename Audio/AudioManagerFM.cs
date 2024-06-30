// ************************************************************************ 
// File Name:   AudioInfoFM.cs 
// Purpose:    	Information on a piece of audio, using FMOD
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2024 Bounder Games
// ************************************************************************
namespace Bounder.Framework
{

    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using FMODUnity;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************ 
    #region Class: AudioInfoFM
    // ************************************************************************
    public class AudioManagerFM : Singleton<AudioManagerFM>
    {
        // ********************************************************************
        #region Exposed Data Members
        // ********************************************************************
        [SerializeField]
        private StudioEventEmitter m_emitterPrefab = null;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Exposed Data Members
        // ********************************************************************
        private Dictionary<EventReference, ObjectPool> m_objectPools = new();
        private StudioEventEmitter m_currentMusic = null;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Methods
        // ********************************************************************
        public static void PlayOneShot(AudioInfoFM audioInfo, Transform parent = null)
        {
            if (audioInfo.eventRef.IsNull)
                return;

            if (parent == null)
                RuntimeManager.PlayOneShot(audioInfo.eventRef);
            else
                RuntimeManager.PlayOneShotAttached(audioInfo.eventRef, parent.gameObject);
        }
        // ********************************************************************
        public static void PlayOneShotAtLocation(AudioInfoFM audioInfo, Vector3 location)
        {
            if (audioInfo.eventRef.IsNull)
                return;

            RuntimeManager.PlayOneShot(audioInfo.eventRef, location);
        }
        // ********************************************************************
        public static StudioEventEmitter CreateAndPlayEmitter(AudioInfoFM audioInfo, Transform parent = null)
        {
            if (audioInfo.eventRef.IsNull)
                return null;

            if (!instance.m_objectPools.ContainsKey(audioInfo.eventRef))
            {
                instance.m_objectPools[audioInfo.eventRef] = new ObjectPool(instance.m_emitterPrefab.gameObject);
            }

            GameObject spawnedObject = instance.m_objectPools[audioInfo.eventRef].RequestObject(parent);
            spawnedObject.transform.localPosition = Vector3.zero;

            StudioEventEmitter emitter = spawnedObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = audioInfo.eventRef;
            emitter.Play();

            return emitter;
        }
        // ********************************************************************
        public static StudioEventEmitter CreateAndPlayEmitterAtLocation(AudioInfoFM audioInfo, Vector3 location, Transform parent = null)
        {
            if (audioInfo.eventRef.IsNull)
                return null;

            StudioEventEmitter emitter = CreateAndPlayEmitter(audioInfo, parent);
            emitter.transform.position = location;
            return emitter;
        }
        // ********************************************************************
        public static void PlayMusic(AudioInfoFM audioInfo)
        {
            if (audioInfo.eventRef.IsNull)
                return;

            // Fade out existing music
            if (instance.m_currentMusic != null)
            {
                instance.m_currentMusic.Stop(); // Music should all have a fade out / fade in as appropriate, not need for waiting for fade out.

                instance.m_currentMusic.gameObject.SetActive(false);
            }

            // New music
            instance.m_currentMusic = CreateAndPlayEmitter(audioInfo);
        }
        // ********************************************************************
        public static StudioEventEmitter GetMusic()
        {
            return instance.m_currentMusic;
        }
        // ********************************************************************
        public static void PauseMusic()
        {
            if (instance.m_currentMusic != null)
                instance.m_currentMusic.EventInstance.setPaused(true);
        }
        // ********************************************************************
        public static void ResumeMusic()
        {
            if (instance.m_currentMusic != null)
                instance.m_currentMusic.EventInstance.setPaused(false);
        }
        // ********************************************************************
        public static void StopMusic()
        {
            if (instance.m_currentMusic != null)
                instance.m_currentMusic.Stop();
        }
        #endregion
        // ********************************************************************


    }
    #endregion
    // ************************************************************************


}