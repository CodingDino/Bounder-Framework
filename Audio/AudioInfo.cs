// ************************************************************************ 
// File Name:   AudioInfo.cs 
// Purpose:    	Holds information used for playing audio
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework 
{ 
	// ******************************************************************** 
	#region Class: AudioInfo
	// ********************************************************************
	[System.Serializable]
	public class AudioInfo : Archive 
	{
		
		// ****************************************************************
		#region Public Data Members
		// ****************************************************************
		public AudioCategory category = AudioCategory.EFFECTS;
		public AudioChannelOverride overrideChannelLimit = AudioChannelOverride.NONE;
		public float volume = 1.0f;
		public float volumeFuzz = 0.0f;
		public float pitch = 1.0f;
		public float pitchFuzz = 0.0f;
		public bool loop = false;
		public float fadeDuration = 0.0f;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Archive Methods
		// ****************************************************************
		public virtual bool Load(JSON _JSON)
		{
			bool success = true;

			success &= _JSON["category"].GetEnum<AudioCategory>(ref category);
			success &= _JSON["override"].GetEnum<AudioChannelOverride>(ref overrideChannelLimit);
			success &= _JSON["volume"].Get(ref volume);
			success &= _JSON["volumeFuzz"].Get(ref volumeFuzz);
			success &= _JSON["pitch"].Get(ref pitch);
			success &= _JSON["pitchFuzz"].Get(ref pitchFuzz);
			success &= _JSON["loop"].Get(ref loop);
			success &= _JSON["fadeDuration"].Get(ref fadeDuration);

			return success;
		}
		// ****************************************************************
		public virtual JSON Save()
		{
			JSON save = new JSON();

			save["category"].data = category.ToString();
			save["override"].data = overrideChannelLimit.ToString();
			save["volume"].data = volume;
			save["volumeFuzz"].data = volumeFuzz;
			save["pitch"].data = pitch;
			save["pitchFuzz"].data = pitchFuzz;
			save["shouldLoop"].data = loop;
			save["fadeDuration"].data = fadeDuration;

			return save;
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Public Methods
		// ****************************************************************
		public AudioInfo() { }
		// ****************************************************************
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
		// ****************************************************************
		#endregion
		// ****************************************************************

	}
	#endregion
	// ********************************************************************
}
// ************************************************************************