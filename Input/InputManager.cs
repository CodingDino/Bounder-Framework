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
using System.Collections.Generic;
using UnityEngine;

#if USE_REWIRED
    using Rewired;
#endif
#endregion
// ************************************************************************

namespace Bounder.Framework
{
    // ************************************************************************ 
    #region Enum: ControlScheme
    // ************************************************************************
    [Flags]
    public enum ControlScheme
    {
        NONE = 0,
        TOUCH = 1 << 0,
        MOUSE = 1 << 1,
        GAMEPAD = 1 << 2,
        KEYBOARD = 1 << 3,
        MOUSE_KEYBOARD = MOUSE | KEYBOARD,
        ALL = TOUCH | MOUSE | KEYBOARD | GAMEPAD
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
        private Animator[] m_cursorPrefabs = null;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Data Members 
        // ********************************************************************
        private Dictionary<string, Animator> m_cursorMap = new Dictionary<string, Animator>();
        private Animator m_cursor = null;
        private string m_defaultCursor = "";
        private Vector3 m_mousePositionLastFrame = Vector3.zero;
        private float m_lastInputDetected = 0f;
        private ControlScheme m_lastControlScheme = ControlScheme.NONE;
#if USE_REWIRED
            private IList<Player> m_players;
#endif
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Properties 
        // ********************************************************************
        public static ControlScheme controlScheme
        {
            get
            {
#if USE_REWIRED
                ControllerType lastUsed = ReInput.controllers.GetLastActiveControllerType();
#endif

                // Touch
                if (SystemInfo.deviceType == DeviceType.Handheld)
                    return ControlScheme.TOUCH;

                // Mouse and keyboard
#if USE_REWIRED
                if (lastUsed == ControllerType.Mouse)
                    return ControlScheme.MOUSE;
                if (lastUsed == ControllerType.Keyboard)
                    return ControlScheme.KEYBOARD;

                // Gamepad
                if (lastUsed == ControllerType.Joystick)
                    return ControlScheme.GAMEPAD;
#else
                // if no rewired, default to mouse/keyboard
                else
                    return ControlScheme.MOUSE_KEYBOARD;
#endif

                // Unknown control scheme
                return ControlScheme.NONE;
            }
        }

        public static bool useDirectionalUINavigation 
        {
            get
            {
                ControlScheme controls = controlScheme;
                return controls == ControlScheme.GAMEPAD || controls == ControlScheme.KEYBOARD;
            }
        }

        public static string cursor
        {
            get { return instance.m_cursor == null ? "" : instance.m_cursor.name; }
        }
        public static float timeSinceLastInput
        {
            get { return Time.realtimeSinceStartup - instance.m_lastInputDetected; }
        }
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Static Methods 
        // ********************************************************************
        public static bool GetButtonDownForFirstPlayer(string buttonName)
        {
            return GetButtonDownForPlayer(buttonName, 0);
        }
        // ********************************************************************
        public static bool GetButtonDownForPlayer(string buttonName, int playerNumber)
        {
#if USE_REWIRED
            return instance.m_players[playerNumber].GetButtonDown(buttonName);
#else
            //Debug.LogWarning("GetButtonDownForPlayer() not supported without Rewired");
            return Input.GetButtonDown(buttonName);
#endif
        }
        // ********************************************************************
        public static bool GetButtonDownForAnyPlayer(string buttonName)
        {
#if USE_REWIRED
            int numPlayers = instance.m_players.Count; // could allow less players than the rewired number?
            for (int iPlayer = 0; iPlayer < numPlayers; ++iPlayer)
            {
                if (GetButtonDownForPlayer(buttonName,iPlayer))
                    return true;
            }
            return false;
#else
            //Debug.LogWarning("GetButtonDownForAnyPlayer() not supported without Rewired");
            return Input.GetButtonDown(buttonName);
            #endif
        }
        // ********************************************************************
        public static bool GetAnyButtonDown()
        {
#if USE_REWIRED
            return ReInput.controllers.GetAnyButtonDown();
#else
            return Input.anyKey;
#endif
        }
        // ********************************************************************
        public static float GetAxisForFirstPlayer(string axisName)
        {
            return GetAxisForPlayer(axisName, 0);
        }
        // ********************************************************************
        public static float GetAxisForPlayer(string axisName, int playerNumber)
        {
#if USE_REWIRED
            return instance.m_players[playerNumber].GetAxis(axisName);
#else
            //Debug.LogWarning("GetAxisForPlayer() not supported without Rewired");
            return Input.GetAxis(axisName);
#endif
        }
        // ********************************************************************
        public static float GetAxisForAnyPlayer(string axisName)
        {
#if USE_REWIRED
            int numPlayers = instance.m_players.Count; // could allow less players than the rewired number?
            for (int iPlayer = 0; iPlayer < numPlayers; ++iPlayer)
            {
                float axisVal = GetAxisForPlayer(axisName, iPlayer);
                if (axisVal != 0)
                    return axisVal;
            }
            return 0; // default if there were no player input.
#else
            //Debug.LogWarning("GetAxisForAnyPlayer() not supported without Rewired");
            return Input.GetAxis(axisName);
#endif
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region MonoBehaviour Methods 
        // ********************************************************************
        void Awake()
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
        void Start()
        {
#if USE_REWIRED
            m_players = ReInput.players.GetPlayers();
#endif
        }
        // ********************************************************************
        void OnGUI()
        {
            if (InputManager.controlScheme == ControlScheme.MOUSE_KEYBOARD && m_cursor != null)
            {
                m_cursor.SetBool("Active", true);
                Vector2 currentScreenPoint = new Vector3(Input.mousePosition.x,
                                                             Input.mousePosition.y,
                                                             0);

                Vector2 currentWorldPoint =
                    Camera.main.ScreenToWorldPoint(currentScreenPoint);
                m_cursor.transform.position = currentWorldPoint;

                if (m_cursor.HasParameterOfType("Click", AnimatorControllerParameterType.Bool))
                    m_cursor.SetBool("Click", Input.GetMouseButton(0));

                Cursor.visible = false;
            }
            else if (m_cursor != null)
            {
                m_cursor.SetBool("Active", false);
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
            bool anyButtonPressed = GetAnyButtonDown();
            bool mousePositionChanged = Input.mousePosition != m_mousePositionLastFrame;
            if (anyButtonPressed || mousePositionChanged)
                m_lastInputDetected = Time.realtimeSinceStartup;

            // Record mouse position
            m_mousePositionLastFrame = Input.mousePosition;

            // Determine if input type has changed
            ControlScheme newScheme = controlScheme;
            if (m_lastControlScheme != newScheme)
            {
                Debug.Log("Control scheme changed from "+m_lastControlScheme+" to "+newScheme);
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
#if USE_REWIRED
            ControllerType lastUsed = ReInput.controllers.GetLastActiveControllerType();
            if (lastUsed != ControllerType.Mouse && lastUsed != ControllerType.Keyboard)
                return;
#endif

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
                    Debug.LogError("Can't find cursor " + toSwap);
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

            Animator pendingCursor = Instantiate(m_cursorMap[_new].gameObject, transform).GetComponent<Animator>();
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

}