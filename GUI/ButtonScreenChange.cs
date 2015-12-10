// ************************************************************************ 
// File Name:   ButtonScreenChange.cs 
// Purpose:     Button that changes between screens  
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: ButtonScreenChange 
// ************************************************************************ 
public class ButtonScreenChange : ButtonImplementation 
{
	
    // ********************************************************************
    // Exposed Data Members 
    // ********************************************************************
	public string m_targetScene;
	
	
    // ********************************************************************
    // Function:	OnClick()
    // Purpose:		Called when the button is clicked
    // ********************************************************************
	public override void OnClick()
	{		
		Debug.Log("Loading scene: "+m_targetScene);
		Application.LoadLevel(m_targetScene);
	}
	
}
