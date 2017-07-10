// ************************************************************************ 
// File Name:   SpawnOnAnimationEvent.cs 
// Purpose:    	Instantiates a prefab on animation event
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************
namespace BounderFramework { 


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
#region Class: SpawnOnAnimationEvent
// ************************************************************************ 
[RequireComponent(typeof(Animator))]
public class SpawnOnAnimationEvent : MonoBehaviour 
{
	// ********************************************************************
	#region Class: SpawnData
	// ********************************************************************
	[System.Serializable]
	public class SpawnData
	{
		[Tooltip("Prefab you want to spawn")]
		public GameObject prefab = null;
		[Tooltip("Where you want the new object positioned")]
		public Transform position = null;
		[Tooltip("Parent for the new object")]
		public Transform parent = null;
		[Tooltip("Should an object pool be used for spawning?")]
		public bool useObjectPool = true;
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("Objects you want to spawn")]
	private List<SpawnData> m_objects = new List<SpawnData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string,SpawnData> m_spawnMap = new Dictionary<string, SpawnData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Static Data Members 
	// ********************************************************************
	protected static Dictionary<string, ObjectPool> s_objectPools = new Dictionary<string, ObjectPool>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		for (int i = 0; i < m_objects.Count; ++i)
		{
			SpawnData spawn = m_objects[i];
			if (m_spawnMap.ContainsKey(spawn.prefab.name))
				Debug.LogError("Duplicate ID found: "+spawn.prefab.name);
			else
				m_spawnMap[spawn.prefab.name] = spawn;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	private void SpawnObject (string _id) 
	{
		if (!m_spawnMap.ContainsKey(_id))
		{
			Debug.LogError("No data found for ID: "+_id);
			return;
		}

		SpawnData spawn = m_spawnMap[_id];

		GameObject spawnedObject = null;
		if (spawn.useObjectPool)
		{
			if (!s_objectPools.ContainsKey(spawn.prefab.name))
				s_objectPools[spawn.prefab.name] = new ObjectPool(spawn.prefab);
			spawnedObject = s_objectPools[spawn.prefab.name].RequestObject();
		}
		else
			spawnedObject = Instantiate(spawn.prefab);
		if (spawn.position != null)
			spawnedObject.transform.position = spawn.position.position;
		if (spawn.parent != null)
			spawnedObject.transform.SetParent(spawn.parent,true);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************

}
