// ************************************************************************ 
// File Name:   ButtonGamepadSettings.cs 
// Purpose:    	Changes a button when the player is using a gamepad
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2019 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{

    // ************************************************************************ 
    #region Libraries 
    // ************************************************************************ 
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    // ************************************************************************ 
    #endregion
    // ************************************************************************ 


    // ************************************************************************ 
    #region Class: ButtonGamepadSettings
    // ************************************************************************ 
    [RequireComponent(typeof(Button))]
    public class ButtonGamepadSettings : MonoBehaviour
    {
        // ********************************************************************
        #region Serialized Data Members 
        // ********************************************************************
        [SerializeField]
        private bool m_shouldOverrideNavigation = false;
        [SerializeField]
        [ShowInInspectorIf("m_shouldOverrideNavigation")]
        private Navigation m_navigation = Navigation.defaultNavigation;
        [SerializeField]
        private GameObject m_buttonOultine = null;
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Data Members 
        // ********************************************************************
        private Button m_button = null;
        private Navigation m_originalNavigation = Navigation.defaultNavigation;
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region MonoBehaviour Functions 
        // ********************************************************************
        void Awake()
        {
            // Record original values
            m_button = GetComponent<Button>();
            m_originalNavigation = m_button.navigation;
        }
        // ********************************************************************
        void Start()
        {
            UpdateControlScheme(new ControlSchemeChangedEvent(InputManager.controlScheme));
        }
        // ********************************************************************
        void OnEnable()
        {
            Events.AddListener<ControlSchemeChangedEvent>(UpdateControlScheme);
            UpdateControlScheme(new ControlSchemeChangedEvent(InputManager.controlScheme));
        }
        // ********************************************************************
        void OnDisable()
        {
            Events.RemoveListener<ControlSchemeChangedEvent>(UpdateControlScheme);
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Functions 
        // ********************************************************************
        void UpdateControlScheme(ControlSchemeChangedEvent _event)
        {
            bool isGamepad = _event.newScheme == ControlScheme.GAMEPAD;
            Debug.Log("Updating button gamepad mode = "+isGamepad);

            if (m_shouldOverrideNavigation)
                m_button.navigation = isGamepad ? m_navigation : m_originalNavigation;
            if (m_buttonOultine != null)
                m_buttonOultine.SetActive(isGamepad);
        }
        // ********************************************************************
        #endregion
        // ********************************************************************
    }

    // ************************************************************************ 
    #endregion
    // ************************************************************************ 

}