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
[System.Serializable]
public class ObjectPool : IncrementalLoader
{


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private List<ObjectPoolObject> m_available = new List<ObjectPoolObject>();
	private List<ObjectPoolObject> m_inUse = new List<ObjectPoolObject>();
	private GameObject m_prefab;


	// ****************************************************************
	#region Properties
	// ****************************************************************
	public int Count { get { return m_inUse.Count; } }
	public ObjectPoolObject FirstActive { get { return Count > 0 ? m_inUse[0] : null; } }
	public List<ObjectPoolObject> activeObjects { get { return m_inUse; } }
	#endregion
	// ****************************************************************


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
	public ObjectPool(GameObject _prefab = null, int _toAllocate = 0)
	{
		m_prefab = _prefab;
		AllocateImmediate(_toAllocate);
	}
	

	// ********************************************************************
	// Function:	ObjectBecameAvailable()
	// Purpose:		Moves object from in use to available
	// ********************************************************************
	public void ObjectBecameAvailable (ObjectPoolObject _object) 
	{
//		Debug.Log ("object became available: "+_object.name);
		m_inUse.Remove(_object);
		m_available.Add(_object);
	}
	
	
	// ********************************************************************
	// Function:	ObjectBecameUnavailable()
	// Purpose:		Moves object from available to in use
	// ********************************************************************
	public void ObjectBecameUnavailable (ObjectPoolObject _object) 
	{
//		Debug.Log ("object became unavailable: "+_object.name);
		m_available.Remove(_object);
		m_inUse.Add(_object);
	}


	// ********************************************************************
	// Function:	ObjectDestroyed()
	// Purpose:		Removes object from all pools
	// ********************************************************************
	public void ObjectDestroyed (ObjectPoolObject _object) 
	{
//		Debug.Log ("object destroyed: "+_object.name);
		m_available.Remove(_object);
		m_inUse.Remove(_object);
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
//			Debug.Log ("returning available object: "+toReturn.name);
		}
		else
		{
			toReturn = CreateObject();
//			Debug.Log ("creating new object: "+toReturn.name);
		}
		toReturn.gameObject.SetActive(true); // Will mark it as unavailable
		return toReturn.gameObject;
	}

	
	// ********************************************************************
	// Function:	CreateObject()
	// Purpose:		Creates an available objects from the supplied prefab.
	// ********************************************************************
	private ObjectPoolObject CreateObject(bool _active = true)
	{
		GameObject newObject = null;
		if (m_prefab == null)
		{
			newObject = new GameObject("ObjectPool Object");
		}
		else
		{
			bool wasEnabled = m_prefab.activeSelf;
			m_prefab.SetActive(false);
			newObject = GameObject.Instantiate<GameObject>(m_prefab);
			m_prefab.SetActive(wasEnabled);
		}
		ObjectPoolObject objectPoolObject = newObject.AddComponent<ObjectPoolObject>();
		objectPoolObject.pool = this;
		newObject.SetActive(_active);
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
	public void AllocateImmediate (int _numToAllocate) 
	{
		for (int i = 0; i < _numToAllocate; ++i)
		{
			m_available.Add(CreateObject());
		}
	}


}
