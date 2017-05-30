﻿// ************************************************************************ 
// File Name:   ObjectPoolObject.cs 
// Purpose:    	Object managed by an object pool.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: ObjectPoolObject
// ************************************************************************
public class ObjectPoolObject : MonoBehaviour 
{
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private ObjectPool m_pool;
	private static bool m_shuttingDown = false;


	// ********************************************************************
	// Properties 
	// ********************************************************************
	public ObjectPool pool { set { m_pool = value; } }


	// ********************************************************************
	// Monobehavior Methods
	// ********************************************************************
	void OnEnable () 
	{
		Reserve();
	}
	// ********************************************************************
	void OnDisable () 
	{
		Recycle();
	}
	// ********************************************************************
	void OnDestroy () 
	{
		if (!m_shuttingDown && !BounderFramework.LoadingSceneManager.loading)
			Debug.LogWarning("ObjectPoolObject destroyed - should simply disable so it can be reused!");
		if (m_pool != null)
			m_pool.ObjectDestroyed(this);
	}
	// ********************************************************************
	void OnApplicationQuit()
	{
		m_shuttingDown = true;
	}
	// ********************************************************************


	// ********************************************************************
	// Private Methods
	// ********************************************************************
	void Recycle()
	{
		if (m_pool != null)
			m_pool.ObjectBecameAvailable(this);
	}
	// ********************************************************************
	void Reserve()
	{
		if (m_pool != null)
			m_pool.ObjectBecameUnavailable(this);
	}
	// ********************************************************************

}
