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
	// Interfae: Data
	// ********************************************************************
	public interface Data
	{
		string id { get; set; }
	}
	// ********************************************************************


	// ********************************************************************
	// Class: Database 
	// ******************************************************************** 
	public class Database<T> : Singleton<Database<T>> where T : Archive, Data, new() 
	{
		// ****************************************************************
		#region Exposed Data Members
		// ****************************************************************
		[SerializeField]
		private string[] m_databaseFolders;
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
		#region Public Methods
		// ****************************************************************
		public static T GetData(string _id)
		// ****************************************************************
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

		// ****************************************************************
		public static void Initialise()
		// ****************************************************************
		{
			for (int iFolder = 0; iFolder < instance.m_databaseFolders.Length; ++iFolder)
			{
				TextAsset[] files = Resources.LoadAll<TextAsset>(instance.m_databaseFolders[iFolder]);
				for (int iFile = 0; iFile < files.Length; ++iFile)
				{
					TextAsset file = files[iFile];
					string jsonString = file.text;
					if (!jsonString.NullOrEmpty())
					{
						JSON N = JSON.ParseString(jsonString);
						foreach(JSON entry in N["data"])
						{
							instance.ReadEntry(entry);
						}
					}
					else
					{
						Debug.LogError("Database.Initialise(): Text file "+file.name+" contained empty json string.");	
					}
				}
			}
			instance.m_initialised = true;
		}
		// ****************************************************************
		#endregion
		// ****************************************************************


		// ****************************************************************
		#region Protected Methods
		// ****************************************************************
		protected void ReadEntry(JSON _entry)
		// ****************************************************************
		{
			T data = _entry.GetArchive<T>();

			if (data != null)
			{
				m_data[data.id] = data;
			}
			else
			{
				Debug.LogError("Database.ReadEntry(): Failed to load item: "+_entry.ToString());
			}
		}
		// ****************************************************************
		#endregion
		// ****************************************************************

	}
	// ********************************************************************

}
// ************************************************************************