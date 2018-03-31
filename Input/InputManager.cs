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
	CONTROLLER		= 1 << 2,
	ALL				= TOUCH | MOUSE_KEYBOARD | CONTROLLER
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
#region Class: InputManager
// ************************************************************************
public class InputManager : Singleton<InputManager> 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private ControlScheme m_controlScheme = ControlScheme.MOUSE_KEYBOARD;
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
	#region Exposed Data Members 
	// ********************************************************************
	private Dictionary<string, Animator> m_cursorMap = new Dictionary<string,Animator>();
	private Animator m_cursor;
	private string m_defaultCursor;
	private Vector3 m_mousePositionLastFrame;
	private float m_lastInputDetected;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public static ControlScheme controlScheme 
	{ 
		get { return instance.m_controlScheme; } 
		set { instance.m_controlScheme = value; } 
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
		// Determine control scheme
		switch(SystemInfo.deviceType)
		{
		case DeviceType.Handheld:
			m_controlScheme = ControlScheme.TOUCH;
			break;
		case DeviceType.Desktop:
			if (Input.GetJoystickNames().Length > 0)
			{
				m_controlScheme = ControlScheme.CONTROLLER;
			}
			else if (Input.mousePresent)
			{
				m_controlScheme = ControlScheme.MOUSE_KEYBOARD;
			}
			break;
		case DeviceType.Console:
			m_controlScheme = ControlScheme.CONTROLLER;
			break;
		default:
			Debug.LogError("Unrecognized device type - unable to determine proper control scheme");
			break;
		}
		// TODO: Real time change between control schemes


		if (m_cursorPrefabs != null && m_cursorPrefabs.Length > 0)
		{
			Cursor.visible = false;
			for (int i = 0; i < m_cursorPrefabs.Length; ++i)
			{
				m_cursorMap[m_cursorPrefabs[i].name] = m_cursorPrefabs[i];
			}
			m_defaultCursor = m_cursorPrefabs[0].name;

			StartCoroutine(SwapCursors(m_defaultCursor));
		}
	}
	// ********************************************************************
	void OnGUI () 
	{
		if (controlScheme == ControlScheme.MOUSE_KEYBOARD && m_cursor != null)
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

		switch (InputManager.controlScheme)
		{
		case ControlScheme.MOUSE_KEYBOARD:
			if(Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(0) || Input.mousePosition != m_mousePositionLastFrame)
			{
				m_lastInputDetected = Time.realtimeSinceStartup;
			}
			m_mousePositionLastFrame = Input.mousePosition;
			break;
		case ControlScheme.CONTROLLER:
			if (Input.anyKey) // TODO: handle joystick axis
			{
				m_lastInputDetected = Time.realtimeSinceStartup;
			}
			break;
		case ControlScheme.TOUCH:
			if(Input.touchCount > 0)
			{
				m_lastInputDetected = Time.realtimeSinceStartup;
			}
			break;
		default:
			break;
		}

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
				DebugMenu.TriggerResetEvent();
				LoadingSceneManager.LoadScene(0); // Load title
			}
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
		if (controlScheme != ControlScheme.MOUSE_KEYBOARD)
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
		StartCoroutine(SwapCursors(toSwap));
	}
	// ********************************************************************
	private IEnumerator SwapCursors(string _new)
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
		yield return null;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
