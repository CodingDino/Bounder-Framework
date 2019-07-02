// ************************************************************************ 
// File Name:   RandomTint.cs 
// Purpose:    	Select a random tint from a list
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************
namespace BounderFramework { 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: RandomTint
// ************************************************************************ 
public class RandomTint : MonoBehaviour 
{
	// ********************************************************************
	#region Enum: Selection 
	// ********************************************************************
	private enum Selection {
		DISCRETE, LERP
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("DISCRETE = pick one of the supplied tints. LERP - pick tint between supplied tints.")]
	private Selection m_selection = Selection.DISCRETE;
	[SerializeField]
	[Tooltip("List of sprites to be tinted")]
	private List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();
	[SerializeField]
	[Tooltip("List of tints to choose from")]
	private Color[] m_tints = null;
	[SerializeField]
	[Tooltip("Should the random tint be applied on start")]
	private bool m_applyOnStart = true;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Monobehavior Methods 
	// ********************************************************************
	void Awake () 
	{
		if (m_sprites.Count == 0)
		{
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			if (renderer != null)
				m_sprites.Add(renderer);
		}
	}
	// ********************************************************************
	void Start () 
	{
		if (m_applyOnStart)
			ApplyRandomTint();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ApplyRandomTint()
	{
		if (m_tints == null || m_tints.Length == 0)
		{
			Debug.LogError("RandomTint.ApplyRandomTint() - no tints provided");
			return;
		}

		Color color = Color.white;
		switch (m_selection)
		{
		case Selection.DISCRETE:
			int index = Random.Range(0,m_tints.Length);
			color = m_tints[index];
			break;
		case Selection.LERP:
			Color color1 = m_tints[0];
			Color color2 = m_tints[m_tints.Length-1];
			float lerp = Random.Range(0.0f,1.0f);
			color = Color.Lerp(color1,color2,lerp);
			break;
		}

		if (m_sprites == null || m_sprites.Count == 0)
		{
			Debug.LogError("RandomTint.ApplyTint() - no SpriteRenderer supplied");
			return;
		}

		for (int i = 0; i < m_sprites.Count; ++i)
			m_sprites[i].color = color;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************

}
