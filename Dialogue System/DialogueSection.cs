// ************************************************************************ 
// File Name:   DialogueSection.cs 
// Purpose:    	Information about a section of dialogue
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using BounderFramework;
#if UNITY_EDITOR
using UnityEditor;
#endif


// ************************************************************************ 
// Class: DialogueSection
// ************************************************************************ 
[System.Serializable]
public class DialogueSection 
{
    // Overrides
	public DialogueTextSettings textSettings;

	// Animations and Effects
	// TODO: Can we make this not need an enum but still have error checking?
	public enum PortraitEmotion { 
		NEUTRAL,	// = 0
		ANGRY,		// = 1
		SAD,		// = 2
		HAPPY 		// = 3
	}
	public PortraitEmotion emotion = PortraitEmotion.NEUTRAL;
    public string triggerAnimation;
    public string triggerEffect;

	// Text
	[TextArea(3,10)]
	public string text;
}
//
//
//[CustomPropertyDrawer(typeof(DialogueSection))]
//public class IngredientDrawer : PropertyDrawer
//{
//	// Draw the property inside the given rect
//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//	{
//		// Using BeginProperty / EndProperty on the parent property means that
//		// prefab override logic works on the entire property.
//		EditorGUI.BeginProperty(position, label, property);
//
//		// Draw label
//		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//
//		property.NextVisible(true);
//		do
//		{
//			if (property.name == "")
//			{
//				
//			}
//			else
//			{
//				EditorGUILayout.PropertyField(property, !property.hasVisibleChildren || property.isExpanded);
//			}
//		} while (property.NextVisible(false));
//
//		EditorGUI.EndProperty();
//	}
//}