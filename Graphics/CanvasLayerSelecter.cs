// ************************************************************************ 
// File Name:   LayerSelecter.cs 
// Purpose:    	Allows the sorting layer of the object to be selected.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2021 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;
#endif
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: CanvasLayerSelecter
// ************************************************************************ 
[ExecuteInEditMode]
public class CanvasLayerSelecter : MonoBehaviour
{
    // ********************************************************************
    #region Private Data Members 
    // ********************************************************************
    [SerializeField]
    [SortingLayerAttribute]
    private string m_sortingLayer = "Default";
    [SerializeField]
    private int m_orderInLayer = 0;
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region MonoBehaviour Methods
    // ********************************************************************
    void Start()
    {
        if (GetComponent<Canvas>() != null)
        {
            GetComponent<Canvas>().sortingLayerName = m_sortingLayer;
            GetComponent<Canvas>().sortingOrder = m_orderInLayer;
        }
        if (Application.isPlaying)
            this.enabled = false;
    }
    // ********************************************************************
    void Update()
    {
        if (GetComponent<Canvas>() != null)
        {
            GetComponent<Canvas>().sortingLayerName = m_sortingLayer;
            GetComponent<Canvas>().sortingOrder = m_orderInLayer;
        }
    }
    // ********************************************************************
    #endregion
    // ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************