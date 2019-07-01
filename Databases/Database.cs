// ************************************************************************ 
// File Name:   Database.cs 
// Purpose:    	Database base class to be used to hold definition files
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework { 


// ************************************************************************ 
#region Class: ClearDatabaseEvent
// ************************************************************************
public class ClearDatabaseForSceneChangeEvent : GameEvent { }
#endregion
// ************************************************************************


// ************************************************************************
// Class: Database 
// ************************************************************************
public class Database<T> : Singleton<Database<T>> where T : UnityEngine.Object 
{
	// ********************************************************************
	#region Exposed Data Members
	// ********************************************************************
	[SerializeField]
	protected string m_assetBundleName;
	[SerializeField]
	protected bool m_unloadOnSceneChange;
	[SerializeField]
	protected List<T> m_preloadedAssets;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members
	// ********************************************************************
	protected Dictionary<string, T> m_data = new Dictionary<string, T>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Monobehavior Methods
	// ********************************************************************
	protected virtual void Start()
	{
		SetupPreloadedAssets();
	}
	// ********************************************************************
	void OnEnable()
	{
		if (instance == this)
		{
			Events.AddListener<ClearDatabaseForSceneChangeEvent>(ClearDatabase);
		}
	}
	// ********************************************************************
	void OnDisable()
	{
		if (instance == this)
		{
			Events.RemoveListener<ClearDatabaseForSceneChangeEvent>(ClearDatabase);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************



	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public static bool HasData(string _id)
	{
		return instance.m_data.ContainsKey(_id);
	}
	// ********************************************************************
	public static T GetData(string _id)
	{
		if (instance.m_data.ContainsKey(_id))
		{
			return instance.m_data[_id];
		}
		else
		{
			Debug.LogError("Database.GetData("+_id+"): Database does not contain key.");
			return default (T);
		}
	}
	// ********************************************************************
	public static void MakeAvailable(T _asset)
	{
		instance.m_data[_asset.name] = _asset;
	}
	// ********************************************************************
	public static void MakeAvailable(List<T> _assets)
	{
		for (int i = 0; i < _assets.Count; ++i)
		{
			if (_assets[i] != null)
				MakeAvailable(_assets[i]);
		}
	}
	// ********************************************************************
	public static void UnloadAssets()
	{
		instance.m_data.Clear();
		instance.SetupPreloadedAssets();
	}
	// ********************************************************************
	public static List<T> GetDatabaseContents()
	{
		List<T> data =	new List<T>();
		foreach (var entry in instance.m_data)
		{
			data.Add(entry.Value);
		}
		return data;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods
	// ********************************************************************
	private void SetupPreloadedAssets()
	{
		for (int i = 0; i < m_preloadedAssets.Count; ++i)
		{
			m_data[m_preloadedAssets[i].name] = m_preloadedAssets[i];
		}
	}
	// ********************************************************************
	private void ClearDatabase(ClearDatabaseForSceneChangeEvent _gameEvent)
	{
		if (m_unloadOnSceneChange)
			UnloadAssets();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
// ************************************************************************

}
// ************************************************************************