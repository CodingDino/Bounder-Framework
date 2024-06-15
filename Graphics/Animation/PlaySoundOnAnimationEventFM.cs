// ************************************************************************ 
// File Name:   PlaySoundOnAnimationEventFM.cs 
// Purpose:    	Play a sound triggered by an animation event, using FMOD
// Project:		Bounder Framework
// Author:      Sarah Herzog  
// Copyright: 	2024 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{
    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using System.Collections.Generic;
    using UnityEngine;
    using Bounder.Framework;
    using FMODUnity;
    #endregion
    // ************************************************************************

    // ************************************************************************ 
    #region Class: PlaySoundOnAnimationEventFM
    // ************************************************************************
    public class PlaySoundOnAnimationEventFM : MonoBehaviour
    {
        // ********************************************************************
        #region Exposed Data Members 
        // ********************************************************************
        [SerializeField]
        [Tooltip("Audio info to play")]
        private List<AudioInfoFM> m_audioInfo = new List<AudioInfoFM>();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Data Members 
        // ********************************************************************
        private Dictionary<string, AudioInfoFM> m_soundMap = new ();
        private Dictionary<string, StudioEventEmitter> m_activeSounds = new ();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region MonoBehaviour Methods
        // ********************************************************************
        void Awake()
        {
            for (int i = 0; i < m_audioInfo.Count; ++i)
            {
                AudioInfoFM sound = m_audioInfo[i];
                string id = sound.GetID();
                if (m_soundMap.ContainsKey(id))
                    Debug.LogWarning("Duplicate ID found: " + id);
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
        private void PlaySound(string _id)
        {
            if (!m_soundMap.ContainsKey(_id))
            {
                Debug.LogWarning("No data found for ID: " + _id);
                return;
            }

            AudioInfoFM sound = m_soundMap[_id];

            StudioEventEmitter audio = AudioManagerFM.CreateAndPlayEmitter(sound);
            m_activeSounds[_id] = audio;
        }
        // ********************************************************************
        private void StopSound(string _id)
        {
            if (!m_soundMap.ContainsKey(_id))
            {
                Debug.LogWarning("No data found for ID: " + _id);
                return;
            }

            if (!m_activeSounds.ContainsKey(_id))
            {
                // NOTE: No error message as we just want this to be a no-op
                return;
            }

            StudioEventEmitter audio = m_activeSounds[_id];
            if (audio != null
                && audio.gameObject.activeSelf
                && audio.IsPlaying())
            {
                audio.Stop();
            }
            m_activeSounds.Remove(_id);
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
    #endregion
    // ************************************************************************
}
