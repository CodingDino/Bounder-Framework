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
	public class Database<T> : Singleton<Database<T>> where T : UnityEngine.Object 
	{
		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		protected string m_assetBundleName;
		[SerializeField]
		protected List<T> m_preloadedAssets;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Data Members
		// ****************************************************************
		protected Dictionary<string, T> m_data = new Dictionary<string, T>();
		protected List<AssetBundleLoadAssetOperation> m_loadRequests = new List<AssetBundleLoadAssetOperation>();
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Properties
		// ****************************************************************
		public bool loading { get { return m_loadRequests.Count != 0; } }
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Monobehavior Methods
		// ****************************************************************
		void Start()
		{
			for (int i = 0; i < m_preloadedAssets.Count; ++i)
			{
				m_data[m_preloadedAssets[i].name] = m_preloadedAssets[i];
			}
		}
		// ****************************************************************
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
		public static Coroutine RequestAsset(string _asset, string _assetBundleName = "")
		{
			List<string> assets = new List<string>();
			assets.Add(_asset);
			return RequestAssets(assets, _assetBundleName);
		}
		// ****************************************************************
		public static Coroutine RequestAssets(List<string> _assets, string _assetBundleName = "")
		{
			if (_assets.Count == 0)
				return null;
			
			List<AssetBundleLoadAssetOperation> requests = new List<AssetBundleLoadAssetOperation>();
			for (int i = 0; i < _assets.Count; ++i)
			{
				if (_assets[i].NullOrEmpty())
					continue;
				AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(_assetBundleName.NullOrEmpty() ? instance.m_assetBundleName : _assetBundleName, _assets[i], typeof(T));
				instance.StartCoroutine(request);
				requests.Add(request);
				instance.m_loadRequests.Add(request);	
			}
			if (requests.Count > 0)
				return instance.StartCoroutine(instance.WaitForAssets(requests));
			else
				return null;
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
						Debug.Log("Database: Asset request complete: "+asset.name);
					}
					else
					{
						Debug.LogError("Database: Asset request failed.");
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