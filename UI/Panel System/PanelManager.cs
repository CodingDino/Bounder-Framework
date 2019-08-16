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
using Bounder.Framework;
using Rewired;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: PanelManager
// ************************************************************************
public class PanelManager : Singleton<PanelManager>
{
    // ********************************************************************
    #region Private Data Members 
    // ********************************************************************
    private Dictionary<string, List<Panel>> m_groupStacks = new Dictionary<string, List<Panel>>();
    private List<Panel> m_activePanels = new List<Panel>();
    private List<Panel> m_closingPanels = new List<Panel>();
    private IList<Player> m_players;
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
    void Start()
    {
        m_players = ReInput.players.GetPlayers();
    }
    // ********************************************************************
    void Update()
    {
        int numPlayers = 1; // TODO: Update to support multiple player UI functions
        for (int iPlayer = 0; iPlayer < numPlayers; ++iPlayer)
        {
            if (m_players[iPlayer].GetButtonDown("Cancel"))
            {
                // Attempt to close top level panel if it allows this
                Panel topPanel = m_activePanels.Back();
                if (topPanel != null && topPanel.closeOnCancel)
                {
                    ClosePanel(topPanel);
                    break; // If multiple players are pressing cancel, don't process it twice.
                }
            }
        }
    }
    // ********************************************************************
    void OnEnable()
    {
        if (s_instance != this)
            return;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        DebugMenu.OnResetGame += CloseAllPanels;
#endif
    }
    // ********************************************************************
    void OnDisable()
    {
        if (s_instance != this)
            return;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        DebugMenu.OnResetGame -= CloseAllPanels;
#endif
    }
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Public Methods 
    // ********************************************************************
    public static Panel OpenPanel(Panel _prefab, PanelData _data = null)
    {
        if (_prefab == null)
        {
            Debug.LogError("OpenPanel() called with null prefab.");
            return null;
        }

        string panelGroup = _prefab.group;
        if (_data != null && !_data.panelGroup.NullOrEmpty())
            panelGroup = _data.panelGroup;
        PanelLimitOverride limitOverride = _data != null ? _data.limitOverride : PanelLimitOverride.REPLACE;

        bool atLimit = (!panelGroup.NullOrEmpty()
                        && instance.m_groupStacks.ContainsKey(panelGroup)
                        && instance.m_groupStacks[panelGroup].Count > 0);

        // Hide existing panel if we are replacing it
        Panel hidingPanel = null;
        if (atLimit && limitOverride == PanelLimitOverride.REPLACE)
        {
            hidingPanel = instance.m_groupStacks[panelGroup].Front();
        }

        // If we are allowed to show this new panel, do so
        if (!atLimit || limitOverride != PanelLimitOverride.NONE)
        {
            Panel newPanel = Instantiate<GameObject>(_prefab.gameObject, instance.transform, false).GetComponent<Panel>();
            newPanel.name = _prefab.name;
            newPanel.group = panelGroup;

            // Set sibling position (if no data, gets left where created - on top)
            if (_data != null)
            {
                switch (_data.siblingPosition)
                {
                    case PanelData.SiblingPosition.TOP:
                        {
                            newPanel.transform.SetAsLastSibling();
                            break;
                        }
                    case PanelData.SiblingPosition.BOTTOM:
                        {
                            newPanel.transform.SetAsFirstSibling();
                            break;
                        }
                    case PanelData.SiblingPosition.SPECIFIED:
                        {
                            newPanel.transform.SetSiblingIndex(_data.siblingIndex);
                            break;
                        }
                    default:
                        break;
                }
            }

            newPanel.Initialise(_data);
			if (!instance.m_activePanels.Empty())
			{
				instance.m_activePanels.Back().FocusChanged(false);
			}
            instance.m_activePanels.Add(newPanel);
            if (!instance.m_groupStacks.ContainsKey(panelGroup))
                instance.m_groupStacks[panelGroup] = new List<Panel>();

            if (atLimit && limitOverride == PanelLimitOverride.WAIT)
            {
                instance.m_groupStacks[panelGroup].Add(newPanel);
            }
            else
            {
                instance.m_groupStacks[panelGroup].AddAtFront(newPanel);
                instance.StartCoroutine(instance._OpenPanel(newPanel, hidingPanel));
            }

            //Debug.Log("Opening new panel: "+_prefab.name);
            return newPanel;
        }

        return null;
    }
    // ********************************************************************
    public static bool ClosePanel(string _id)
    {
        // If it isn't open we can't close it
        if (instance == null || !IsPanelActive(_id))
        {
            Debug.LogError("PanelManager.ClosePanel(" + _id + ") - no panel found of this name.");
            return false;
        }

        if (!IsPanelClosing(_id))
            instance.StartCoroutine(instance._ClosePanel(GetActivePanel(_id)));
        return true;
    }
    // ********************************************************************
    public static bool ClosePanel(Panel _panel)
    {
        // If it isn't open we can't close it
        if (!instance.m_activePanels.Contains(_panel))
        {
            Debug.LogError("PanelManager.ClosePanel(" + _panel + ") - no panel found.");
            return false;
        }

        if (!instance.m_closingPanels.Contains(_panel))
            instance.StartCoroutine(instance._ClosePanel(_panel));
        return true;
    }
    // ********************************************************************
    public static void CloseAllPanels()
    {
        List<Panel> activePanels = instance.m_activePanels.Copy();
        for (int i = 0; i < activePanels.Count; ++i)
        {
            ClosePanel(activePanels[i]);
        }
    }
    // ********************************************************************
    public static Panel GetActivePanel(string _panelName)
    {
        for (int i = 0; i < instance.m_activePanels.Count; ++i)
        {
            if (instance.m_activePanels[i].name == _panelName)
                return instance.m_activePanels[i];
        }
        return null;
    }
    // ********************************************************************
    public static bool IsPanelActive(string _panelName)
    {
        for (int i = 0; i < instance.m_activePanels.Count; ++i)
        {
            if (instance.m_activePanels[i].name == _panelName)
                return true;
        }
        return false;
    }
    // ********************************************************************
    public static bool IsPanelClosing(string _panelName)
    {
        for (int i = 0; i < instance.m_closingPanels.Count; ++i)
        {
            if (instance.m_closingPanels[i].name == _panelName)
                return true;
        }
        return false;
    }
    // ********************************************************************
    public static int NumPanelsOpenInGroup(string _group)
    {
        if (instance.m_groupStacks.ContainsKey(_group))
            return instance.m_groupStacks[_group].Count;
        else
            return 0;
    }
    // ********************************************************************
    public static int NumPanelsOpen()
    {
        return instance.m_activePanels.Count;
    }
    // ********************************************************************
    public static void SetActivePanelInteraction(bool _interactable)
    {
        for (int i = 0; i < instance.m_activePanels.Count; ++i)
        {
            instance.m_activePanels[i].interactable = _interactable;
        }
    }
    // ********************************************************************
    public static PanelState GetStateForPanel(string _panelName)
    {
        for (int i = 0; i < instance.m_activePanels.Count; ++i)
        {
            if (instance.m_activePanels[i].name == _panelName)
                return instance.m_activePanels[i].state;
        }
        return PanelState.CLOSED;
    }
    // ********************************************************************
    public static Panel AddPanel(Panel _panel)
    {
        string panelGroup = _panel.group;

        bool atLimit = (!panelGroup.NullOrEmpty()
                        && instance.m_groupStacks.ContainsKey(panelGroup)
                        && instance.m_groupStacks[panelGroup].Count > 0);

        // Hide existing panel if we are replacing it
        Panel hidingPanel = null;
        if (atLimit)
        {
            hidingPanel = instance.m_groupStacks[panelGroup].Front();
        }

        // If we are allowed to show this new panel, do so
        Panel newPanel = _panel;
        newPanel.name = _panel.name;
        newPanel.group = panelGroup;

        // Set sibling position (if no data, gets left where created - on top)
        newPanel.Initialise();
        instance.m_activePanels.Add(newPanel);
        if (!instance.m_groupStacks.ContainsKey(panelGroup))
            instance.m_groupStacks[panelGroup] = new List<Panel>();

        instance.m_groupStacks[panelGroup].AddAtFront(newPanel);
        instance.StartCoroutine(instance._OpenPanel(newPanel, hidingPanel));

        Debug.Log("Opening new panel: " + _panel.name);
        return newPanel;
    }
    // ********************************************************************
    public static bool IsTopPanel(Panel _panel)
    {
        return instance.m_activePanels.Back() == _panel;
    }
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    #region Private Methods 
    // ********************************************************************
    private IEnumerator _ClosePanel(Panel _panel)
    {
        //Debug.Log("Closing panel: "+_panel.name);
        m_closingPanels.Add(_panel);
        _panel.Hide();
        while (_panel.IsVisible())
            yield return null;

        // Clean up panel
        string group = _panel.group;
        _panel.Uninitialise();
        m_activePanels.Remove(_panel);
        if (!group.NullOrEmpty())
            m_groupStacks[group].Remove(_panel);
        if (OnPanelClosed != null)
            OnPanelClosed(_panel.name);
        m_closingPanels.Remove(_panel);
        Destroy(_panel.gameObject);

        // Re-show next panel in the group
        // If the next panel on the stack is already closing, leave it alone.
        // It will trigger the next one on the stack when it finishes closing.
        if (!group.NullOrEmpty() && m_groupStacks[group].Count > 0 && !m_closingPanels.Contains(m_groupStacks[group].Front()))
        {
            m_groupStacks[group].Front().Show();
        }
		m_activePanels.Back().FocusChanged(true);
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
