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


// ************************************************************************ 
// Class: ObjectPoolObject
// ************************************************************************
public class ObjectPoolObject : MonoBehaviour 
{
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private ObjectPool m_pool;


	// ********************************************************************
	// Properties 
	// ********************************************************************
	public ObjectPool pool { set { m_pool = value; } }

	
	// ********************************************************************
	// Function:	Recycle()
	// Purpose:		Inform pool that we are available
	// ********************************************************************
	void Recycle()
	{
		if (m_pool != null)
			m_pool.ObjectBecameAvailable(this);
	}
	
	
	// ********************************************************************
	// Function:	Reserve()
	// Purpose:		Inform pool that we are NOT available
	// ********************************************************************
	void Reserve()
	{
		if (m_pool != null)
			m_pool.ObjectBecameUnavailable(this);
	}
	
	
	// ********************************************************************
	// Function:	OnDisable()
	// Purpose:		Recycle object when it is disabled
	// ********************************************************************
	void OnEnable () 
	{
		Reserve();
	}

	
	// ********************************************************************
	// Function:	OnDisable()
	// Purpose:		Recycle object when it is disabled
	// ********************************************************************
	void OnDisable () 
	{
		Recycle();
	}
}
