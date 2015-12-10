// ************************************************************************ 
// File Name:   ObjectPool.cs 
// Purpose:    	Recycles objects to be used in the level.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************
// TODO: Allocate as coroutine? allow more spread out loading


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


// ************************************************************************ 
// Class: ObjectPool
// ************************************************************************
public class ObjectPool : IncrementalLoader
{


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private List<ObjectPoolObject> m_available = new List<ObjectPoolObject>();
	private List<ObjectPoolObject> m_inUse = new List<ObjectPoolObject>();
	private GameObject m_prefab;


	// ********************************************************************
	// Implement: IncrementalLoader 
	// ********************************************************************
	private float m_progress;
	public float GetProgress() { return m_progress; }
	public string GetCurrentAction() { return "Allocating objects"; }

	
	// ********************************************************************
	// Function:	Constructor
	// Purpose:		Sets the prefab for this object pool
	// ********************************************************************
	public ObjectPool(GameObject _prefab)
	{
		m_prefab = _prefab;
	}
	

	// ********************************************************************
	// Function:	ObjectBecameAvailable()
	// Purpose:		Moves object from in use to available
	// ********************************************************************
	public void ObjectBecameAvailable (ObjectPoolObject _object) 
	{
		Debug.Log ("object became available: "+_object.name);
		m_inUse.Remove(_object);
		m_available.Add(_object);
	}
	
	
	// ********************************************************************
	// Function:	ObjectBecameUnavailable()
	// Purpose:		Moves object from available to in use
	// ********************************************************************
	public void ObjectBecameUnavailable (ObjectPoolObject _object) 
	{
		Debug.Log ("object became unavailable: "+_object.name);
		m_available.Remove(_object);
		m_inUse.Add(_object);
	}

	
	// ********************************************************************
	// Function:	RequestObject()
	// Purpose:		Returns a recycled object if available, or creates a 
	//				new one if not.
	// ********************************************************************
	public GameObject RequestObject()
	{
		ObjectPoolObject toReturn;
		if (m_available.Count > 0)
		{
			toReturn = m_available[0];
			Debug.Log ("returning available object: "+toReturn.name);
		}
		else
		{
			toReturn = CreateObject();
			Debug.Log ("creating new object: "+toReturn.name);
		}
		toReturn.gameObject.SetActive(true); // Will mark it as unavailable
		return toReturn.gameObject;
	}

	
	// ********************************************************************
	// Function:	CreateObject()
	// Purpose:		Creates an available objects from the supplied prefab.
	// ********************************************************************
	private ObjectPoolObject CreateObject()
	{
		GameObject newObject = GameObject.Instantiate<GameObject>(m_prefab);
		ObjectPoolObject objectPoolObject = newObject.AddComponent<ObjectPoolObject>();
		objectPoolObject.pool = this;
		newObject.SetActive(false);
		return objectPoolObject;
	}
	
	
	// ********************************************************************
	// Function:	Allocate()
	// Purpose:		Creates a specified number of available objects
	//				from the supplied prefab. Spread over several frames.
	// ********************************************************************
	public IEnumerator Allocate (int _numToAllocate) 
	{
		m_progress = 0.0f;
		for (int i = 0; i < _numToAllocate; ++i)
		{
			m_progress = ((float)i)/((float)_numToAllocate);
			m_available.Add(CreateObject());
			yield return null;
		}
		m_progress = 1.0f;
	}


}
