﻿// ************************************************************************ 
// File Name:   LoadingSceneManager.cs 
// Purpose:    	Handles loading and unloading scenes through an animated 
//				loading scene
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{


    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using TMPro;
    #endregion
    // ************************************************************************


    // ************************************************************************ 
    #region Enum: AudioCategory
    // ************************************************************************ 
    public enum LoadingState
    {
        INVALID = -1,
        // ---
        COVERING_SCREEN = 0,
        CLOSING_PANELS,
        OPENING_LOADING_SCREEN,
        REVEALING_LOADING_SCREEN,
        UNLOADING_OLD_SCENE,
        UNLOADING_ASSETS,
        SAVING_GAME,
        GAME_SAVED,
        LOADING_NEW_SCENE,
        PROCESSING_INCREMENTAL_LOADERS,
        NEW_SCENE_LOADED,
        HIDING_LOADING_SCREEN,
        UNLOADING_LOADING_SCREEN,
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
        private string m_loadingScene = "";
        [SerializeField]
        private string m_loadingSceneSave = "";
        [SerializeField]
        private string m_saveTextObjectName = "";
        [SerializeField]
        private float m_minDuration = 1.5f;
        [SerializeField]
        private float m_saveDisplayDuration = 1.0f;
        [SerializeField]
        private CanvasGroup m_screenCover;
        [SerializeField]
        private float m_screenCoverFadeTime = 0.2f;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Data Members
        // ********************************************************************
        private List<IncrementalLoader> m_loaders = new List<IncrementalLoader>();
        private bool m_loading = false;
        private List<string> m_sceneHistory = new List<string>();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Properties
        // ********************************************************************
        public static bool loading { get { return instance == null ? false : instance.m_loading; } }
        public static List<string> sceneHistory { get { return instance.m_sceneHistory.Copy(); } }
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
        #region Public Methods
        // ********************************************************************
        public static bool LoadScene(int _sceneIndex, bool _save = false)
        {
            if (!instance.m_loading)
            {
                instance.StartCoroutine(instance.CR_LoadScene("", _sceneIndex, _save));
                return true;
            }
            return false;
        }
        public static bool LoadScene(string _newScene, bool _save = false)
        {
            if (!instance.m_loading)
            {
                instance.StartCoroutine(instance.CR_LoadScene(_newScene, 0, _save));
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
                //			Debug.Log("Adding IncrementalLoader: "+_loader);
                instance.m_loaders.Add(_loader);
            }
            else
            {
                //			Debug.Log("Removing IncrementalLoader: "+_loader);
                instance.m_loaders.Remove(_loader);
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Methods
        // ********************************************************************
        public IEnumerator CR_LoadScene(string _newScene, int _buildIndex = 0, bool _save = false)
        {
            Debug.Log("Loading Scene: " + _newScene);
            m_loading = true;

            string oldScene = SceneManager.GetActiveScene().name;
            m_sceneHistory.Add(oldScene);

            // COVERING_SCREEN
            {
                //Debug.Log("Loading Scene state: " + LoadingState.COVERING_SCREEN);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.COVERING_SCREEN, _newScene, oldScene);

                // Fade to black
                m_screenCover.blocksRaycasts = true;
                LeanTween.alphaCanvas(m_screenCover, 1.0f, m_screenCoverFadeTime).setIgnoreTimeScale(true);
                yield return new WaitForSecondsRealtime(m_screenCoverFadeTime);
            }

            // CLOSING_PANELS
            {
                //Debug.Log("Loading Scene state: " + LoadingState.CLOSING_PANELS);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.CLOSING_PANELS, _newScene, oldScene);

                PanelManager.CloseAllPanels();
                while (PanelManager.NumPanelsOpen() > 0)
                    yield return null;
            }

            // OPENING_LOADING_SCREEN
            {
                //Debug.Log("Loading Scene state: " + LoadingState.OPENING_LOADING_SCREEN);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.OPENING_LOADING_SCREEN, _newScene, oldScene);

                // Load loading screen
                yield return SceneManager.LoadSceneAsync(_save ? m_loadingSceneSave : m_loadingScene, LoadSceneMode.Additive);

                if (_save)
                {
                    // Inform player of save
                    TMP_Text saveText = GameObject.Find(m_saveTextObjectName).GetComponent<TMP_Text>();
                    saveText.text = "Saving...";
                }
            }

            // HIDING_SCREEN_COVER
            {
                //Debug.Log("Loading Scene state: " + LoadingState.HIDING_SCREEN_COVER);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.REVEALING_LOADING_SCREEN, _newScene, oldScene);

                // Fade to loading screen
                LeanTween.alphaCanvas(m_screenCover, 0.0f, m_screenCoverFadeTime).setIgnoreTimeScale(true);
                yield return new WaitForSecondsRealtime(m_screenCoverFadeTime);
                m_screenCover.blocksRaycasts = false;
            }

            // UNLOADING_OLD_SCENE
            {
                //Debug.Log("Loading Scene state: " + LoadingState.UNLOADING_OLD_SCENE);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.UNLOADING_OLD_SCENE, _newScene, oldScene);

                // !!! unload old screen
                yield return SceneManager.UnloadSceneAsync(oldScene);
            }

            // UNLOADING_ASSETS
            {
                //Debug.Log("Loading Scene state: " + LoadingState.UNLOADING_ASSETS);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.UNLOADING_ASSETS, _newScene, oldScene);

                // Clear databses to allow asset unloading
                Events.Raise(new ClearDatabaseForSceneChangeEvent());
            }

            // SAVING_GAME
            if (_save)
            {
                //Debug.Log("Loading Scene state: " + LoadingState.SAVING_GAME);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.SAVING_GAME, _newScene, oldScene);

                // Figure out how long to wait
                float saveEndTime = Time.realtimeSinceStartup + m_saveDisplayDuration;

                // Triggering save
                ProfileManager.Save();

                // Wait a minimum time for player to see message
                while (Time.realtimeSinceStartup < saveEndTime)
                    yield return null;
            }

            // GAME_SAVED
            if (_save)
            {
                //Debug.Log("Loading Scene state: " + LoadingState.GAME_SAVED);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.GAME_SAVED, _newScene, oldScene);

                // Inform player of save complete
                TMP_Text saveText = GameObject.Find(m_saveTextObjectName).GetComponent<TMP_Text>();
                saveText.text = "Game saved!";

                // Wait a minimum time for player to see message
                yield return new WaitForSecondsRealtime(m_saveDisplayDuration);
            }

            float endTime = Time.time + m_minDuration;

            // LOADING_NEW_SCENE
            {
                //Debug.Log("Loading Scene state: " + LoadingState.LOADING_NEW_SCENE);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.LOADING_NEW_SCENE, _newScene, oldScene);

                // Load level async
                if (_newScene.NullOrEmpty())
                    yield return SceneManager.LoadSceneAsync(_buildIndex, LoadSceneMode.Additive);
                else
                    yield return SceneManager.LoadSceneAsync(_newScene, LoadSceneMode.Additive);

                if (_newScene.NullOrEmpty())
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_buildIndex));
                else
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(_newScene));
            }

            // PROCESSING_INCREMENTAL_LOADERS
            {
                //Debug.Log("Loading Scene state: " + LoadingState.PROCESSING_INCREMENTAL_LOADERS);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.PROCESSING_INCREMENTAL_LOADERS, _newScene, oldScene);

                while (m_loaders.Count > 0)
                    yield return null;
            }


            // NEW_SCENE_LOADED
            {
                //Debug.Log("Loading Scene state: " + LoadingState.NEW_SCENE_LOADED);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.NEW_SCENE_LOADED, _newScene, oldScene);

                while (Time.time < endTime)
                    yield return null;
            }

            // HIDING_LOADING_SCREEN
            {
                //Debug.Log("Loading Scene state: " + LoadingState.HIDING_LOADING_SCREEN);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.HIDING_LOADING_SCREEN, _newScene, oldScene);

                // Fade to black
                m_screenCover.blocksRaycasts = true;
                LeanTween.alphaCanvas(m_screenCover, 1.0f, m_screenCoverFadeTime).setIgnoreTimeScale(true);
                yield return new WaitForSecondsRealtime(m_screenCoverFadeTime);
            }

            // UNLOADING_LOADING_SCREEN
            {
                //Debug.Log("Loading Scene state: " + LoadingState.UNLOADING_LOADING_SCREEN);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.UNLOADING_LOADING_SCREEN, _newScene, oldScene);

                // !!! unload loading screen
                yield return SceneManager.UnloadSceneAsync(_save ? m_loadingSceneSave : m_loadingScene);
            }

            // REVEALING_NEW_SCENE
            {
                //Debug.Log("Loading Scene state: " + LoadingState.REVEALING_NEW_SCENE);
                if (OnStateChanged != null)
                    OnStateChanged(LoadingState.REVEALING_NEW_SCENE, _newScene, oldScene);

                // Fade to new screen
                LeanTween.alphaCanvas(m_screenCover, 0.0f, m_screenCoverFadeTime).setIgnoreTimeScale(true);
                yield return new WaitForSecondsRealtime(m_screenCoverFadeTime);
                m_screenCover.blocksRaycasts = false;
            }

            // LOADING_COMPLETE
            {
                //Debug.Log("Loading Scene state: " + LoadingState.LOADING_COMPLETE);
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