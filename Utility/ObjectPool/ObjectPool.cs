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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class:  ObjectPool
// ************************************************************************
[System.Serializable]
public class ObjectPool : IncrementalLoader
{
    // ********************************************************************
    #region Static Generic Object Pools
    // ********************************************************************
	private static Dictionary<GameObject, ObjectPool> genericPools = new Dictionary<GameObject, ObjectPool>();
    // ********************************************************************
    public static ObjectPool GetGenericObjectPool(GameObject _prefab)
    {
        if (!genericPools.ContainsKey(_prefab))
        {
            genericPools[_prefab] = new ObjectPool(_prefab);
        }
        return genericPools[_prefab];
    }
    // ********************************************************************
    public static GameObject RequestGenericObject(GameObject _prefab)
    {
        return GetGenericObjectPool(_prefab).RequestObject();
    }
    // ********************************************************************
    public static void PreAllocateGenericObjectsImmediate(GameObject _prefab, int _numToAllocate)
    {
        GetGenericObjectPool(_prefab).AllocateImmediate(_numToAllocate);
    }
    // ********************************************************************
    public static ObjectPool GetGenericObjectPool<T>(T _prefab) where T : MonoBehaviour
    {
        if (!genericPools.ContainsKey(_prefab.gameObject))
        {
            genericPools[_prefab.gameObject] = new ObjectPool(_prefab.gameObject);
        }
        return genericPools[_prefab.gameObject];
    }
    // ********************************************************************
    public static T RequestGenericObject<T>(T _prefab, Transform _parent = null) where T : MonoBehaviour
    {
        return GetGenericObjectPool(_prefab.gameObject).RequestObject(_parent).GetComponent<T>();
    }
    // ********************************************************************
    public static void PreAllocateGenericObjectsImmediate<T>(T _prefab, int _numToAllocate, Transform _parent = null) where T : MonoBehaviour
    {
        GetGenericObjectPool(_prefab.gameObject).AllocateImmediate(_numToAllocate, _parent);
    }
    // ********************************************************************
    #endregion
    // ********************************************************************



    // ********************************************************************
    #region Private Data Members 
    // ********************************************************************
    private List<ObjectPoolObject> m_available = new List<ObjectPoolObject>();
	private List<ObjectPoolObject> m_inUse = new List<ObjectPoolObject>();
	private GameObject m_prefab;
	private int m_allocationCount = 0;
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Properties
    // ********************************************************************
    public int Count { get { return m_inUse.Count; } }
	public ObjectPoolObject FirstActive { get { return Count > 0 ? m_inUse[0] : null; } }
	public List<ObjectPoolObject> activeObjects { get { return m_inUse; } }
	public GameObject parent { get; set; } = null;
    #endregion
    // ********************************************************************


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
	public GameObject RequestObject(Transform _parent = null)
	{
		ObjectPoolObject toReturn;

		if (m_available.Count <= 0)
		{
			AllocateImmediate(1, _parent);
		}
		toReturn = m_available[0];
		if (_parent)
			toReturn.transform.SetParent(_parent);
        toReturn.gameObject.SetActive(true); // Will mark it as unavailable
		return toReturn.gameObject;
	}
	// ********************************************************************
	public IEnumerator Allocate (int _numToAllocate, Transform _parent = null) 
	{
		m_progress = 0.0f;
		for (int i = 0; i < _numToAllocate; ++i)
		{
			m_progress = ((float)i)/((float)_numToAllocate);
			m_available.Add(CreateObject(_parent));
			yield return null;
		}
		m_progress = 1.0f;
	}
	// ********************************************************************
	public void AllocateImmediate (int _numToAllocate, Transform _parent = null) 
	{
		for (int i = 0; i < _numToAllocate; ++i)
		{
			m_available.Add(CreateObject(false, _parent));
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
		if (parent == null)
		{
			string parentName = "Object Pool";
			if (m_prefab)
				parentName += "-" + m_prefab.name;
			parent = new GameObject(parentName);
		}
		return parent;
	}
    // ********************************************************************
    private ObjectPoolObject CreateObject(bool _active = true, Transform _parent = null)
	{
		GameObject newObject = null;
		if (m_prefab == null)
		{
			newObject = new GameObject("ObjectPool Object");
            newObject.transform.SetParent(_parent ? _parent : GetParent().transform);
        }
		else
		{
			bool wasEnabled = m_prefab.activeSelf;
			m_prefab.SetActive(false);
			newObject = GameObject.Instantiate<GameObject>(m_prefab, _parent ? _parent : GetParent().transform);
            m_prefab.SetActive(wasEnabled);
        }
		newObject.name += "-" + m_allocationCount++;
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