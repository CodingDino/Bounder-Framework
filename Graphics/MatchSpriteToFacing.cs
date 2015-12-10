// ************************************************************************ 
// File Name:   MatchSpriteToFacing.cs 
// Purpose:    	Changes sprite origentation based on facing.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: MatchSpriteToFacing
// ************************************************************************ 
public class MatchSpriteToFacing : MonoBehaviour {
	
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private SpriteRenderer m_sprite = null;
	[SerializeField]
	private Entity m_entity = null;


	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
	
	}


	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update () {
		if (m_sprite == null || m_entity == null)
			return;

		float invert = 1;

		if (m_entity.facing > 90 && m_entity.facing < 270)
			invert = -1;

		float scale = Mathf.Abs(m_sprite.transform.localScale.x) * (float)invert;
		m_sprite.transform.localScale = new Vector3(scale, m_sprite.transform.localScale.y, m_sprite.transform.localScale.z);
	}
}
