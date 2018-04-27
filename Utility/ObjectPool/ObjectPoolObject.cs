// ************************************************************************ 
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
using System.Collections.Generic;


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
	#region Events 
	// ********************************************************************
	public delegate void Action(ObjectPoolObject _object);
	public event Action OnRecycle;
	public event Action OnReserve;
	#endregion
	// ********************************************************************


	// ********************************************************************
	// Monobehavior Methods
	// ********************************************************************
	void OnEnable () 
	{
		Reserve();
		ObjectPoolResetResponder[] responders = GetComponentsInChildren<ObjectPoolResetResponder>();
		for (int i = 0; i < responders.Length; ++i)
		{
			responders[i].Reset();
		}
	}
	// ********************************************************************
	void OnDisable () 
	{
		Recycle();
	}
	// ********************************************************************
	void OnDestroy () 
	{
		if (!m_shuttingDown && !BounderFramework.LoadingSceneManager.loading && m_pool != null && m_pool.activeObjects.Contains(this))
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

		if (OnRecycle != null)
			OnRecycle(this);
	}
	// ********************************************************************
	void Reserve()
	{
		if (m_pool != null)
			m_pool.ObjectBecameUnavailable(this);

		if (OnReserve != null)
			OnReserve(this);
	}
	// ********************************************************************

}
