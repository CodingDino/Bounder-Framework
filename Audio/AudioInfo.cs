// ************************************************************************ 
// File Name:   AudioInfo.cs 
// Purpose:    	Holds information used for playing audio
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{


    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using UnityEngine;
    #endregion
    // ************************************************************************



    // ************************************************************************
    #region Class: AudioInfo
    // ************************************************************************
    [System.Serializable]
    public class AudioInfo
    {

        // ********************************************************************
        #region Public Data Members
        // ********************************************************************
        public string id = "";
        public AudioClip clip = null;
        public AudioCategory category = AudioCategory.EFFECTS;
        public AudioChannelOverride overrideChannelLimit = AudioChannelOverride.NONE;
        public float volume = 1.0f;
        public float volumeFuzz = 0.0f;
        public float pitch = 1.0f;
        public float pitchFuzz = 0.0f;
        public bool loop = false;
        public float fadeDuration = 0.0f;
        public Transform parent = null;
        public int allowedDuplicates = 3;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Methods
        // ********************************************************************
        public AudioInfo() { }
        // ********************************************************************
        public AudioInfo(AudioClip _clip)
        {
            clip = _clip;
        }
        // ********************************************************************
        public AudioInfo(AudioCategory _category)
        {
            category = _category;
            if (_category == AudioCategory.MUSIC)
            {
                overrideChannelLimit = AudioChannelOverride.REPLACE;
                fadeDuration = 1.0f;
                loop = true;
            }
        }
        // ********************************************************************
        public AudioInfo(AudioClip _clip, AudioCategory _category)
        {
            clip = _clip;
            category = _category;
            if (_category == AudioCategory.MUSIC)
            {
                overrideChannelLimit = AudioChannelOverride.REPLACE;
                fadeDuration = 1.0f;
                loop = true;
            }
        }
        // ********************************************************************
        public string GetID()
        {
            return id.NullOrEmpty() ? clip.name : id;
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
    #endregion
    // ************************************************************************

}
// ************************************************************************