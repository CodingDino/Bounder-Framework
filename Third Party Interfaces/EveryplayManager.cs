// ************************************************************************ 
// File Name:   EveryplayManager.cs 
// Purpose:    	Interface for handling Everyplay functionality
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// ************************************************************************ 
// Class: DebugMenu
// ************************************************************************ 
public class EveryplayManager : MonoBehaviour {

    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
    private bool m_recording = false;
    private bool m_readyToRecord = false;
    private GameObject m_debugRecordButton = null;


    // ********************************************************************
    // Function:	Start()
    // Purpose:		Run when new instance of the object is created.
    // ********************************************************************
    void Start()
    {
//        Everyplay.ReadyForRecording += OnReadyForRecording;
    }


    // ********************************************************************
    // Function:	Start()
    // Purpose:		Run when new instance of the object is created.
    // ********************************************************************
    private void OnReadyForRecording(bool enabled)
    {
        if (enabled)
        {
            m_readyToRecord = true;
            UpdateDebugToggleButton();
        }
    }


    // ********************************************************************
    // Function:	OnEnable()
    // Purpose:		Called when the script is enabled.
    // ********************************************************************
    void OnEnable()
    {
        m_debugRecordButton = DebugMenu.AddButton("EveryplayToggle", "Everyplay NOT READY", DebugEveryplayToggle);
        UpdateDebugToggleButton();
    }


    // ********************************************************************
    // Function:	OnDisable()
    // Purpose:		Called when the script is disabled.
    // ********************************************************************
    void OnDisable()
    {
        DebugMenu.RemoveButton("EveryplayToggle");
    }

    // ********************************************************************
    // Function:	UpdateDebugToggleButton()
    // Purpose:		Updates the image of the debug toggle button.
    // ********************************************************************
    private void UpdateDebugToggleButton()
    {
        if (m_debugRecordButton == null) return;

        string buttonName = "Everyplay NOT READY";
        Color buttonColor = Color.gray;
        if (m_readyToRecord && m_recording)
        {
            buttonName = "Everyplay STOP";
            buttonColor = Color.green;
        }
        else if (m_readyToRecord && !m_recording)
        {
            buttonName = "Everyplay START";
            buttonColor = Color.red;
        }
        m_debugRecordButton.GetComponent<Image>().color = buttonColor;
        m_debugRecordButton.transform.GetChild(0).GetComponent<Text>().text = buttonName;
    }



    // ********************************************************************
    // Function:	DebugEveryplayToggle()
    // Purpose:		Called when debug toggle button is pressed.
    // ********************************************************************
    void DebugEveryplayToggle(string _id, GameObject _button)
    {
        if (!m_readyToRecord) return;

        m_recording = !m_recording;
        UpdateDebugToggleButton();

//        if (m_recording)
//            Everyplay.StartRecording();
//        else
//            Everyplay.StopRecording();
    }


}
