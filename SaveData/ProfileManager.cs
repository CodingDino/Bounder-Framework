// ************************************************************************ 
// File Name:   ProfileManager.cs 
// Purpose:    	
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;

// ************************************************************************ 
// Class: ProfileManager
// ************************************************************************ 
public class ProfileManager : Singleton<ProfileManager> {
	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	[SerializeField]
	private PlayerProfile m_baseProfile = null;
	[SerializeField]
	private PlayerProfile m_demoProfile = null;
	[SerializeField]
	private PlayerProfile m_profile = null;

	private bool m_loadedData = false;

	// ********************************************************************
	// Properties 
	// ********************************************************************
	public static PlayerProfile profile { 
		get {
			if (instance == null) return null;
			if (instance.m_profile == null)
				instance.m_profile = instance.m_baseProfile;
			return instance.m_profile;
		} 
		set {
			instance.m_profile = value;
		}
	}
	public static bool loadedData { 
		get {
			if (instance == null)
				return false;
			else
				return instance.m_loadedData;
		}
	}


	// ********************************************************************
	#region Events 
	// ********************************************************************
	public delegate void ProfileLoaded(PlayerProfile _profile);
	public static event ProfileLoaded OnProfileLoaded;
	#endregion
	// ********************************************************************
	
	// ********************************************************************
	public static void Load<T>(int slot) where T : PlayerProfile
	{
		string saveID = "SaveSlot" + slot.ToString();
		Load<T>(saveID);
	}
	public static void Load<T>(string saveID = "") where T : PlayerProfile
	{
		if (saveID == "")
			saveID = profile.name;
		
		if (DebugMenu.demoMode)
		{
			Load(instance.m_demoProfile);
		}
		else
		{
			string jsonString = PlayerPrefs.GetString (saveID);
			profile = JsonUtility.FromJson<T>(jsonString);
			Load(profile, false);
		}
	}
	public static void Load<T>(T _profile, bool _copy = true) where T : PlayerProfile
	{
		if (_copy)
		{
			profile = Instantiate(_profile);
			profile.name = _profile.name + "-Copy";
		}
		instance.m_loadedData = true;
		Debug.Log ("ProfileManager --- LOAD "+profile.name+" --- JSON String loaded: "+ profile.ToString());
		if (OnProfileLoaded != null)
			OnProfileLoaded(profile);
	}
	// ********************************************************************


	// ********************************************************************
	public static void Save(string saveID = "")
	{
		if (saveID == "")
			saveID = profile.name;

		string json = JsonUtility.ToJson(profile);

		PlayerPrefs.SetString(saveID, json);
		PlayerPrefs.Save();
		Debug.Log ("ProfileManager --- SAVE "+profile.name+" --- JSON String saved: "+ json);
	}
	// ********************************************************************

	
	// ********************************************************************
	public static void Clear()
	{
		Clear (profile.name);
	}
	public static void Clear(int slot)
	{
		string saveID = "SaveSlot" + slot.ToString();
		Clear (saveID);
	}
	public static void Clear(string saveID)
	{
		Debug.Log("ProfileManager --- CLEAR "+saveID);
		PlayerPrefs.DeleteKey(saveID);
	}
	// ********************************************************************

	
	// ********************************************************************
	public static void ClearAll()
	{
		PlayerPrefs.DeleteAll();
	}
	// ********************************************************************


	// ********************************************************************
	public static T GetProfile<T>() where T : PlayerProfile
	{
		return profile as T;
	}
	public static T GetProfile<T>(int slot) where T : PlayerProfile
	{
		string saveID = "SaveSlot" + slot.ToString();
		return GetProfile<T>(saveID);
	}
	public static T GetProfile<T>(string saveID) where T : PlayerProfile
	{
		string jsonString = PlayerPrefs.GetString (saveID);
		T thisProfile = JsonUtility.FromJson<T>(jsonString);
		if (thisProfile != null)
			thisProfile.name = saveID;
		return thisProfile;

	}
	// ********************************************************************
}