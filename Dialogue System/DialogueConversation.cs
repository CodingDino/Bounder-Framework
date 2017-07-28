// ************************************************************************ 
// File Name:   DialogueConversation.cs 
// Purpose:    	Information about a conversation for the dialogue system
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System;
#endif


// ************************************************************************ 
// Class: DialogueConversation
// ************************************************************************ 
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Dialogue/DialogueConversation", order = 1)]
public class DialogueConversation : ScriptableObject 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
    // Defaults (Can be overridden by frames and sections)
    public bool allowSkip = true;
    public bool waitForInput = true;
	
	// Requirements
	public List<Condition> conditions = new List<Condition>();

    // Frames
	public List<DialogueFrame> frames = new List<DialogueFrame>();
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
    public bool MeetsConditions()
    {
		if (conditions == null)
			return true;
		
        for (int i = 0; i < conditions.Count; ++i)
        {
			if (conditions[i] == null || !conditions[i].Evaluate())
                return false;
        }
        return true;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#if UNITY_EDITOR
	// ********************************************************************
	public void AddNewFrameAt(int _pos)
	{
		DialogueFrame newFrame = CreateInstance<DialogueFrame>();
		newFrame.allowSkip = allowSkip;
		newFrame.waitForInput = waitForInput;
		newFrame.id = name.Replace("DC-","")+"-"+_pos.ToString();
		AddFrameAt(newFrame,_pos);
	}
	// ********************************************************************
	public void AddFrameAt(DialogueFrame _frame, int _pos)
	{
		// Add cue
		frames.Insert(_pos,_frame);
		_frame.conversation = this;
		_frame.expanded = true;

		// Set dirty
		EditorUtility.SetDirty(this);

		// Create the asset and add it to the project
		string parentFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
		string assetPath = parentFolder+"/Frames";
		if (!AssetDatabase.IsValidFolder(assetPath))
		{
			string newFolderID = AssetDatabase.CreateFolder(parentFolder,"Frames");
			assetPath = AssetDatabase.GUIDToAssetPath(newFolderID);
		}
		string assetPathAndName = assetPath + "/DF-"+_frame.id+".asset";
		string uniqueAssetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPathAndName);
		AssetDatabase.CreateAsset(_frame,uniqueAssetPathAndName);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
	// ********************************************************************
	public void RemoveFrame(DialogueFrame _frame)
	{
		frames.Remove(_frame);

		// Set dirty
		EditorUtility.SetDirty(this);

		string pathToDelete = AssetDatabase.GetAssetPath(_frame);
		AssetDatabase.DeleteAsset(pathToDelete);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
	// ********************************************************************
	public void MoveFrame(DialogueFrame _frame, int _move)
	{
		MoveFrameTo(_frame, frames.IndexOf(_frame) + _move);
	}
	// ********************************************************************
	public void MoveFrameTo(DialogueFrame _frame, int _index)
	{
		if (_index < 0 || _index >= frames.Count)
			return;
		frames.Remove(_frame);
		frames.Insert(_index,_frame);

		// Set dirty
		EditorUtility.SetDirty(this);
	}
	// ********************************************************************
	[CustomEditor(typeof(DialogueConversation))]
	public class Inspector : Editor
	{
		// ****************************************************************
		#region Editor Methods 
		// ****************************************************************
		public override void OnInspectorGUI()
		{
			DialogueConversation conversation = (DialogueConversation)target;
			SerializedObject serialized = new SerializedObject(conversation);
			serialized.Update();

			GUIStyle style;
			Color oldColor;

			// default - just draw objects
			SerializedProperty property = serialized.GetIterator();
			property.NextVisible(true);
			do
			{
				if (property.name == "frames")
				{
					for (int i = 0; i < conversation.frames.Count; ++i)
					{
						if (conversation.frames[i] != null)
						{
							conversation.frames[i].DrawUI();
						}
					}
				}
				else
				{
					EditorGUILayout.PropertyField(property, !property.hasVisibleChildren || property.isExpanded);
				}

			} while (property.NextVisible(false));

			style = new GUIStyle();
			style.alignment = TextAnchor.MiddleRight;
			EditorGUILayout.BeginHorizontal(style);
			GUILayout.FlexibleSpace();
			oldColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("+",GUILayout.Width(30)))
			{
				conversation.AddNewFrameAt(conversation.frames.Count);
			}
			GUI.backgroundColor = oldColor;
			EditorGUILayout.EndHorizontal();

			serialized.ApplyModifiedProperties();
   		}
		// ****************************************************************
		#endregion
		// ****************************************************************
	}
	// ********************************************************************
	#endif
	// ********************************************************************

}
