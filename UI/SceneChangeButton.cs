﻿// ************************************************************************ 
// File Name:   SceneChangeButton.cs 
// Purpose:    	Change scene on button press
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: SceneChangeButton
// ************************************************************************
public class SceneChangeButton : MonoBehaviour 
{
	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ChangeScene (string _scene) 
	{
		LoadingSceneManager.LoadScene(_scene);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************
