// ************************************************************************ 
// File Name:   ButtonPlaySFX.cs 
// Purpose:    	
// Project:		
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using Bounder.Framework;
using UnityEngine.EventSystems;
using System;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ButtonPlaySFX
// ************************************************************************
public class ButtonPlaySFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{

    [Flags]
	public enum EffectType
    {
        NONE = 0,
        // ---
        HIGHLIGHT = 1 << 0,
        SELECT = 1 << 1,
        PRESS_DOWN = 1 << 2,
        CLICK = 1 << 3,
        SUBMIT = 1 << 4,
        // ---
        HIGHLIGHT_OR_SELECT = SELECT | HIGHLIGHT,
        CLICK_OR_SUBMIT = CLICK | SUBMIT
    }

	[System.Serializable]
	public class ButtonSFXInfo
	{
		public EffectType type = EffectType.NONE;
		public AudioInfo info = new AudioInfo();
	}


    // ********************************************************************
    #region Exposed Data Members 
    // ********************************************************************
    [SerializeField]
    private ButtonSFXInfo[] buttonSFXInfo;
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Public Methods 
    // ********************************************************************
    public void OnPointerEnter(PointerEventData ped)
    {
        Debug.Log("OnPointerEnter");
        for (int i = 0; i < buttonSFXInfo.Length; ++i)
        {
            if (buttonSFXInfo[i].type.Contains(EffectType.HIGHLIGHT))
            {
                AudioManager.Play(buttonSFXInfo[i].info);
            }
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("OnPointerDown");
        for (int i = 0; i < buttonSFXInfo.Length; ++i)
        {
            if (buttonSFXInfo[i].type.Contains(EffectType.PRESS_DOWN))
            {
                AudioManager.Play(buttonSFXInfo[i].info);
            }
        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("OnPointerClick");
        for (int i = 0; i < buttonSFXInfo.Length; ++i)
        {
            if (buttonSFXInfo[i].type.Contains(EffectType.CLICK))
            {
                AudioManager.Play(buttonSFXInfo[i].info);
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("OnSelect");
        for (int i = 0; i < buttonSFXInfo.Length; ++i)
        {
            if (buttonSFXInfo[i].type.Contains(EffectType.SELECT) && InputManager.useDirectionalUINavigation)
            {
                AudioManager.Play(buttonSFXInfo[i].info);
            }
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("OnSubmit");
        for (int i = 0; i < buttonSFXInfo.Length; ++i)
        {
            if (buttonSFXInfo[i].type.Contains(EffectType.SUBMIT))
            {
                AudioManager.Play(buttonSFXInfo[i].info);
            }
        }
    }
    // ********************************************************************
    #endregion
    // ********************************************************************

}
#endregion
// ************************************************************************
