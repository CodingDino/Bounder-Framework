// ************************************************************************ 
// File Name:   DialogueFrame.cs 
// Purpose:    	Information about a frame of dialogue
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections.Generic;
using Bounder.Framework;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


// ************************************************************************ 
// Class: DialogueFrame
// ************************************************************************ 
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Bounder/Dialogue/DialogueFrame", order = 1)]
public class DialogueFrame : ScriptableObject 
{
    public string id;

    // Linking
    public bool endOnThisFrame = false;
    public bool displayChoices = false;
	public List<DialogueLink> links = new List<DialogueLink>();

    // Overrides (will be set to parent values if no override present) 
    public bool allowSkip = true;
    public bool waitForInput = true;
	public DialogueCharacter character;

	// Portrait side
	public enum PortraitSide { LEFT, RIGHT }
	public PortraitSide portraitSide = PortraitSide.LEFT;

    // Sections
	public List<DialogueSection> sections = new List<DialogueSection>();


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	[SerializeField] [HideInInspector]
	public DialogueConversation conversation = null;
	#if UNITY_EDITOR
	[HideInInspector]
	public bool expanded = false;
	#endif
	#endregion
	// ********************************************************************


	// ********************************************************************
	#if UNITY_EDITOR
	// ********************************************************************
	public void DrawUI()
	{
		SerializedObject serialized = new SerializedObject(this);
		serialized.Update();
		string oldID = id;

		Rect outerRect = EditorGUILayout.BeginVertical("Box");
		EditorGUI.DrawRect(outerRect,character == null ? Color.white : character.editorColor);
		// create delete button for child
		{
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleRight;
			EditorGUILayout.BeginHorizontal(style);
			if (GUILayout.Button(expanded ? "-" : "+",GUILayout.Width(25)))
			{
				expanded = !expanded;
			}
			EditorGUILayout.LabelField(id,EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			Color oldColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("+",GUILayout.Width(25)))
			{
				conversation.AddNewFrameAt(conversation.frames.IndexOf(this)+1);
			}
			if (GUILayout.Button("=",GUILayout.Width(25)))
			{
				DialogueFrame newFrame = CreateInstance<DialogueFrame>();
				EditorUtility.CopySerialized(this,newFrame);
				conversation.AddFrameAt(newFrame,conversation.frames.IndexOf(this)+1);
			}
			GUI.backgroundColor = oldColor;
			if (GUILayout.Button("/\\",GUILayout.Width(25)))
			{
				conversation.MoveFrame(this,-1);
			}
			if (GUILayout.Button("\\/",GUILayout.Width(25)))
			{
				conversation.MoveFrame(this,+1);
			}
			if (GUILayout.Button("/|\\",GUILayout.Width(25)))
			{
				conversation.MoveFrameTo(this,0);
			}
			if (GUILayout.Button("\\|/",GUILayout.Width(25)))
			{
				conversation.MoveFrameTo(this,conversation.frames.Count-1);
			}
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("X",GUILayout.Width(25)))
			{
				conversation.RemoveFrame(this);
				return;
			}
			GUI.backgroundColor = oldColor;
			EditorGUILayout.EndHorizontal();
		}
		++EditorGUI.indentLevel;
		if (expanded)
		{

			SerializedProperty property = serialized.GetIterator();
			property.NextVisible(true);
			do
			{
				EditorGUILayout.PropertyField(property, !property.hasVisibleChildren || property.isExpanded);
			} while (property.NextVisible(false));
		}
		--EditorGUI.indentLevel;
		EditorGUILayout.EndVertical();

		serialized.ApplyModifiedProperties();

		// If id changed, change file name
		if (id != oldID)
		{
			string originalName = AssetDatabase.GetAssetPath(this);
			string parentFolder = Path.GetDirectoryName(originalName);
			string newName = "DF-"+id;
			string newNamePath = parentFolder + "/"+ newName + ".asset";
			string uniqueNewNamePath = AssetDatabase.GenerateUniqueAssetPath(newNamePath);
			string uniqueNewName = Path.GetFileNameWithoutExtension(uniqueNewNamePath);
			if (uniqueNewName != originalName)
			{
				string result = AssetDatabase.RenameAsset(originalName,uniqueNewName);
				if (!result.NullOrEmpty())
					Debug.LogError("Error renaming asset: "+result);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}
	}
	// ********************************************************************
	#endif
	// ********************************************************************
}
