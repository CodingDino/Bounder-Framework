// ************************************************************************ 
// File Name:   TintSpritesOnAnimationEvent.cs 
// Purpose:    	Tints sprites when animation event is called
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
#region Class: TintSpritesOnAnimationEvent
// ************************************************************************
public class TintSpritesOnAnimationEvent : MonoBehaviour 
{
	// ********************************************************************
	#region Class: SoundData
	// ********************************************************************
	[System.Serializable]
	public class TintData
	{
		[Tooltip("ID to be referenced by the animation event")]
		public string id;
		[Tooltip("Renderers you want to tint")]
		public SpriteRenderer[] renderers;
		[Tooltip("Color you want to tint to")]
		public Color tint = Color.white;
		[Tooltip("How long the tint should take")]
		public float duration = 0;
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[Tooltip("Tints you want to apply")]
	[SerializeField]
	private List<TintData> m_tints = new List<TintData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string,TintData> m_tintMap = new Dictionary<string, TintData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		for (int i = 0; i < m_tints.Count; ++i)
		{
			TintData tint = m_tints[i];
			if (m_tintMap.ContainsKey(tint.id))
				Debug.LogError("Duplicate ID found: "+tint.id);
			else
				m_tintMap[tint.id] = tint;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void ApplyTint (string _id) 
	{
		if (!m_tintMap.ContainsKey(_id))
		{
			Debug.LogError("No data found for ID: "+_id);
			return;
		}

		TintData tint = m_tintMap[_id];

		if (tint.duration == 0)
		{
			for (int i = 0; i < tint.renderers.Length; ++i)
				tint.renderers[i].color = tint.tint;
		}
		else
		{
			StartCoroutine(ApplyTintOverTime(tint));
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private IEnumerator ApplyTintOverTime (TintData _tint) 
	{
		float startTime = Time.time;
		Color[] originalColors = new Color[_tint.renderers.Length];
		for (int i = 0; i < originalColors.Length; ++i)
		{
			originalColors[i] = _tint.renderers[i].color;
		}
		while (Time.time < startTime + _tint.duration)
		{
			float timePassed = Time.time - startTime;
			for (int i = 0; i < _tint.renderers.Length; ++i)
				_tint.renderers[i].color = Color.Lerp(originalColors[i],_tint.tint,timePassed/_tint.duration);
			yield return null;
		}
		for (int i = 0; i < _tint.renderers.Length; ++i)
			_tint.renderers[i].color = _tint.tint;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
