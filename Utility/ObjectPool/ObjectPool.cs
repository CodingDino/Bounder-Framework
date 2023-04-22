// ************************************************************************ 
// File Name:   ObjectPool.cs 
// Purpose:    	Recycles objects to be used in the level.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************
// TODO: Allocate as coroutine? allow more spread out loading


// ************************************************************************ 
#region Imports
// ************************************************************************ 
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class:  ObjectPool
// ************************************************************************
[System.Serializable]
public class ObjectPool : IncrementalLoader
{
	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private List<ObjectPoolObject> m_available = new List<ObjectPoolObject>();
	private List<ObjectPoolObject> m_inUse = new List<ObjectPoolObject>();
	private GameObject m_prefab;
	private GameObject m_parent;
	private int m_runningCount = 0;
	#endregion
	// ****************************************************************


	// ****************************************************************
	#region Properties
	// ****************************************************************
	public int Count { get { return m_inUse.Count; } }
	public ObjectPoolObject FirstActive { get { return Count > 0 ? m_inUse[0] : null; } }
	public List<ObjectPoolObject> activeObjects { get { return m_inUse; } }
	#endregion
	// ****************************************************************


	// ********************************************************************
	#region IncrementalLoader Methods
	// ********************************************************************
	private float m_progress;
	public float GetProgress() { return m_progress; }
	public string GetCurrentAction() { return "Allocating objects"; }
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public ObjectPool(GameObject _prefab = null, int _toAllocate = 0)
	{
		m_prefab = _prefab;
		AllocateImmediate(_toAllocate);
	}
	// ********************************************************************
	public void ObjectBecameAvailable (ObjectPoolObject _object) 
	{
//		Debug.Log ("object became available: "+_object.name);
		m_inUse.Remove(_object);
		m_available.Add(_object);
	}
	// ********************************************************************
	public void ObjectBecameUnavailable (ObjectPoolObject _object) 
	{
//		Debug.Log ("object became unavailable: "+_object.name);
		m_available.Remove(_object);
		m_inUse.Add(_object);
	}
	// ********************************************************************
	public void ObjectDestroyed (ObjectPoolObject _object) 
	{
//		Debug.Log ("object destroyed: "+_object.name);
		m_available.Remove(_object);
		m_inUse.Remove(_object);
	}
	// ********************************************************************
	public GameObject RequestObject()
	{
		ObjectPoolObject toReturn;

		if (m_available.Count <= 0)
		{
			AllocateImmediate(1);
		}
		toReturn = m_available[0];
		toReturn.gameObject.SetActive(true); // Will mark it as unavailable
		return toReturn.gameObject;
	}
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
	// ********************************************************************
	public void AllocateImmediate (int _numToAllocate) 
	{
		for (int i = 0; i < _numToAllocate; ++i)
		{
			m_available.Add(CreateObject(false));
		}
	}
	// ********************************************************************
	public void RecycleAll()
	{
		// Walk backwards through list so we can remove from it safely
		for (int iObject = m_inUse.Count-1; iObject >= 0; --iObject)
		{
			m_inUse[iObject].gameObject.SetActive(false);
		}
	}
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Private Methods 
    // ********************************************************************
	private GameObject GetParent()
	{
		if (m_parent == null)
		{
			string parentName = "Object Pool";
			if (m_prefab)
				parentName += "-" + m_prefab.name;
			m_parent = new GameObject(parentName);
		}
		return m_parent;
	}
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
		newObject.name += "-" + m_runningCount++;
        newObject.transform.SetParent(GetParent().transform);
        ObjectPoolObject objectPoolObject = newObject.AddComponent<ObjectPoolObject>();
		objectPoolObject.pool = this;
		newObject.SetActive(_active);
		return objectPoolObject;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


}
#endregion
// ************************************************************************