// ************************************************************************ 
// File Name:   PanelManager.cs 
// Purpose:    	Controls hiding and showing of UI panels
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: PanelManager
// ************************************************************************
public class PanelManager : Singleton<PanelManager> 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private List<Panel> m_panelPrefabs = new List<Panel>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string, Panel> m_panelPrefabMap = new Dictionary<string, Panel>();
	private Dictionary<string, List<Panel>> m_groupStacks = new Dictionary<string, List<Panel>>();
	private Dictionary<string,Panel> m_activePanelMap = new Dictionary<string,Panel>();
	private Dictionary<string,Panel> m_closingPanels = new Dictionary<string,Panel>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region  Events 
	// ********************************************************************
	public delegate void PanelClosed(string _panelName);
	public static event PanelClosed OnPanelClosed;
	public delegate void PanelOpened(string _panelName);
	public static event PanelOpened OnPanelOpened;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void Awake () 
	{
		for (int i = 0; i < m_panelPrefabs.Count; ++i)
		{
			m_panelPrefabMap[m_panelPrefabs[i].name] = m_panelPrefabs[i];
		}
	}
	// ********************************************************************
	void OnEnable()
	{
		DebugMenu.OnResetGame += CloseAllPanels;
	}
	// ********************************************************************
	void OnDisable()
	{
		DebugMenu.OnResetGame -= CloseAllPanels;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public static Panel OpenPanel (string _id, JSON _data) 
	{
		// Don't try to show if we don't have the prefab
		if (!instance.m_panelPrefabMap.ContainsKey(_id))
			return null;

		// Don't allow duplicate panels of the same ID
		if (instance.m_activePanelMap.ContainsKey(_id))
			return null;

		// Get prefab from map
		Panel prefab = instance.m_panelPrefabMap[_id];

		PanelData data = prefab.CreatePanelData(_data);

		return OpenPanel(_id, data);
	}
	// ********************************************************************
	public static Panel OpenPanel (string _id, PanelData _data = null) 
	{
		// Don't try to show if we don't have the prefab
		if (!instance.m_panelPrefabMap.ContainsKey(_id))
			return null;

		// Don't allow duplicate panels of the same ID
		if (instance.m_activePanelMap.ContainsKey(_id))
			return null;
		
		// Get prefab from map
		Panel prefab = instance.m_panelPrefabMap[_id];
		return OpenPanel(prefab,_data);
	}
	// ********************************************************************
	public static Panel OpenPanel (Panel _prefab, PanelData _data = null)
	{
		PanelLimitOverride limitOverride = _data != null ? _data.limitOverride : PanelLimitOverride.REPLACE;

		bool atLimit = (   !_prefab.group.NullOrEmpty() 
			&&  instance.m_groupStacks.ContainsKey(_prefab.group)
			&&  instance.m_groupStacks[_prefab.group].Count > 0 );

		// Hide existing panel if we are replacing it
		Panel hidingPanel = null;
		if (atLimit && limitOverride == PanelLimitOverride.REPLACE)
		{
			hidingPanel = instance.m_groupStacks[_prefab.group].Front();
		}

		// If we are allowed to show this new panel, do so
		if (!atLimit || limitOverride != PanelLimitOverride.NONE)
		{
			Panel newPanel = Instantiate<GameObject>(_prefab.gameObject,instance.transform,false).GetComponent<Panel>();
			newPanel.name = _prefab.name;

			newPanel.Initialise(_data);
			instance.m_activePanelMap[newPanel.name] = newPanel;
			if (!instance.m_groupStacks.ContainsKey(newPanel.group))
				instance.m_groupStacks[newPanel.group] = new List<Panel>();
			instance.m_groupStacks[newPanel.group].AddAtFront(newPanel);

			instance.StartCoroutine(instance._OpenPanel(newPanel,hidingPanel));

			return newPanel;
		}

		return null;
	}
	// ********************************************************************
	public static bool ClosePanel (string _id) 
	{
		// If it isn't open we can't close it
		if (!instance.m_activePanelMap.ContainsKey(_id))
		{
			Debug.LogError("PanelManager.ClosePanel("+_id+") - no panel found of this name.");
			return false;
		}

		if (!instance.m_closingPanels.ContainsKey(_id))
			instance.StartCoroutine(instance._ClosePanel(instance.m_activePanelMap[_id]));
		return true;
	}
	// ********************************************************************
	public static bool ClosePanel (Panel _panel) 
	{
		// If it isn't open we can't close it
		if (!instance.m_activePanelMap.ContainsValue(_panel))
		{
			Debug.LogError("PanelManager.ClosePanel("+_panel+") - no panel found.");
			return false;
		}

		if (!instance.m_closingPanels.ContainsValue(_panel))
			instance.StartCoroutine(instance._ClosePanel(_panel));
		return true;
	}
	// ********************************************************************
	public static void CloseAllPanels()
	{
		foreach (KeyValuePair<string,Panel> panel in instance.m_activePanelMap)
		{
			ClosePanel(panel.Key);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private IEnumerator _ClosePanel(Panel _panel)
	{
		m_closingPanels[_panel.name] = _panel;
		_panel.Hide();
		while (_panel.IsVisible())
			yield return null;

		// Clean up panel
		string group = _panel.group;
		_panel.Uninitialise();
		m_activePanelMap.Remove(_panel.name);
		if (!group.NullOrEmpty())
			m_groupStacks[group].Remove(_panel);
		if (OnPanelClosed != null)
			OnPanelClosed(_panel.name);
		m_closingPanels.Remove(_panel.name);
		Destroy(_panel.gameObject);

		// Re-show next panel in the group
		if (!group.NullOrEmpty() && m_groupStacks[group].Count > 0)
			m_groupStacks[group].Front().Show();
	}
	// ********************************************************************
	private IEnumerator _OpenPanel(Panel _panel, Panel _hidingPanel)
	{
		// Hide existing panel
		if (_hidingPanel != null)
		{
			_hidingPanel.Hide();
			while (_hidingPanel.IsVisible())
				yield return null;
		}

		if (OnPanelOpened != null)
			OnPanelOpened(_panel.name);

		// Show new panel
		_panel.Show();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
