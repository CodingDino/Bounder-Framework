#if UNITY_EDITOR || DEVELOPMENT_BUILD

// ************************************************************************ 
// File Name:   DebugMenu.cs 
// Purpose:    	Basic debugging functionality
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BounderFramework;

// ************************************************************************ 
// Class: DebugMenu
// ************************************************************************ 
public class DebugMenu : Singleton<DebugMenu>
{


    // ********************************************************************
    // Struct: Log 
    // ********************************************************************
    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }


    // ********************************************************************
    // Delegates 
    // ********************************************************************
    public delegate void DebugButtonCallback(string _id, GameObject _button);


    // ********************************************************************
    // Static Data Members 
    // ********************************************************************
    static readonly Dictionary<LogType, string> s_logTypeColors = new Dictionary<LogType, string>()
	{
		{ LogType.Assert, "black" },
		{ LogType.Error, "red" },
		{ LogType.Exception, "red" },
		{ LogType.Log, "black" },
		{ LogType.Warning, "orange" },
	};


    // ********************************************************************
    // Exposed Data Members 
    // ********************************************************************
    
    [Header("Settings")]
    [SerializeField]
    private bool m_useRichText = true;
    [SerializeField]
    private string m_debugKey = "]";
    [SerializeField]
    private bool m_openWith4FingerTouch = true;
    [SerializeField]
    private float m_touchCooldown = 0.5f;
    [SerializeField]
    private int m_numLogsKept = 100;
    [SerializeField]
    private Text m_versionText = null;
    [SerializeField]
    private bool m_showVersionText = true;
    [SerializeField]
    private string m_versionNumber = "v1.0";
    [SerializeField]
    private bool m_previewMode = true;
    [SerializeField]
    private bool m_eventMode = true;

    [Header("Elements")]
    [SerializeField]
    private GameObject m_visibleElements = null;
    [SerializeField]
    private Text m_consoleTextBox = null;
    [SerializeField]
    private Scrollbar m_scrollbar = null;
    [SerializeField]
    private Image m_showErrorButton = null;
    [SerializeField]
    private Image m_showWarningsButton = null;
    [SerializeField]
    private Image m_showLogsButton = null;
    [SerializeField]
    private Image m_jumpToBottomButton = null;
    [SerializeField]
    private Transform m_buttonGrid = null;
    [SerializeField]
    private GameObject m_buttonPrototype = null;


    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
    private List<Log> m_logs = new List<Log>();
    private bool m_callbackRegistered = false;
    private bool m_showErrors = true;
    private bool m_showWarnings = true;
    private bool m_showLogs = true;
    private bool m_jumpToBottom = true;
    private float m_timeScale = 0;
    private List<GameObject> m_debugButtons = new List<GameObject>();
    private List<DebugButtonCallback> m_debugButtonCallbacks = new List<DebugButtonCallback>();
    private float m_last4FingerTouch = 0;


    // ********************************************************************
    // Properties 
    // ********************************************************************
    public static bool previewMode 
	{
		get 
		{ 
			if (instance == null)
			{
				LogManager.Log("Attempt to access property while uninitialised.",
				               LogCategory.INPUT,
				               LogSeverity.ERROR, 
				               "DebugMenu");
			}
			return instance != null ? instance.m_previewMode : false; 
		}
    }
    public static bool eventMode
    {
        get
        {
            if (instance == null)
            {
                LogManager.Log("Attempt to access property while uninitialised.",
                               LogCategory.INPUT,
                               LogSeverity.ERROR,
                               "DebugMenu");
            }
            return instance != null ? instance.m_eventMode : false;
        }
    }


    // ********************************************************************
    // Delegates 
    // ********************************************************************
    public delegate void ModeToggle(bool _modeActive);
	public static event ModeToggle OnPreviewModeToggle;
    public static event ModeToggle OnEventModeToggle;
    public delegate void ResetGame();
	public static event ResetGame OnResetGame;
	public static void TriggerResetEvent() { if (OnResetGame != null) OnResetGame(); }


	// ********************************************************************
	// Function:	Start()
	// ********************************************************************
	void Start()
	{
		// Create preview button
		GameObject previewButton = AddButton("preview", "Preview Mode", PreviewButtonPressed);
        previewButton.GetComponent<Image>().color = m_previewMode ? Color.green : Color.red;

        // Create event button
        GameObject eventButton = AddButton("event", "Event Mode", EventButtonPressed);
        eventButton.GetComponent<Image>().color = m_eventMode ? Color.green : Color.red;


        // TODO: Create reset button
        AddButton("reset", "Reset", ResetButtonPressed);


        SetupVersionText();
	}


    private void SetupVersionText()
    {
        m_versionText.text = "v" + m_versionNumber;
        if (m_previewMode)
            m_versionText.text += " PERVIEW";
        if (m_eventMode)
            m_versionText.text += " EVENT";
        m_versionText.enabled = m_showVersionText;
    }


	// ********************************************************************
	// Function:	PreviewButtonPressed()
	// ********************************************************************
	private void PreviewButtonPressed(string _id, GameObject _button)
	{
		m_previewMode = !m_previewMode;
        _button.GetComponent<Image>().color = m_previewMode ? Color.green : Color.red;
        SetupVersionText();

        // Perform game-specific actions on preview mode toggle
        if (OnPreviewModeToggle != null)
			OnPreviewModeToggle(m_previewMode);

		ResetButtonPressed(_id, _button);
    }


    // ********************************************************************
    // Function:	EventButtonPressed()
    // ********************************************************************
    private void EventButtonPressed(string _id, GameObject _button)
    {
        m_eventMode = !m_eventMode;
        _button.GetComponent<Image>().color = m_previewMode ? Color.green : Color.red;
        SetupVersionText();

        // Perform game-specific actions on event mode toggle
        if (OnEventModeToggle != null)
            OnEventModeToggle(m_eventMode);

        ResetButtonPressed(_id, _button);
    }


    // ********************************************************************
    // Function:	ResetButtonPressed()
    // ********************************************************************
    private void ResetButtonPressed(string _id, GameObject _button)
	{
		// Reset to title screen
		if (OnResetGame != null)
			OnResetGame();
		LoadingSceneManager.LoadScene(0);
		ToggleVisibility();

		// Remove all non-default buttons
		for (int i = 0; i < instance.m_debugButtons.Count; ++i)
		{
			if (instance.m_debugButtons[i].name != "preview" && instance.m_debugButtons[i].name != "reset" )
			{
				GameObject button = instance.m_debugButtons[i];
				instance.m_debugButtons.RemoveAt(i);
				instance.m_debugButtonCallbacks.RemoveAt(i);
				GameObject.Destroy(button);
			}
		}
	}


    // ********************************************************************
    // Function:	OnEnable()
    // Purpose:		Called when the script is enabled.
    // ********************************************************************
	void OnEnable ()
    {
        if (!m_callbackRegistered)
        {
            m_callbackRegistered = true;
//            Debug.Log("INITIALIZING DEBUG WINDOW!");
            Application.logMessageReceived += HandleLog;
        }
	}


    // ********************************************************************
    // Function:	OnDisable()
    // Purpose:		Called when the script is disabled.
    // ********************************************************************
    void OnDisable()
    {
        m_callbackRegistered = false;
		Application.logMessageReceived -= HandleLog;
//        Debug.Log("DE_INITIALIZING DEBUG WINDOW!");
	}


    // ********************************************************************
    // Function:	Update()
    // Purpose:		Run once per frame.
    // ********************************************************************
    void Update()
    {
		int fingerCount = 0;
        for (int i = 0; i < Input.touches.Length; ++i)
        {
            if (Input.touches[i].phase != TouchPhase.Ended && Input.touches[i].phase != TouchPhase.Canceled)
                fingerCount++;
        }

		if ((!m_debugKey.NullOrEmpty() && Input.GetKeyUp(m_debugKey))
            || (m_openWith4FingerTouch && fingerCount >=4))
        {
            if (Time.realtimeSinceStartup < m_last4FingerTouch + m_touchCooldown) return;

            m_last4FingerTouch = Time.realtimeSinceStartup;

			ToggleVisibility();
        }

		// Debug pause
		if (Input.GetKey(KeyCode.P))
		{
			Debug.Break();
		}
    }


    // ********************************************************************
    // Function:	AddButton()
    // Purpose:		Adds a button to the debug menu.
    // ********************************************************************
    public static GameObject AddButton(string _id, string _name, DebugButtonCallback _callback, GameObject _prototype = null)
    {
		if (instance == null) return null;

        GameObject prototype = _prototype == null ? instance.m_buttonPrototype : _prototype;
		GameObject newButton = GameObject.Instantiate(prototype,instance.m_buttonGrid) as GameObject;
		newButton.transform.localScale = Vector3.one;
        newButton.name = _id;
        newButton.GetComponentInChildren<Text>().text = _name;
        newButton.GetComponent<Button>().onClick.AddListener(delegate { ButtonPressed(_id); });
        instance.m_debugButtons.Add(newButton);
        instance.m_debugButtonCallbacks.Add(_callback);

        return newButton;
    }


    // ********************************************************************
    // Function:	RemoveButton()
    // Purpose:		Removes a button from the debug menu.
    // ********************************************************************
    public static void RemoveButton(string _id)
    {
        if (instance == null) return;

        for (int i = 0; i < instance.m_debugButtons.Count; ++i)
        {
            if (instance.m_debugButtons[i].name == _id)
            {
                GameObject button = instance.m_debugButtons[i];
                instance.m_debugButtons.RemoveAt(i);
                instance.m_debugButtonCallbacks.RemoveAt(i);
                GameObject.Destroy(button);
            }
        }

    }


    // ********************************************************************
    // Function:	ButtonPressed()
    // Purpose:		Called when a generic debug button is pressed.
    // ********************************************************************
    public static void ButtonPressed(string _id)
    {
        for (int i = 0; i < instance.m_debugButtons.Count; ++i)
        {
            if (instance.m_debugButtons[i].name == _id)
            {
                instance.m_debugButtonCallbacks[i](_id, instance.m_debugButtons[i]);
            }
        }

    }


    // ********************************************************************
    // Function:	HandleLog()
    // Purpose:		Records a log from the log callback.
    // ********************************************************************
    void HandleLog(string _message, string _stackTrace, LogType _type)
    {
        m_logs.Add(new Log()
        {
            message = _message,
            stackTrace = _stackTrace,
            type = _type,
        });

		while (m_logs.Count > m_numLogsKept)
		{
			m_logs.RemoveAt(0);
		}

        UpdateTextBox();
    }


    // ********************************************************************
    // Function:	UpdateScrollbar()
    // Purpose:		Jumps the scrollbar to the newest point
    // ********************************************************************
    private IEnumerator UpdateScrollbar()
    {
        if (m_jumpToBottom)
        {
            yield return new WaitForSeconds(0.25f);
            m_scrollbar.value = 0.0f;
        }

        yield return null;
    }


    // ********************************************************************
    // Function:	UpdateTextBox()
    // Purpose:		Updates the contents of the text box
    // ********************************************************************
    private void UpdateTextBox()
    {
        if (m_consoleTextBox == null || m_visibleElements.activeSelf == false) return;

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < m_logs.Count; ++i)
        {
            if (!m_showErrors && m_logs[i].type == LogType.Error) continue;
            if (!m_showWarnings && m_logs[i].type == LogType.Warning) continue;
            if (!m_showLogs && m_logs[i].type == LogType.Log) continue;

            if (m_useRichText)
                builder.Append("<color=" + s_logTypeColors[m_logs[i].type] + ">");

            builder.Append("\n");
            builder.Append(m_logs[i].message);

            if (m_useRichText)
                builder.Append("</color>");
        }

        m_consoleTextBox.text = builder.ToString();

        StartCoroutine(UpdateScrollbar());
    }

    // ********************************************************************
    // Function:	ToggleShowErrors()
    // Purpose:		Changes whether errors are shown or not
    // ********************************************************************
    public void ToggleShowErrors()
    {
        m_showErrorButton.GetComponent<Image>().color = m_showErrors ? new Color(0.75f, 0.25f, 0.25f) : new Color(1.0f, 0.5f, 0.5f) ;
        m_showErrors = !m_showErrors;
        UpdateTextBox();
    }

    // ********************************************************************
    // Function:	ToggleShowWarnings()
    // Purpose:		Changes whether warnings are shown or not
    // ********************************************************************
    public void ToggleShowWarnings()
    {
        m_showWarningsButton.GetComponent<Image>().color = m_showWarnings ? new Color(0.75f, 0.75f, 0.25f) : new Color(1.0f, 1.0f, 0.5f);
        m_showWarnings = !m_showWarnings;
        UpdateTextBox();
    }

    // ********************************************************************
    // Function:	ToggleShowLogs()
    // Purpose:		Changes whether logs are shown or not
    // ********************************************************************
    public void ToggleShowLogs()
    {
        m_showLogsButton.GetComponent<Image>().color = m_showLogs ? new Color(0.75f, 0.75f, 0.75f) : new Color(1.0f, 1.0f, 1.0f);
        m_showLogs = !m_showLogs;
        UpdateTextBox();
    }

    // ********************************************************************
    // Function:	ToggleJumpToBottom()
    // Purpose:		Changes whether the log jumps to the newest
    // ********************************************************************
    public void ToggleJumpToBottom()
    {
        m_jumpToBottomButton.GetComponent<Image>().color = m_jumpToBottom ? new Color(0.75f, 0.75f, 0.75f) : new Color(1.0f, 1.0f, 1.0f);
        m_jumpToBottom = !m_jumpToBottom;
	}

	// ********************************************************************
	// Function:	ToggleVisibility()
	// Purpose:		Changes between visible and invisible debug
	// ********************************************************************
	public void ToggleVisibility()
	{
		m_visibleElements.SetActive(!m_visibleElements.activeSelf);
		UpdateTextBox();

		float oldTimeScale = m_timeScale;
		m_timeScale = Time.timeScale;
		Time.timeScale = oldTimeScale;
		AudioListener.pause = Time.timeScale == 0;
	}

	public static void Show(bool _show)
	{
		instance.m_visibleElements.SetActive(_show);
		instance.UpdateTextBox();

		if ((instance.m_timeScale == 0 && _show) || (instance.m_timeScale != 0 && !_show))
		{
			float oldTimeScale = instance.m_timeScale;
			instance.m_timeScale = Time.timeScale;
			Time.timeScale = oldTimeScale;
		}
		AudioListener.pause = _show;
	}
	public static bool Shown()
	{
		return instance.m_visibleElements.activeSelf;
	}
}

#endif