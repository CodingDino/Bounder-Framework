// ************************************************************************ 
// File Name:   SpriteTint.cs 
// Purpose:    	Tint a sprite
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: SpriteTint
// ************************************************************************
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTint : MonoBehaviour 
{
	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private SpriteRenderer m_renderer = null;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void Awake () 
	{
		m_renderer = GetComponent<SpriteRenderer>();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ChangeColor (Color _newColor, float _duration = 0) 
	{
		StartCoroutine(ChangeColorOverTime(_newColor, _duration));
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private IEnumerator ChangeColorOverTime (Color _newColor, float _duration) 
	{
		float startTime = Time.time;
		Color originalColor = m_renderer.color;
		while (Time.time < startTime + _duration)
		{
			float timePassed = Time.time - startTime;
			m_renderer.color = Color.Lerp(originalColor,_newColor,timePassed/_duration);
			yield return null;
		}
		m_renderer.color = _newColor;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
