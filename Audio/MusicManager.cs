// ************************************************************************ 
// File Name:   MusicManager.cs 
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
using BounderFramework;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 


// ************************************************************************ 
// Class: MusicManager
// ************************************************************************ 
public class MusicManager : Singleton<MusicManager> {


    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	[SerializeField]
	private FadeAudio m_fadeAudio;


	// ********************************************************************
	public static void FadeOut() { instance.m_fadeAudio.FadeOut(); }
	// ********************************************************************


	// ********************************************************************
	public static void FadeIn() { instance.m_fadeAudio.FadeIn(); }
	// ********************************************************************
	
	
	// ********************************************************************
	public static void PlayMusic(string clipResourceLocation) {
		AudioClip clip = (AudioClip) Resources.Load(clipResourceLocation);
		PlayMusic(clip);
	}
	// ********************************************************************


    // ********************************************************************
	public static IEnumerator PlayMusic(AudioClip clip) {
		Debug.Log ("Queuing clip "+clip.name);
		// fade current audio out if clip does not match, then when faded out, switch clips and fade back in
		if (instance.m_fadeAudio.audioSource.clip != clip)
		{
			yield return instance.m_fadeAudio.FadeOut();

			Debug.Log ("Playing clip "+clip.name);
			instance.m_fadeAudio.audioSource.clip = clip;
			instance.m_fadeAudio.audioSource.Play();
			instance.m_fadeAudio.FadeIn();
		}
	}
	// ********************************************************************


}