// ************************************************************************ 
// File Name:   BuildConfiguration.cs 
// Purpose:    	Defines system configurations for the game, for different 
//              build purposes
// Project:		Bounder Framework
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
    #region Class: BuildConfiguration
    // ************************************************************************
    [CreateAssetMenu(fileName = "BuildConfiguration", menuName = "Bounder/Settings/BuildConfiguration", order = 1)]
    public class BuildConfiguration : SettingsDefinition
    {
        // ********************************************************************
        #region Exposed Data Members
        // ********************************************************************

        // ********************************************************************
        [Header("Framework Build Settings")]
        // ********************************************************************
        [SerializeField] private PlayerProfile m_baseProfile = null;
        public PlayerProfile baseProfile { get { return m_baseProfile; } }
        [SerializeField] private bool m_displayVersionNum = false;
        public bool displayVersionNum { get { return m_displayVersionNum; } }
        [SerializeField] private bool m_enableDebugMenu = false;
        public bool enableDebugMenu { get { return m_enableDebugMenu; } }
        // ********************************************************************
        
        #endregion
        // ********************************************************************
    }
    #endregion
    // ************************************************************************
}