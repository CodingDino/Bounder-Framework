// ************************************************************************ 
// File Name:   DialogueLink.cs 
// Purpose:    	Information about how two frames link together
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
// Class: DialogueLink
// ************************************************************************ 
[System.Serializable]
public class DialogueLink 
{
	public DialogueFrame linkedFrame;
	public List<Condition> conditions = new List<Condition>();
    public string text;
	public Sprite icon = null;
	public RuntimeAnimatorController animation = null;
	public bool saveChoice;

    public bool MeetsConditions()
    {
		if (conditions == null || conditions.Count == 0) return true;
        for (int i = 0; i < conditions.Count; ++i)
        {
			if (!conditions[i].Evaluate())
                return false;
        }
        return true;
    }
}
