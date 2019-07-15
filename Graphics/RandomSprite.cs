// ************************************************************************ 
// File Name:   RandomSprite.cs 
// Purpose:    	Select a random sprite from a list
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************
namespace Bounder.Framework { 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: RandomSprite
// ************************************************************************ 
[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("List of sprites to choose from")]
	private Sprite[] m_sprites = null;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Monobehavior Methods 
	// ********************************************************************
	void Start () 
	{
		if (m_sprites == null || m_sprites.Length == 0)
		{
			Debug.LogError("RandomSprite.Start() - no sprites provided");
			return;
		}

		int index = Random.Range(0,m_sprites.Length);
		Sprite sprite = m_sprites[index];
		if (sprite == null)
		{
			Debug.LogError("RandomSprite.Start() - sprite "+index+" null");
			return;
		}

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		if (renderer == null)
		{
			Debug.LogError("RandomSprite.Start() - no SpriteRenderer attached");
			return;
		}

		renderer.sprite = sprite;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************

}
