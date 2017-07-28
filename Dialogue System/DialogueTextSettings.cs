// ************************************************************************ 
// File Name:   DialogueTextSettings.cs 
// Purpose:    	Settings for text printout during a section of dialogue.
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using System.Collections.Generic;
using UnityEngine;


// ************************************************************************ 
// Class: DialogueTextSettings
// ************************************************************************
[System.Serializable]
public class DialogueTextSettings 
{
	[Tooltip("Multiplier to base text speed")]
	public float textSpeed = 1.0f;
	[Tooltip("Pitch multiplier")]
	public float textPitch = 1.0f;
	[Tooltip("Pitch variation (using perlin noise)")]
	public float textPitchVariation = 0.0f;
	[Tooltip("Audio clip used when printing text")]
	public AudioClip textAudio;
	[Tooltip("Maximum speed at which the audio will play, even if text speed is faster.")]
	public float textAudioMaxSpeed = 0.0f;

	[System.Serializable]
	public struct DialogueTextSymbolTime
	{
		public char symbol;
		public float time;

		public DialogueTextSymbolTime(char _symbol, float _time) 
		{
			symbol = _symbol;
			time = _time;
		}
	}

	[Tooltip("Text symbols with special printing durations, such as . or ! ")]
	public List<DialogueTextSymbolTime> textSymbolTime = new List<DialogueTextSymbolTime>();

	public void Merge(DialogueTextSettings _toMerge)
	{
		if (_toMerge.textSpeed != 0)
			textSpeed = _toMerge.textSpeed;
		if (_toMerge.textPitch != 0)
			textPitch = _toMerge.textPitch;
		if (_toMerge.textPitchVariation != 0)
			textPitchVariation = _toMerge.textPitchVariation;
		if (_toMerge.textAudio != null)
			textAudio = _toMerge.textAudio;
		if (_toMerge.textAudioMaxSpeed != 0)
			textAudioMaxSpeed = _toMerge.textAudioMaxSpeed;

		for (int i = 0; i < _toMerge.textSymbolTime.Count; ++i)
		{
			if (!textSymbolTime.Contains(_toMerge.textSymbolTime[i]))
				textSymbolTime.Add(_toMerge.textSymbolTime[i]);
		}
	}
}