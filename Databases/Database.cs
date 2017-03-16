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
using System.Collections.Generic;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework { 
	
	// ********************************************************************
	// Class: Data
	// ********************************************************************
	public class Data : Archive
	{
		public string id;

		// ********************************************************************
		// Archive Methods 
		// ********************************************************************
		public virtual bool Load(JSON _JSON)
		{
			bool success = true;

			success &= _JSON["id"].Get (ref id);

			return success;
		}
		// ********************************************************************
		public virtual JSON Save()
		{
			JSON save = new JSON();

			save["id"].data = id;

			return save;
		}
		// ********************************************************************
	}
	// ********************************************************************


	// ********************************************************************
	// Class: Database 
	// ******************************************************************** 
	public class Database<T> : Singleton<Database<T>> where T : Data, new() 
	{
		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		protected string[] m_databaseFolders;
		[SerializeField]
		protected string[] m_demoModeFolders;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Private Data Members
		// ****************************************************************
		private Dictionary<string, T> m_data = new Dictionary<string, T>();
		private bool m_initialised = false;
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Monobehaviour Methods 
		// ****************************************************************
		void OnEnable()
		{
			DebugMenu.OnDemoModeToggle += OnDemoModeToggle;
		}
		// ****************************************************************
		void OnDisable()
		{
			DebugMenu.OnDemoModeToggle -= OnDemoModeToggle;
		}
		// ****************************************************************
		protected void OnDemoModeToggle(bool _demoActive)
		{
			Reinitialise();
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Public Methods
		// ****************************************************************
		public static bool HasData(string _id)
		{
			if (!instance.m_initialised)
			{
				Debug.LogError("Database.HasData("+_id+"): Uninitialised Databse.");	
				return false;
			}

			return instance.m_data.ContainsKey(_id);
		}
		// ****************************************************************
		public static T GetData(string _id)
		{
			if (!instance.m_initialised)
			{
				Debug.LogError("Database.GetData("+_id+"): Uninitialised Databse.");	
				return default (T);
			}

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
		public static void Initialise()
		{
			instance._Initialise();
		}
		// ****************************************************************
		protected virtual bool _Initialise()
		{
			Debug.Log("Database.Initialise() for "+name);
			string[] databaseFolders = DebugMenu.demoMode ? m_demoModeFolders : m_databaseFolders;
			for (int iFolder = 0; iFolder < databaseFolders.Length; ++iFolder)
			{
				TextAsset[] files = Resources.LoadAll<TextAsset>(databaseFolders[iFolder]);
				if (files.Length == 0)
					Debug.LogError("Database.Initialise(): No files found in folder "+databaseFolders[iFolder]);
				for (int iFile = 0; iFile < files.Length; ++iFile)
				{
					TextAsset file = files[iFile];
					string jsonString = file.text;
					if (!jsonString.NullOrEmpty())
					{
						JSON N = JSON.ParseString(jsonString);
						// Single entry
						if (N.GetArchive<T>() != null)
						{
							ReadEntry(N);
						}
						// Multiple entries
						else
						{
							foreach(JSON entry in N)
							{
								ReadEntry(entry);
							}
						}
					}
					else
					{
						Debug.LogError("Database.Initialise(): Text file "+file.name+" contained empty json string.");	
					}
				}
			}
			m_initialised = true;
			return m_initialised;
		}
		// ****************************************************************
		public static bool IsInitialised()
		{
			return instance.m_initialised;
		}
		// ****************************************************************
		public static void Reinitialise() 
		{ 
			instance.m_initialised = false;
			instance.m_data.Clear();
			Initialise();
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Protected Methods
		// ****************************************************************
		protected virtual T ReadEntry(JSON _entry)
		{
			Debug.Log("Database.ReadEntry("+_entry.ToString()+")");
			T data = _entry.GetArchive<T>();

			if (data != null)
			{
				m_data[data.id] = data;
			}
			else
			{
				Debug.LogError("Database.ReadEntry(): Failed to load item: "+_entry.ToString());
			}

			return data;
		}
		// ****************************************************************
		#endregion
		// ****************************************************************

	}
	// ********************************************************************

}
// ************************************************************************