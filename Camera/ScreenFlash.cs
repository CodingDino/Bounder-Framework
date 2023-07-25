// ************************************************************************ 
// File Name:   ScreenFlash.cs 
// Purpose:    	Apply screen flash to camera from events
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ScreenFlashEvent
// ************************************************************************
public class ScreenFlashEvent : GameEvent 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public float duration = 0.5f;
    public float maxAlpha = 1.0f;
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Constructors 
    // ********************************************************************
    public ScreenFlashEvent (float _duration = 0.5f, float _maxAlpha = 1.0f) 
	{
		duration = _duration;
		maxAlpha = _maxAlpha;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ScreenFlash
// ************************************************************************
public class ScreenFlash : MonoBehaviour
{
    // ********************************************************************
    #region Private Data 
    // ********************************************************************
    [SerializeField]
	private AnimationCurve flashAlphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Private Data 
    // ********************************************************************
    private SpriteRenderer sprite = null;
	private Image image = null;
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region MonoBehaviour Methods 
    // ********************************************************************
    private void Awake()
    {
		sprite = GetComponent<SpriteRenderer>();
		if (!sprite)
			image = GetComponent<Image>();

        if (sprite)
        {
            sprite.enabled = false;
        }
        if (image)
        {
            image.enabled = false;
        }
    }
    // ********************************************************************
    void OnEnable () 
	{
		Events.AddListener<ScreenFlashEvent>(OnScreenFlashEvent);
	}
	// ********************************************************************
	void OnDisable () 
	{
		Events.RemoveListener<ScreenFlashEvent>(OnScreenFlashEvent);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ApplyScreenFlash(float _duration = 0.5f, float _maxAlpha = 1.0f)
	{
		StartCoroutine(ApplyScreenFlash_CR(_duration, _maxAlpha));
	}
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Private Methods 
    // ********************************************************************
	private IEnumerator ApplyScreenFlash_CR(float _duration = 0.5f, float _maxAlpha = 1.0f)
	{
		float startTime = Time.time;
		Color color = Color.white;
		if (sprite)
        {
            color = sprite.color;
			sprite.enabled = true;
        }
		if (image)
        {
            color = image.color;
            image.enabled = true;
        }
		while (Time.time < startTime+ _duration)
        {
            color.a = _maxAlpha * flashAlphaCurve.Evaluate((Time.time - startTime) / _duration);

			if (sprite)
				sprite.color = color;
			if (image)
				image.color = color;

			yield return null;
        }
        if (sprite)
        {
            sprite.enabled = false;
        }
        if (image)
        {
            image.enabled = false;
        }
    }
    // ********************************************************************
    private void OnScreenFlashEvent (ScreenFlashEvent _event) 
	{
		ApplyScreenFlash(_event.duration, _event.maxAlpha);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************