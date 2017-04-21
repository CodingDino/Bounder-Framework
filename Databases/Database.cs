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
using AssetBundles;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework { 


	// ********************************************************************
	// Class: Database 
	// ******************************************************************** 
	public class Database<T> : Singleton<Database<T>> where T : ScriptableObject, new() 
	{
		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		protected string m_assetBundleName;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Data Members
		// ****************************************************************
		private Dictionary<string, T> m_data = new Dictionary<string, T>();
		private List<AssetBundleLoadAssetOperation> m_loadRequests = new List<AssetBundleLoadAssetOperation>();
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Properties
		// ****************************************************************
		public bool loading { get { return m_loadRequests.Count != 0; } }
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Public Methods
		// ****************************************************************
		public static bool HasData(string _id)
		{
			return instance.m_data.ContainsKey(_id);
		}
		// ****************************************************************
		public static T GetData(string _id)
		{
			if (instance.m_data.ContainsKey(_id))
			{
				return instance.m_data[_id];
			}
			else
			{
				Debug.LogWarning("Database.GetData("+_id+"): Database does not contain key.");
				return default (T);
			}
		}
		// ****************************************************************
		public static Coroutine RequestAssets(List<string> _assets)
		{
			List<AssetBundleLoadAssetOperation> requests = new List<AssetBundleLoadAssetOperation>();
			for (int i = 0; i < _assets.Count; ++i)
			{
				AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(instance.m_assetBundleName,_assets[i], typeof(T));
				instance.StartCoroutine(request);
				requests.Add(request);
				instance.m_loadRequests.Add(request);	
			}
			return instance.StartCoroutine(instance.WaitForAssets(requests));
		}
		// ****************************************************************
		public static void UnloadAsset()
		{
			instance.m_data.Clear();
			AssetBundleManager.UnloadAssetBundle(instance.m_assetBundleName);
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Methods
		// ****************************************************************
		private IEnumerator WaitForAssets(List<AssetBundleLoadAssetOperation> _requests)
		{
			while (_requests.Count > 0)
			{
				if (_requests.Front().IsDone())
				{
					T asset = _requests.Front().GetAsset<T>();
					if (asset != null)
					{
						m_data[asset.name] = asset;
					}
					m_loadRequests.Remove(_requests.Front());
					_requests.RemoveAt(0);
				}
				yield return null;
			}
		}
		// ****************************************************************
		#endregion
		// ****************************************************************

	}
	// ********************************************************************

}
// ************************************************************************