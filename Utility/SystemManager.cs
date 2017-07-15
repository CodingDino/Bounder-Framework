// ************************************************************************ 
// File Name:   SystemManager.cs 
// Purpose:    	Loads game-wide system prefabs if they aren't present
// Project:		ArmouredEngines
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


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
#region Class: SystemManager
// ************************************************************************
public class SystemManager :  Singleton<SystemManager>
{
	// ********************************************************************
	#region Exposed Data Members
	// ********************************************************************
	[SerializeField]
	private GameObject[] m_prefabs;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members
	// ********************************************************************
	private bool m_initialised = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties
	// ********************************************************************
	public static bool initialised { get { return instance.m_initialised; } }
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	void Awake()
	{
		if (instance != this)
			DestroyImmediate(gameObject);
		else if (m_initialised == false)
		{
			for (int i = 0; i < m_prefabs.Length; ++i)
			{
				Instantiate(m_prefabs[i],transform);
			}
			m_initialised = true;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************