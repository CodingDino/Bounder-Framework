// ************************************************************************ 
// File Name:   SettingsCondition.cs 
// Purpose:    	A Condition for checking the current settings
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
    #region Class: SettingsCondition
    // ************************************************************************
    [CreateAssetMenu(fileName = "SettingsCondition", menuName = "Bounder/Conditions/SettingsCondition", order = 1)]
    public class SettingsCondition : Condition
    {
        // ********************************************************************
        #region Public Data Members 
        // ********************************************************************
		[SerializeField]
		SettingsManager m_settingsManager = null;
		[SerializeField]
		SettingsDefinition m_targetSettings = null;
        [SerializeField]
        private bool m_invert = false;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Functions 
        // ********************************************************************
        public override void Register(bool _register)
        {
            // register for build config changes
            if (_register)
                m_settingsManager.OnSettingsChanged += BuildConfigChanged;
            else
                m_settingsManager.OnSettingsChanged -= BuildConfigChanged;
        }
        // ********************************************************************
        public override bool Evaluate()
        {
            if (m_invert == false && m_settingsManager.activeSettings == m_targetSettings)
                return true;
            else if (m_invert == true && m_settingsManager.activeSettings != m_targetSettings)
                return true;
            else
                return false;
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Functions 
        // ********************************************************************
        private void BuildConfigChanged(SettingsDefinition _newSettings, SettingsDefinition _oldSettings)
        {
            if (Evaluate())
                Trigger();
        }
        // ********************************************************************
        #endregion
        // ********************************************************************
    }
    #endregion
    // ************************************************************************


}