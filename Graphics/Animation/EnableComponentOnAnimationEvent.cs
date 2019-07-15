// ************************************************************************ 
// File Name:   EnableComponentOnAnimationEvent.cs 
// Purpose:    	Enable a component on animation event
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: EnableComponentOnAnimationEvent
// ************************************************************************
public class EnableComponentOnAnimationEvent : MonoBehaviour 
{
	// ********************************************************************
	#region Class: ComponentData
	// ********************************************************************
	[System.Serializable]
	private class ComponentData
	{
		public string id = "";
		public MonoBehaviour component = null;
		public bool enable = true;
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private List<ComponentData> m_components = new List<ComponentData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string,ComponentData> m_componentMap = new Dictionary<string, ComponentData>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		for (int i = 0; i < m_components.Count; ++i)
		{
			ComponentData data = m_components[i];
			if (m_componentMap.ContainsKey(data.id))
				Debug.LogError("Duplicate ID found: "+data.id);
			else
				m_componentMap[data.id] = data;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void EnableComponent (string _id) 
	{
		ComponentData data = m_componentMap[_id];
		data.component.enabled = data.enable;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
