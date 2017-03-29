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
	public string cursor;
	public ChangeCursorEvent(string _cursor = "")
	{
		cursor = _cursor;
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
	private ControlScheme m_controlScheme;
	[SerializeField]
	private Animator[] m_cursorPrefabs;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	private Dictionary<string, Animator> m_cursorMap = new Dictionary<string,Animator>();
	private Animator m_cursor;
	private string m_defaultCursor;
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
			Vector2 currentScreenPoint = new Vector3(Input.mousePosition.x, 
			                                             Input.mousePosition.y, 
			                                             0);

			Vector2 currentWorldPoint = 
				Camera.main.ScreenToWorldPoint(currentScreenPoint);
			m_cursor.transform.position = currentWorldPoint;

			m_cursor.SetBool("Click",Input.GetMouseButton(0));
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
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void OnChangeCursorEvent(ChangeCursorEvent _event)
	{
		if (controlScheme != ControlScheme.MOUSE_KEYBOARD)
			return;

		if (_event.cursor == cursor)
			return;

		string toSwap = _event.cursor;
		if (toSwap.NullOrEmpty())
			toSwap = m_defaultCursor;

		if (!m_cursorMap.ContainsKey(toSwap))
		{
			Debug.LogError("Can't find cursor "+toSwap);
			return;
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
