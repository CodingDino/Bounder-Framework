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
using UnityEngine.UI;
using Bounder.Framework;
using UnityEngine.EventSystems;
using System;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ButtonPlaySFX
// ************************************************************************
[RequireComponent(typeof(Button))]
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
		public AudioInfoFM info = new AudioInfoFM();
	}


    // ********************************************************************
    #region Exposed Data Members 
    // ********************************************************************
    [SerializeField]
    private ButtonSFXInfo[] buttonSFXInfo;
    [SerializeField]
    private bool playWhenButtonDisabled = false;
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Private Data Members 
    // ********************************************************************
    private Button button = null;
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Public Methods 
    // ********************************************************************
    public void Awake()
    {
        button = GetComponent<Button>();
    }
    // ********************************************************************
    public void OnPointerEnter(PointerEventData ped)
    {
        //Debug.Log("OnPointerEnter");
        if (playWhenButtonDisabled || (button!= null && button.enabled && button.interactable) )
        {
            for (int i = 0; i < buttonSFXInfo.Length; ++i)
            {
                if (buttonSFXInfo[i].type.Contains(EffectType.HIGHLIGHT))
                {
                    AudioManagerFM.PlayAsOneShot(buttonSFXInfo[i].info);
                }
            }
        }
    }
    // ********************************************************************
    public void OnPointerDown(PointerEventData ped)
    {
        //Debug.Log("OnPointerDown");
        if (playWhenButtonDisabled || (button != null && button.enabled && button.interactable))
        {
            for (int i = 0; i < buttonSFXInfo.Length; ++i)
            {
                if (buttonSFXInfo[i].type.Contains(EffectType.PRESS_DOWN))
                {
                    AudioManagerFM.PlayAsOneShot(buttonSFXInfo[i].info);
                }
            }
        }
    }
    // ********************************************************************
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Debug.Log("OnPointerClick");
        if (playWhenButtonDisabled || (button != null && button.enabled && button.interactable))
        {
            for (int i = 0; i < buttonSFXInfo.Length; ++i)
            {
                if (buttonSFXInfo[i].type.Contains(EffectType.CLICK))
                {
                    AudioManagerFM.PlayAsOneShot(buttonSFXInfo[i].info);
                }
            }
        }
    }
    // ********************************************************************
    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("OnSelect");
        if (playWhenButtonDisabled || (button != null && button.enabled && button.interactable))
        {
            for (int i = 0; i < buttonSFXInfo.Length; ++i)
            {
                if (buttonSFXInfo[i].type.Contains(EffectType.SELECT) && InputManager.useDirectionalUINavigation)
                {
                    AudioManagerFM.PlayAsOneShot(buttonSFXInfo[i].info);
                }
            }
        }
    }
    // ********************************************************************
    public void OnSubmit(BaseEventData eventData)
    {
        //Debug.Log("OnSubmit");
        if (playWhenButtonDisabled || (button != null && button.enabled && button.interactable))
        {
            for (int i = 0; i < buttonSFXInfo.Length; ++i)
            {
                if (buttonSFXInfo[i].type.Contains(EffectType.SUBMIT))
                {
                    AudioManagerFM.PlayAsOneShot(buttonSFXInfo[i].info);
                }
            }
        }
    }
    // ********************************************************************
    #endregion
    // ********************************************************************

}
#endregion
// ************************************************************************
