// ************************************************************************ 
// File Name:   DestroyOffScreen.cs 
// Purpose:    	
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 


// ************************************************************************ 
// Class: DestroyOffScreen
// ************************************************************************ 
public class DestroyOffScreen : MonoBehaviour {

	public enum Action {
		DESTROY,
		DISABLE
	};

    // ********************************************************************
    // Private Data Members 
	// ********************************************************************
	[SerializeField]
	private Action m_action = Action.DESTROY;
	[SerializeField]
	private float m_outsideDuration = 5.0f;

	private bool m_isOutside = false;
	private float m_outsideStartTime = 0.0f;
	private SpriteRenderer[] m_sprites;

	void Start () {
		m_sprites = GetComponentsInChildren<SpriteRenderer>();
	}

	void OnEnable () {
		m_isOutside = false;
		m_outsideStartTime = 0;
	}
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () 
	{
		bool outside = true;
		for (int i = 0; i < m_sprites.Length; ++i)
		{
			if (m_sprites[i].isVisible)
			{
				outside = false;
				break;
			}
		}

		if (outside)
		{
			if (!m_isOutside)
			{
				m_isOutside = true;
				m_outsideStartTime = Time.time;
			}

			if (Time.time >= m_outsideStartTime + m_outsideDuration)
			{
				if (m_action == Action.DESTROY)
					Destroy(gameObject);
				else if (m_action == Action.DISABLE)
					gameObject.SetActive(false);
			}
		}
		else
			m_isOutside = false;
	}
}