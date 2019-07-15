// ************************************************************************ 
// File Name:   LogManager.cs
// Purpose:    	Controls logging settings for the game.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{


    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using UnityEngine;
    using System.Collections.Generic;
    #endregion
    // ************************************************************************


    // ********************************************************************
    #region Enum: LogCategory
    // ********************************************************************
    public enum LogCategory
    {
        UNCATEGORISED = 1 << 0,
        // ---
        DATA = 1 << 1,
        INPUT = 1 << 2,
        UI = 1 << 3,
        GAME_LOGIC = 1 << 4,
        THIRD_PARTY = 1 << 5,
        // ---
        NONE = 0,
        ALL = UNCATEGORISED | DATA | INPUT | UI | GAME_LOGIC | THIRD_PARTY
    }
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Enum: LogSeverity
    // ********************************************************************
    public enum LogSeverity
    {
        INVALID = -1,
        // ---
        SPAMMY_LOG = 0,
        LOG,
        WARNING,
        ERROR,
        FATAL_ERROR,
        // ---
        NUM
    }
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Class: LogCategorySettings
    // ********************************************************************
    [System.Serializable]
    public class LogCategorySettings
    {
        public bool enabled = true;
        public Color color = Color.black;
    }
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Class: LogManager
    // ********************************************************************
    public class LogManager : Singleton<LogManager>
    {
        // ****************************************************************
        #region Exposed Data Members
        // ****************************************************************
        [SerializeField]
        [Tooltip("Logs of this severity or higher will be displayed")]
        private LogSeverity m_weakestSeverity = LogSeverity.LOG;
        [SerializeField]
        [Tooltip("Logs will be restricted to these tags. If empty, no restriction will be applied.")]
        private List<string> m_tags = new List<string>();
        // ****************************************************************
        [Header("Categories")]
        [SerializeField]
        [Tooltip("Logs without a category")]
        private LogCategorySettings m_UNCATEGORISED = new LogCategorySettings();
        [SerializeField]
        [Tooltip("Logs relating to data loading or storage - archives and player profile.")]
        private LogCategorySettings m_DATA = new LogCategorySettings();
        [SerializeField]
        [Tooltip("Logs relating to user input.")]
        private LogCategorySettings m_Input = new LogCategorySettings();
        [SerializeField]
        [Tooltip("Logs relating to the user interface - visual display or audio related logs.")]
        private LogCategorySettings m_UI = new LogCategorySettings();
        [SerializeField]
        [Tooltip("Logs relating to game logic or calculations")]
        private LogCategorySettings m_GAME_LOGIC = new LogCategorySettings();
        [SerializeField]
        [Tooltip("Logs relating to interfacing with third party libraries")]
        private LogCategorySettings m_THIRD_PARTY = new LogCategorySettings();
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Private Data Members
        // ****************************************************************
        private Dictionary<LogCategory, LogCategorySettings> m_logCategorySettings = new Dictionary<LogCategory, LogCategorySettings>();
        private LogCategory m_enabledCategories = LogCategory.NONE;
        private bool m_bInitialised = false;
        #endregion


        // ****************************************************************
        #region MonoBehaviour Methods
        // ****************************************************************
        void Start()
        {
            m_logCategorySettings[LogCategory.UNCATEGORISED] = m_UNCATEGORISED;
            m_logCategorySettings[LogCategory.DATA] = m_DATA;
            m_logCategorySettings[LogCategory.INPUT] = m_Input;
            m_logCategorySettings[LogCategory.UI] = m_UI;
            m_logCategorySettings[LogCategory.GAME_LOGIC] = m_GAME_LOGIC;
            m_logCategorySettings[LogCategory.THIRD_PARTY] = m_THIRD_PARTY;

            foreach (var categorySetting in m_logCategorySettings)
            {
                if (categorySetting.Value.enabled)
                {
                    m_enabledCategories |= categorySetting.Key;
                }
            }

            m_bInitialised = true;
        }
        // ****************************************************************
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Public Methods
        // ****************************************************************
        public static void Log(string _message,
                               LogCategory _category = LogCategory.UNCATEGORISED,
                               LogSeverity _severity = LogSeverity.LOG,
                               string _tag = "",
                               GameObject _object = null)
        {
            if (instance == null)
            {
                Debug.LogError("Log() called when no LogManager exists.");
                return;
            }

            if (instance.m_bInitialised == false)
            {
                Debug.LogError("Log() called before LogManager was initialised.");
                return;
            }

            if (!instance.ShouldShow(_category, _severity, _tag))
                return;

            LogCategorySettings settings = instance.m_logCategorySettings[_category];

            string objectID = _object == null ? "" : " (" + _object.GetPath() + "" + _object.GetInstanceID() + ")";

            string output = "<color=" + settings.color.ToHex() + "><b>" + _category + ":</b></color> ";
            output += "<i>" + _tag + objectID + "</i> - ";
            output += _message;

            if (_severity >= LogSeverity.ERROR)
            {
                Debug.LogError(output, _object);
            }
            else if (_severity >= LogSeverity.WARNING)
            {
                Debug.LogWarning(output, _object);
            }
            else
            {
                Debug.Log(output, _object);
            }
        }
        // ****************************************************************
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Private Methods
        // ****************************************************************
        private bool ShouldShow(LogCategory _category,
                                LogSeverity _severity,
                                string _tag)
        {
            return (m_enabledCategories & _category) != LogCategory.NONE
                    && _severity >= m_weakestSeverity
                && (m_tags.Count == 0 || _tag.NullOrEmpty() ? true : m_tags.Contains(_tag));
        }
        // ****************************************************************
        #endregion
        // ****************************************************************

    }
    #endregion
    // ********************************************************************

}