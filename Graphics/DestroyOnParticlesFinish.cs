﻿// ************************************************************************ 
// File Name:   DestroyOnParticlesFinish.cs 
// Purpose:    	
// Project:		
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
namespace Bounder.Framework 
{ 

// ************************************************************************ 
#region Class: DestroyOnParticlesFinish
// ************************************************************************
public class DestroyOnParticlesFinish : MonoBehaviour 
{
	public enum Action {
		DESTROY,
		DISABLE
	};

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private Action m_action = Action.DESTROY;
	[SerializeField]
	private ParticleSystem m_particles = null;
	[SerializeField]
	private GameObject m_toActOn = null;
	#endregion
	// ********************************************************************

	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void Start () 
	{
		if (m_particles == null)
			m_particles = GetComponent<ParticleSystem>();
		if (m_toActOn == null)
			m_toActOn = gameObject;
	}
	// ********************************************************************
	void Update () 
	{
		if(m_particles.IsAlive())
			return;

		if (m_action == Action.DESTROY)
			Destroy(m_toActOn);
		else if (m_action == Action.DISABLE)
			m_toActOn.SetActive(false);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}
