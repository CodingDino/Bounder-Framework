// ************************************************************************ 
// File Name:   LoadSceneCondition.cs 
// Purpose:    	A Condition for checking whether a scene is loaded
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: LoadSceneCondition
// ************************************************************************
[CreateAssetMenu(fileName = "LoadSceneCondition", menuName = "Conditions/LoadSceneCondition", order = 1)]
public class LoadSceneCondition : Condition 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[SerializeField]
	private string m_sceneID;
	[SerializeField]
	private bool m_invert = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Functions 
	// ********************************************************************
	public override void Register(bool _register) 
	{
		if (_register)
			LoadingSceneManager.OnStateChanged += OnLoadingStateChanged;
		else
			LoadingSceneManager.OnStateChanged -= OnLoadingStateChanged;
	}
	// ********************************************************************
	public override int GetProgress_Cumulative() 
	{
		bool active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == m_sceneID;
		return active != m_invert ? 1 : 0;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Functions 
	// ********************************************************************
	private void OnLoadingStateChanged(LoadingState _state, string _newScene, string _oldScene)
	{
		if (_state == LoadingState.LOADING_COMPLETE && _newScene == m_sceneID && !m_invert)
		{
			AddProgress();
			Trigger();
		}
		else if (_state == LoadingState.LOADING_COMPLETE && _oldScene == m_sceneID && m_invert)
		{
			AddProgress();
			Trigger();
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
