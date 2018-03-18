// ************************************************************************ 
// File Name:   DialogueCharacter.cs 
// Purpose:    	Information about dialogue settings for a specific character
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
// Class: DialogueNPCSettings
// ************************************************************************ 
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Bounder/Dialogue/DialogueCharacter", order = 1)]
public class DialogueCharacter : ScriptableObject 
{
	public string displayName;
	public Animator portrait;
	public DialogueTextSettings textSettings;
	public Color editorColor = Color.white;
}
