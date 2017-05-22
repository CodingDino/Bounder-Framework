// ************************************************************************ 
// File Name:   LoadingSceneManager.cs 
// Purpose:    	Handles loading and unloading scenes through an animated 
//				loading scene
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;
using UnityEngine.SceneManagement;
using AssetBundles;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework 
{ 

// ************************************************************************ 
#region Enum: AudioCategory
// ************************************************************************ 
public enum LoadingState
{
	INVALID = -1,
	// ---
	COVERING_SCREEN = 0,
	SCREEN_COVERED,
	LOADING_NEW_SCENE,
	PROCESSING_INCREMENTAL_LOADERS,
	NEW_SCENE_LOADED,
	REVEALING_NEW_SCENE,
	LOADING_COMPLETE,
	// ---
	NUM
}
#endregion
// ************************************************************************ 

// ************************************************************************ 
#region Class: LoadingSceneManager
// ************************************************************************ 
public class LoadingSceneManager : Singleton<LoadingSceneManager> 
{


	// ********************************************************************
	#region Exposed Data Members
	// ********************************************************************
	[SerializeField]
	private bool m_loadOnStart = true;
	[SerializeField]
	private string m_levelToLoad = "";
	[SerializeField]
	private string m_loadingScene = "";
	[SerializeField]
	private string m_loadingSceneBundle = "";
	[SerializeField]
	private FadeSprite m_blackness;
	[SerializeField]
	private float m_minDuration = 1.5f;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members
	// ********************************************************************
	private List<IncrementalLoader> m_loaders = new List<IncrementalLoader>();
	private bool m_loading = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Events
	// ********************************************************************
	public delegate void StateChanged(LoadingState _state, string _newScene, string _oldScene);
	public static event StateChanged OnStateChanged;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Monobehavior Methods
	// ********************************************************************
	void Start()
	{
		if (m_loadOnStart) 
		{
			SceneManager.LoadScene(m_levelToLoad);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public static bool LoadScene(string _newScene, string _bundle = "scenes")
	{
		if (!instance.m_loading)
			{
				instance.StartCoroutine(instance.CR_LoadScene(_newScene, _bundle));
				return true;
			}
			return false;
	}
	// ********************************************************************
	public static void RegisterLoader(IncrementalLoader _loader, bool _register = true)
	{
		if (instance == null)
			return;
		
		if (_register)
			{
				Debug.Log("Adding IncrementalLoader: "+_loader);
				instance.m_loaders.Add(_loader);
			}
		else
			{
				Debug.Log("Removing IncrementalLoader: "+_loader);
				instance.m_loaders.Remove(_loader);
			}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods
	// ********************************************************************
		public IEnumerator CR_LoadScene(string _newScene, string _assetBundle)
	{
		Debug.Log("Loading Scene: "+_newScene);
		m_loading = true;

		string oldScene = SceneManager.GetActiveScene().name;

		// COVERING_SCREEN
		{
				Debug.Log("Loading Scene state: "+LoadingState.COVERING_SCREEN);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.COVERING_SCREEN, _newScene, oldScene);

			// Fade to black
			yield return m_blackness.FadeIn();

			// Load loading screen
			AssetBundleLoadOperation loadOp = AssetBundleManager.LoadLevelAsync(m_loadingSceneBundle.ToLower(), m_loadingScene, false);
			yield return StartCoroutine(loadOp);
			// !!! unload old screen (automatic)
		}

		// SCREEN_COVERED
			{
				Debug.Log("Loading Scene state: "+LoadingState.SCREEN_COVERED);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.SCREEN_COVERED, _newScene, oldScene);

			// Fade to loading screen
			yield return m_blackness.FadeOut();
		}

		float endTime = Time.time + m_minDuration;

		// LOADING_NEW_SCENE
			{
				Debug.Log("Loading Scene state: "+LoadingState.LOADING_NEW_SCENE);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.LOADING_NEW_SCENE, _newScene, oldScene);

			// Load level async
				AssetBundleLoadOperation loadOp = AssetBundleManager.LoadLevelAsync(_assetBundle, _newScene, true);
			yield return StartCoroutine(loadOp);
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(_newScene));
			// TODO: Unload inactive asset bundles to free up memory
		}

		// PROCESSING_INCREMENTAL_LOADERS
			{
				Debug.Log("Loading Scene state: "+LoadingState.PROCESSING_INCREMENTAL_LOADERS);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.PROCESSING_INCREMENTAL_LOADERS, _newScene, oldScene);

			while (m_loaders.Count > 0)
				yield return null;
		}


		// NEW_SCENE_LOADED
			{
				Debug.Log("Loading Scene state: "+LoadingState.NEW_SCENE_LOADED);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.NEW_SCENE_LOADED, _newScene, oldScene);

			while (Time.time < endTime)
				yield return null;
		}

		// REVEALING_NEW_SCENE
			{
				Debug.Log("Loading Scene state: "+LoadingState.REVEALING_NEW_SCENE);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.REVEALING_NEW_SCENE, _newScene, oldScene);

			// Fade to black
			yield return m_blackness.FadeIn();

			// !!! unload loading screen
			yield return SceneManager.UnloadSceneAsync(m_loadingScene);

			// Fade to new screen
			yield return m_blackness.FadeOut();
		}

		// LOADING_COMPLETE
			{
				Debug.Log("Loading Scene state: "+LoadingState.LOADING_COMPLETE);
			if (OnStateChanged != null) 
				OnStateChanged(LoadingState.LOADING_COMPLETE, _newScene, oldScene);
		}

		m_loading = false;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}