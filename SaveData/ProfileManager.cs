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
	public static bool profileIsLoaded { 
		get {
			if (instance == null)
				return false;
			else
				return instance.m_profile != null;
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
	public delegate void ProfileCallback(PlayerProfile _profile);
	public static event ProfileCallback OnProfileLoaded;
	public static event ProfileCallback OnProfileUnLoaded;
	#endregion
	// ********************************************************************
	
	
	// ********************************************************************
	public static string GetNameFromSlot(int slot)
	{
		return "SaveSlot" + slot;
	}
	// ********************************************************************
	public static T CopyProfile<T>(T toCopy, int slot) where T : PlayerProfile
	{
		return CopyProfile(toCopy, GetNameFromSlot(slot));
	}
	// ********************************************************************
	public static T CopyProfile<T>(T toCopy, string newCopyName = "") where T : PlayerProfile
	{
		T thisProfile = Instantiate(toCopy);
		if (!newCopyName.NullOrEmpty())
			thisProfile.name = newCopyName;
		return thisProfile;
	}
	// ********************************************************************


	// ********************************************************************
	public static T Load<T>(int slot) where T : PlayerProfile
	{
		T toLoad = GetProfile<T>(slot);
		return Load(toLoad);
	}
	public static T Load<T>(string saveID) where T : PlayerProfile
    {
		T toLoad = GetProfile<T>(saveID);
		return Load(toLoad);
	}
	public static T Load<T>(T _profile) where T : PlayerProfile
	{
		if (_profile == null)
		{
			Debug.LogError("Attempt to load null profile.");
			return null;
		}

		instance.m_profile = _profile;
		Debug.Log ("ProfileManager --- LOAD "+instance.m_profile.name+" --- JSON String loaded: "+ instance.m_profile.ToString());
		instance.m_profile.Validate();
		if (OnProfileLoaded != null)
			OnProfileLoaded(instance.m_profile);

		return instance.m_profile as T;
	}
	// ********************************************************************


	// ********************************************************************
	public static void UnloadActiveProfile()
	{
		PlayerProfile oldProfile = instance.m_profile;
		instance.m_profile = null;
		if (OnProfileUnLoaded != null)
			OnProfileUnLoaded(oldProfile);
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
	public static void Save()
	{
		Save(profile);
	}
	public static void Save(PlayerProfile _profile)
	{
		string saveID = _profile.name;
		string json = JsonUtility.ToJson(_profile);

		PlayerPrefs.SetString(saveID, json);
		PlayerPrefs.Save();
		Debug.Log ("ProfileManager --- SAVE "+_profile.name+" --- JSON String saved: "+ json);
	}
	// ********************************************************************

	
	// ********************************************************************
	public static void Clear()
	{
		Clear (profile.name);
	}
	public static void Clear(int slot)
	{
		string saveID = GetNameFromSlot(slot);
		Clear (saveID);
	}
	public static void Clear(string saveID)
	{
		Debug.Log("ProfileManager --- CLEAR "+saveID);
		PlayerPrefs.DeleteKey(saveID);
	}
	public static void ClearAll()
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
		string saveID = GetNameFromSlot(slot);
		return GetProfile<T>(saveID);
	}
	public static T GetProfile<T>(string saveID) where T : PlayerProfile
	{
		string jsonString = PlayerPrefs.GetString(saveID);

		if (jsonString.NullOrEmpty())
			return null;

		T thisProfile = ScriptableObject.CreateInstance(typeof(T)) as T;
		JsonUtility.FromJsonOverwrite(jsonString, thisProfile);
		thisProfile.name = saveID;
		return thisProfile;

	}
	// ********************************************************************
}