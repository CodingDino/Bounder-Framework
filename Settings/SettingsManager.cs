// ************************************************************************ 
// File Name:   SettingsManager.cs 
// Purpose:    	A manager for discrete settings which can be swapped
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2019 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{
    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************ 
    #region Class: SettingsManager
    // ************************************************************************
    [CreateAssetMenu(fileName = "SettingsManager", menuName = "Bounder/Managers/SettingsManager", order = 1)]
    public class SettingsManager : ScriptableObject
    {
        // ********************************************************************
        #region Exposed Data Members
        // ********************************************************************
        [SerializeField]
        private SettingsDefinition m_activeSettings;
        #endregion
        // ********************************************************************

        // ********************************************************************
        #region Properties
        // ********************************************************************
        public SettingsDefinition activeSettings
        {
            get { return m_activeSettings; }
            set { 
                if (value != m_activeSettings)
                {
                    SettingsDefinition oldSettings = m_activeSettings;
                    m_activeSettings = value;
                    OnSettingsChanged?.Invoke(m_activeSettings, oldSettings);
                }
            }
        }
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Delegates
        // ********************************************************************
        public delegate void SettingsChangedCallback(SettingsDefinition _newMode, SettingsDefinition _oldMode);
        public event SettingsChangedCallback OnSettingsChanged;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Functions
        // ********************************************************************
        public T GetActiveSettings<T>() where T : SettingsDefinition
        {
            return activeSettings as T;
        }
        // ********************************************************************
        #endregion
        // ********************************************************************
    }
    #endregion
    // ************************************************************************


}