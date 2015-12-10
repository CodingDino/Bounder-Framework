// ************************************************************************ 
// File Name:   SoundInfo.cs 
// Purpose:    	
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 
[System.Serializable]


// ************************************************************************ 
// Class: SoundInfo
// ************************************************************************ 
public class SoundInfo {

	public AudioSource audioSource;
	public AudioClip audioClip;
	public bool shouldLoop = false;
	public Vector2 pitchRange = Vector2.one;
	public float volume = 1.0f;

	public void ApplyToSourceAndPlay()
	{
		ApplyToSourceAndPlay(audioSource);
	}
	public void ApplyToSourceAndPlay(AudioSource source)
	{
		if (source != null)
		{
			ApplyToSource(source);
			source.Play();
		}
	}
	public void ApplyToSource()
	{
		ApplyToSource(audioSource);
	}
	public void ApplyToSource(AudioSource source)
	{
		if (source != null)
		{
			source.clip = audioClip;
			source.pitch = Random.Range(pitchRange.x, pitchRange.y);
			source.volume = volume;
			source.loop = shouldLoop;
		}
	}

}