// ************************************************************************ 
// File Name:   InputManager.cs 
// Purpose:    	
// Project:		
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BounderFramework;
using Rewired;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Enum: ControlScheme
// ************************************************************************
[Flags] 
public enum ControlScheme
{
	NONE 			= 0,
	TOUCH			= 1 << 0,
	MOUSE_KEYBOARD	= 1 << 1,
	GAMEPAD			= 1 << 2,
	ALL				= TOUCH | MOUSE_KEYBOARD | GAMEPAD
}
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ChangeCursorEvent
// ************************************************************************
public class ChangeCursorEvent : GameEvent 
{
	public string cursorID;
	public Animator cursor;
	public ChangeCursorEvent(string _cursor = "")
	{
		cursorID = _cursor;
	}
	public ChangeCursorEvent(Animator _cursor)
	{
		cursor = _cursor;
		cursorID = _cursor.name;
	}
}
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ControlSchemeChangedEvent
// ************************************************************************
public class ControlSchemeChangedEvent : GameEvent 
{
	public ControlScheme previousScheme = ControlScheme.NONE;
	public ControlScheme newScheme = ControlScheme.NONE;

	public ControlSchemeChangedEvent(ControlScheme _previousScheme, ControlScheme _newScheme)
	{
		previousScheme = _previousScheme;
		newScheme = _newScheme;
	}
	public ControlSchemeChangedEvent(ControlScheme _newScheme = ControlScheme.NONE)
	{
		newScheme = _newScheme;
	}
}
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: InputManager
// ************************************************************************
public class InputManager : Singleton<InputManager> 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private Animator[] m_cursorPrefabs;
	[SerializeField]
	private bool m_shouldTimeOut = false;
	[SerializeField]
	[ShowInInspectorIf("m_shouldTimeOut")]
	private Panel m_quitPanelPrefab;
	[SerializeField]
	[ShowInInspectorIf("m_shouldTimeOut")]
	private float m_secondsToDemoExitPopup = 100;
	[SerializeField]
	[ShowInInspectorIf("m_shouldTimeOut")]
	private float m_secondsToDemoTimeOut = 500;
	[SerializeField]
	private List<string> m_excludedScenesForTimeOut = new List<string>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Dictionary<string, Animator> m_cursorMap = new Dictionary<string,Animator>();
	private Animator m_cursor;
	private string m_defaultCursor;
	private Vector3 m_mousePositionLastFrame;
	private float m_lastInputDetected;
	private ControlScheme m_lastControlScheme;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public static ControlScheme controlScheme 
	{ 
		get { 
			ControllerType lastUsed = ReInput.controllers.GetLastActiveControllerType();

			// Touch
			if (SystemInfo.deviceType == DeviceType.Handheld)
				return ControlScheme.TOUCH;

			// Mouse and keyboard
			if (lastUsed == ControllerType.Mouse || lastUsed == ControllerType.Keyboard)
				return ControlScheme.MOUSE_KEYBOARD;

			// Gamepad
			if (lastUsed == ControllerType.Joystick)
				return ControlScheme.GAMEPAD;

			// Unknown control scheme
			return ControlScheme.NONE;
		} 
	}
	public static string cursor
	{
		get { return instance.m_cursor == null ? "" : instance.m_cursor.name; }
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void Start()
	{
		if (m_cursorPrefabs != null && m_cursorPrefabs.Length > 0)
		{
			Cursor.visible = false;
			for (int i = 0; i < m_cursorPrefabs.Length; ++i)
			{
				m_cursorMap[m_cursorPrefabs[i].name] = m_cursorPrefabs[i];
			}
			m_defaultCursor = m_cursorPrefabs[0].name;

			SwapCursors(m_defaultCursor);
		}
	}
	// ********************************************************************
	void OnGUI () 
	{
		if (InputManager.controlScheme == ControlScheme.MOUSE_KEYBOARD && m_cursor != null)
		{
			m_cursor.gameObject.SetActive(true);
			Vector2 currentScreenPoint = new Vector3(Input.mousePosition.x, 
			                                             Input.mousePosition.y, 
			                                             0);

			Vector2 currentWorldPoint = 
				Camera.main.ScreenToWorldPoint(currentScreenPoint);
			m_cursor.transform.position = currentWorldPoint;

			if (m_cursor.HasParameterOfType("Click",AnimatorControllerParameterType.Bool))
				m_cursor.SetBool("Click",Input.GetMouseButton(0));

			Cursor.visible = false;
		}
		else
		{
			m_cursor.gameObject.SetActive(false);
		}
	}
	// ********************************************************************
	void OnEnable()
	{
		Events.AddListener<ChangeCursorEvent>(OnChangeCursorEvent);
	}
	// ********************************************************************
	void OnDisable()
	{
		Events.RemoveListener<ChangeCursorEvent>(OnChangeCursorEvent);
	}
	// ********************************************************************
	void Update()
	{
		// Check for changed input
		bool anyButtonPressed = ReInput.controllers.GetAnyButton();
		bool mousePositionChanged = Input.mousePosition != m_mousePositionLastFrame;
		if (anyButtonPressed || mousePositionChanged)
			m_lastInputDetected = Time.realtimeSinceStartup;

		// Record mouse position
		m_mousePositionLastFrame = Input.mousePosition;

		// Determine if time out has been reached
		string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		if (m_shouldTimeOut && !m_excludedScenesForTimeOut.Contains(currentSceneName))
		{
			if (Time.realtimeSinceStartup >= m_lastInputDetected + m_secondsToDemoExitPopup && PanelManager.GetStateForPanel(m_quitPanelPrefab.name) == PanelState.CLOSED)
			{
				PanelManager.OpenPanel(m_quitPanelPrefab);
			}
			if (Time.realtimeSinceStartup >= m_lastInputDetected + m_secondsToDemoTimeOut)
			{
				PanelManager.ClosePanel(m_quitPanelPrefab.name);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				DebugMenu.TriggerResetEvent();
#endif
				LoadingSceneManager.LoadScene(0); // Load title
			}
		}

		// Determine if input type has changed
		ControlScheme newScheme = controlScheme;
		if (m_lastControlScheme != newScheme)
		{
			Events.Raise(new ControlSchemeChangedEvent(m_lastControlScheme, newScheme));
			m_lastControlScheme = newScheme;
		}
	}
	// ********************************************************************
#endregion
	// ********************************************************************


	// ********************************************************************
#region Private Methods 
	// ********************************************************************
	private void OnChangeCursorEvent(ChangeCursorEvent _event)
	{
		ControllerType lastUsed = ReInput.controllers.GetLastActiveControllerType();
		if (lastUsed != ControllerType.Mouse && lastUsed != ControllerType.Keyboard)
			return;

		if (_event.cursorID == cursor)
			return;

		string toSwap = _event.cursorID;
		if (toSwap.NullOrEmpty())
			toSwap = m_defaultCursor;

		if (!m_cursorMap.ContainsKey(toSwap))
		{
			if (_event.cursor != null)
			{
				m_cursorMap[_event.cursorID] = _event.cursor;
			}
			else
			{
				Debug.LogError("Can't find cursor "+toSwap);
				return;
			}
		}
		SwapCursors(toSwap);
	}
	// ********************************************************************
	private void SwapCursors(string _new)
	{
		Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, 
		                                         Input.mousePosition.y, 
		                                         0);
		Vector3 currentWorldPoint = 
			Camera.main.ScreenToWorldPoint(currentScreenPoint);

		Animator pendingCursor = Instantiate(m_cursorMap[_new].gameObject,transform).GetComponent<Animator>();
		pendingCursor.name = _new;
		pendingCursor.transform.position = currentWorldPoint;
		if (m_cursor != null)
			Destroy(m_cursor.gameObject);
		m_cursor = pendingCursor;
		m_cursor.SetBool("Active", true);
		return;
	}
	// ********************************************************************
#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
