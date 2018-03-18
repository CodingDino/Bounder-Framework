// ************************************************************************ 
// File Name:   ButtonQuitGame.cs 
// Purpose:    	Quits game on button press
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ButtonQuitGame
// ************************************************************************
public class ButtonQuitGame : MonoBehaviour 
{

	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public void Quit()
	{ 
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		Application.OpenURL(webplayerQuitURL);
		#else
		Application.Quit();
		#endif
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************
