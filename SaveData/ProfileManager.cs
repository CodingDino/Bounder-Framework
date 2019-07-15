﻿// ************************************************************************ 
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
using Bounder.Framework;

// ************************************************************************ 
// Class: ProfileManager
// ************************************************************************ 
public class ProfileManager : Singleton<ProfileManager> 
{
	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	[SerializeField]
	private PlayerProfile m_profile = null;

	private bool m_loadedData = false;

	// ********************************************************************
	// Properties 
	// ********************************************************************
	public static PlayerProfile profile { 
		get {
			if (instance == null) 
				return null;
			else
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
	public static string lastPlayedID {
		get {
			return PlayerPrefs.GetString("LastPlayed");
		}
	}
	public static bool hasSaveData {
		get {
			return !lastPlayedID.NullOrEmpty();
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
	public static T Load<T>(int slot) where T : PlayerProfile
	{
		return Load(GetProfile<T>(slot));
	}
	public static T Load<T>(string saveID) where T : PlayerProfile
    {
		return Load(GetProfile<T>(saveID));
	}
	public static T Load<T>(T _profile, bool _copy = true, string copyName = "") where T : PlayerProfile
	{
		if (_profile == null)
		{
			Debug.LogError("Attempt to load null profile.");
			return null;
		}

		if (_copy)
		{
			instance.m_profile = Instantiate(_profile);
			if (copyName.NullOrEmpty())
				instance.m_profile.name = _profile.name + "-Copy";
			else
				instance.m_profile.name = copyName;
		}
		else
		{
			instance.m_profile = _profile;
		}
		Debug.Log ("ProfileManager --- LOAD "+instance.m_profile.name+" --- JSON String loaded: "+ instance.m_profile.ToString());
		instance.m_profile.Validate();
		instance.m_loadedData = true;
		if (OnProfileLoaded != null)
			OnProfileLoaded(instance.m_profile);

		return instance.m_profile as T;
	}
	// ********************************************************************



	// ********************************************************************
	public static void RecordLastPlayed(int slot) 
	{
		RecordLastPlayed(GetProfile<PlayerProfile>(slot));
	}
	public static void RecordLastPlayed(string saveID)
    {
		RecordLastPlayed(GetProfile<PlayerProfile>(saveID));
		PlayerPrefs.SetString("LastPlayed", instance.m_profile.name);
		PlayerPrefs.Save();
	}
	public static void RecordLastPlayed(PlayerProfile profile)
	{
		if (profile == null)
		{
			Debug.LogError("Attempt to record last played for null profile.");
		}

		// Record most recent saved data
		PlayerPrefs.SetString("LastPlayed", instance.m_profile.name);
		PlayerPrefs.Save();
	}
	// ********************************************************************


	// ********************************************************************
	public static void Save<T>() where T : PlayerProfile
	{
		string saveID = profile.name;

		T castProfile = (T)profile;
		string json = JsonUtility.ToJson(castProfile);

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
	[ContextMenu("Clear All Save Data")]
	public void ClearAll()
	{
		Debug.Log("ProfileManager --- CLEAR ALL SAVE DATA");
		PlayerPrefs.DeleteAll();
	}
	// ********************************************************************


	// ********************************************************************
	public static PlayerProfile GetActiveProfile()
	{
		return profile;
	}
	public static T GetActiveProfile<T>() where T : PlayerProfile
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
		string jsonString = PlayerPrefs.GetString(saveID);

		Debug.Log("Json for save "+saveID+": "+jsonString);

		if (jsonString.NullOrEmpty())
			return null;

		T thisProfile = ScriptableObject.CreateInstance(typeof(T)) as T;
		JsonUtility.FromJsonOverwrite(jsonString, thisProfile);
		thisProfile.name = saveID;
		return thisProfile;

	}
	// ********************************************************************
}