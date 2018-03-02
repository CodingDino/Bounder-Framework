// ************************************************************************ 
// File Name:   DialoguePanel.cs 
// Purpose:    	Control dialogue displayed in the dialogue panel
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: DialoguePanelData
// ************************************************************************
[System.Serializable]
public class DialoguePanelData : PanelData 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public DialogueConversation conversation;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Constructors 
	// ********************************************************************
	public DialoguePanelData(DialogueConversation _conversation,
	                         PanelState _startingState = PanelState.HIDDEN,
	                         PanelLimitOverride _limitOverride = PanelLimitOverride.REPLACE)
	: base(_startingState, _limitOverride)
	{
		conversation = _conversation;
	}
	// ********************************************************************
	public DialoguePanelData(JSON _JSON)
		: base(_JSON)
	{
//		_JSON["conversationID"].Get(ref conversationID);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************


// ************************************************************************ 
// Class: DialoguePanel
// ************************************************************************ 
public class DialoguePanel : Panel
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
    [SerializeField]
    private GameObject m_dialoguePanel;
    [SerializeField]
    private Text m_textObject;
    [SerializeField]
    private Text m_portraitText;
    [SerializeField]
    private float m_defaultTextSpeed = 30.0f;
    [SerializeField]
    private GameObject m_waitingIcon;
    [SerializeField]
	private Animator[] m_portraitMovers = new Animator[2];
	[SerializeField]
	private Transform[] m_characterRoots = new Transform[2];
    [SerializeField]
    private GameObject m_choiceRoot = null;
    [SerializeField]
    private GameObject m_choiceButtonPrototype = null;
    [SerializeField]
	private float m_choicePopInDelay = 0.05f;
	#endregion
	// ********************************************************************
    

	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
    private DialogueConversation m_currentConversation;
    private DialogueFrame m_currentFrame;
    private int m_sectionIndex;
    private DialogueSection m_currentSection;
	private DialogueTextSettings m_currentTextSettings;
    private int m_displayIndex = 0;
    private bool m_shouldSkip = false;
    private bool m_waitingForNextFrame = false;
    private bool m_waitingForChoiceInput = false;
	private float m_lastAudioPlayed = 0;
	private AudioInfo m_audioInfo = new AudioInfo(AudioCategory.DIALOGUE);
    private int m_frameCount = 0;
	private List<Animator> m_choices = new List<Animator>();
	private Animator[] m_characterAnimator = new Animator[2];
	private string m_previousSections;
	private string m_previousCursor;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Events 
	// ********************************************************************
	public delegate void ConversationSeen(DialogueConversation _conversation);
	public static event ConversationSeen OnConversationSeen;
	public delegate void ChoiceMade(DialogueLink _link);
	public static event ChoiceMade OnChoiceMade;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Panel Methods 
	// ********************************************************************
	protected override PanelData _CreatePanelData(JSON _JSON)
	{
		return new DialoguePanelData(_JSON);
	}
	// ********************************************************************
	protected override void _Initialise(PanelData _data) 
	{
		DialoguePanelData castData = _data as DialoguePanelData;
		if (castData != null)
		{
			LogManager.Log("DialoguePanel _Initialise with conversation "+castData.conversation.name, 
			           LogCategory.UI, 
			           LogSeverity.WARNING, 
			           "Dialogue", 
			           gameObject);
			m_currentConversation = castData.conversation;
		}
		m_previousCursor = InputManager.cursor;
		Events.Raise(new ChangeCursorEvent("TalkingCursor"));
	}
	// ********************************************************************
	protected override void _Hide()
	{
		m_waitingIcon.SetActive(false);
		m_textObject.text = "";
		StartCoroutine(ShowChoices(false));
		for (int i = 0; i < 2; ++i)
		{
			m_portraitMovers[i].SetBool("Shown", false);
		}
		Events.Raise(new ChangeCursorEvent(m_previousCursor));
	}
	// ********************************************************************
	protected override void _ChangeState(PanelState _newState, PanelState _oldState)
	{
		LogManager.Log("DialoguePanel _ChangeState new state = "+_newState, 
		                   LogCategory.UI, 
		                   LogSeverity.WARNING, 
		                   "Dialogue", 
		                   gameObject);
		if (_newState == PanelState.SHOWN)
			StartCurrentConversation();
		if (IsVisible())
			Events.Raise(new ButtonPopperEvent("BackButton", false));
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Monobehaviour Methods 
    // ********************************************************************
    void Update()
    {
		if (state != PanelState.SHOWN)
			return;

		if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (m_waitingForNextFrame)
			{
				bool end = m_currentFrame.endOnThisFrame;
                if (!end)
                {
                    // Choose next frame based on which frames we meet the requirements for
                    for (int i = 0; i < m_currentFrame.links.Count; ++i )
                    {
                        if (m_currentFrame.links[i].MeetsConditions())
                        {
							FollowLink(m_currentFrame.links[i].linkedFrame);
                            return;
                        }
                    }
					// No link - go to next in list
					int newIndex = m_currentConversation.frames.IndexOf(m_currentFrame) + 1;
					end = newIndex == m_currentConversation.frames.Count;
					if (!end)
					{
						FollowLink(m_currentConversation.frames[newIndex]);
						return;
					}
				}

				if (end)
				{
					if (ProfileManager.profile != null && !ProfileManager.profile.conversationsSeen.Contains(m_currentConversation.name))
					{
						ProfileManager.profile.conversationsSeen.Add(m_currentConversation.name);
					}
					if (OnConversationSeen != null)
						OnConversationSeen(m_currentConversation);

					Close();
					return;
				}
            }
            else
            {
                m_shouldSkip = true;
            }
        }
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public bool StartCurrentConversation()
	{
		if (m_currentConversation == null)
			return false;
		
		LogManager.Log("DialoguePanel: Starting conversation "+m_currentConversation.name, 
		               LogCategory.UI, 
		               LogSeverity.WARNING, 
		               "Dialogue", 
		               gameObject);

		m_currentFrame = m_currentConversation.frames[0];

		StartCoroutine(DisplayFrame());

		return true;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
    // ********************************************************************
	private IEnumerator DisplayFrame()
	{
		LogManager.Log("DialoguePanel: Showing Frame "+m_currentFrame, 
		               LogCategory.UI, 
		               LogSeverity.WARNING, 
		               "Dialogue", 
		               gameObject);

		// Initialize stuff for new frame
		m_shouldSkip = false;
		m_previousSections = "";

		if (m_waitingForNextFrame)
		{
			m_waitingForNextFrame = false;
			m_waitingIcon.SetActive(false);
		}

		if (m_waitingForChoiceInput)
		{
			m_waitingForChoiceInput = false;
			StartCoroutine(ShowChoices(false));
		}

        m_sectionIndex = 0;
        m_textObject.text = "";
        ++m_frameCount;

        // Load correct portrait
        int side = -1;
		if (m_currentFrame.character.portrait != null)
		{
			side = (int) m_currentFrame.portraitSide;

			yield return StartCoroutine(ChangePortraits(side, m_currentFrame.character.portrait));
		}
		m_portraitText.text = m_currentFrame.character.displayName;

		for (int i = 0; i < 2; ++i)
		{
			if (i != side)
			{
				m_portraitMovers[i].SetBool("Shown", false);
			}
		}
        StartCoroutine(DisplaySection());
	}
	// ********************************************************************
	private IEnumerator ChangePortraits(int _side, Animator _portrait)
	{
		LogManager.Log("DialoguePanel: ChangePortraits to "+_portrait.name, 
		               LogCategory.UI, 
		               LogSeverity.WARNING, 
		               "Dialogue", 
		               gameObject);
		
		Animator currentMover = m_portraitMovers[_side];

		if (m_characterAnimator[_side] == null || m_characterAnimator[_side].name != _portrait.name)
		{
			currentMover.SetBool("Shown", false);
			while (!currentMover.GetCurrentAnimatorStateInfo(0).IsName("Hidden"))
				yield return null;

			if (m_characterAnimator[_side] != null)
			{
				GameObject.Destroy(m_characterAnimator[_side].gameObject);
				m_characterAnimator[_side] = null;
			}

			// TODO: Instantiating these each time? Couldn't we create and disable/recycle them?
			GameObject characterObject = GameObject.Instantiate(_portrait.gameObject);
			characterObject.name = _portrait.name;
			Vector3 localPos = characterObject.transform.localPosition;
			characterObject.transform.parent = m_characterRoots[_side].transform;
			characterObject.transform.localPosition = localPos;
			characterObject.transform.localScale = new Vector3(Mathf.Abs(characterObject.transform.localScale.x), characterObject.transform.localScale.y, characterObject.transform.localScale.z);
			m_characterAnimator[_side] = characterObject.GetComponent<Animator>();
		}

		m_portraitMovers[_side].SetBool("Shown", true);
	}
    // ********************************************************************
    private IEnumerator DisplaySection()
	{
        // Initialize stuff for new section
        m_currentSection = m_currentFrame.sections[m_sectionIndex];
		m_displayIndex = 0;

		LogManager.Log("DialoguePanel: DisplaySection "+m_currentSection.text, 
		               LogCategory.UI, 
		               LogSeverity.WARNING, 
		               "Dialogue", 
		               gameObject);

		m_currentTextSettings = new DialogueTextSettings();
		m_currentTextSettings.Merge(m_currentFrame.character.textSettings);
		m_currentTextSettings.Merge(m_currentSection.textSettings);


		Dictionary<char,float> textSymbolTime = new Dictionary<char, float>();
		// Get settings from character
		for (int i = 0; i < m_currentTextSettings.textSymbolTime.Count; ++i )
		{
			textSymbolTime[m_currentTextSettings.textSymbolTime[i].symbol] = m_currentTextSettings.textSymbolTime[i].time;
		}

        // Print text until we're done
        if (m_currentSection.text != null)
        {
			float textSpeed = m_defaultTextSpeed * m_currentTextSettings.textSpeed;
			float secondsToWait = 1.0f / textSpeed;

			int side = -1;
			string currentSpecial = "";

			if (m_currentFrame.character.portrait != null)
			{
				side = (int) m_currentFrame.portraitSide;
				// Set portrait emotion
				m_characterAnimator[side].SetInteger("Emotion", (int)m_currentSection.emotion);
				// Set special animation triggers
				if (m_currentSection.triggerAnimation > 0)
				{
					currentSpecial = "Special-"+m_currentSection.triggerAnimation;
					m_characterAnimator[side].SetTrigger("Special-"+m_currentSection.triggerAnimation);
				}
				else
					// Set portrait to talking animation ONLY if no special animation is playing.
					m_characterAnimator[side].SetBool("Talk", true);
			}

            while (m_displayIndex < m_currentSection.text.Length)
			{
				char currentChar = m_currentSection.text[m_displayIndex];
				float currentSecondsToWait = secondsToWait;
				if (textSymbolTime.ContainsKey(currentChar))
					currentSecondsToWait = textSymbolTime[currentChar];

				// if we were playing a special animation, see if it is time to start the talk animation
				if (!currentSpecial.NullOrEmpty())
				{
					if(!m_characterAnimator[side].GetCurrentAnimatorStateInfo(1).IsName(currentSpecial))
					{
						currentSpecial = "";
						m_characterAnimator[side].SetBool("Talk", true);
					}
				}

				PrintText();
				if (!m_shouldSkip)
					yield return new WaitForSeconds(currentSecondsToWait);
			}

			// Set portrait to idle animation
			if (m_currentFrame.character.portrait != null)
			{
				side = (int) m_currentFrame.portraitSide;
				m_characterAnimator[side].SetBool("Talk", false);
			}
		}

        // TODO: Wait for animation to finish if we triggered a special animation.

        // TODO: Some kind of manual "wait" system? (for cutscenes)

		// Load next section
		m_previousSections += m_currentSection.text;
        ++m_sectionIndex;
        if (m_sectionIndex < m_currentFrame.sections.Count)
        {
            StartCoroutine(DisplaySection());
        }
        else
        {
            m_shouldSkip = false;

            if (m_currentFrame.displayChoices)
            {
                m_waitingForChoiceInput = true;

                List<int> validLinks = new List<int>();
                for (int i = 0; i < m_currentFrame.links.Count; ++i)
                {
                    if (m_currentFrame.links[i].MeetsConditions())
                    {
                        validLinks.Add(i);
                    }
				}
				LogManager.Log("Choices found for frame " + m_currentFrame.id + ": " + validLinks.Count, 
				                   LogCategory.UI, 
				                   LogSeverity.WARNING, 
				                   "Dialogue", 
				                   gameObject);
                for (int i = 0; i < validLinks.Count; ++i)
                {
                    int index = validLinks[i];
					DialogueLink link = m_currentFrame.links[index];
					LogManager.Log("Creating button for "+index+" link frame: " + link.linkedFrame.name, 
					           LogCategory.UI, 
					           LogSeverity.WARNING, 
					           "Dialogue", 
					           gameObject);
                    GameObject choiceButton = GameObject.Instantiate(m_choiceButtonPrototype) as GameObject;
                    choiceButton.transform.SetParent(m_choiceRoot.transform);
                    choiceButton.GetComponentInChildren<Text>().text = link.text;
					AddListenerForChoice(choiceButton.transform.GetComponentInChildren<Button>(), link);
                    m_choices.Add(choiceButton.GetComponent<Animator>());
                }

				StartCoroutine(ShowChoices(true));
            }
            else
			{
				LogManager.Log("DialoguePanel: Setting m_waitingForNextFrame = true for "+m_currentFrame, 
				       LogCategory.UI, 
				       LogSeverity.WARNING, 
				       "Dialogue", 
				       gameObject);
                m_waitingForNextFrame = true;
                m_waitingIcon.SetActive(true);
            }
        }

        yield return null;
         }
    // ********************************************************************
	private void AddListenerForChoice(Button _button, DialogueLink _link)
    {
		_button.onClick.AddListener(() => MakeChoice(_link));
	}  
	// ********************************************************************
	private void MakeChoice(DialogueLink _link)
	{
		if (_link.saveChoice)
		{
			PlayerProfile profile = ProfileManager.GetProfile<PlayerProfile>();
			if (!profile.choicesMade.Contains(_link.linkedFrame.name))
				profile.choicesMade.Add(_link.linkedFrame.name);
		}
		LogManager.Log("DialoguePanel: MakeChoice "+_link.linkedFrame.name, 
		                       LogCategory.UI, 
		                       LogSeverity.WARNING, 
		                       "Dialogue", 
		                       gameObject);
		if (OnChoiceMade != null)
			OnChoiceMade(_link);
		FollowLink(_link.linkedFrame);
	}
	// ********************************************************************
    private void FollowLink(DialogueFrame _linkedFrame)
	{
		LogManager.Log("DialoguePanel: FollowLink "+_linkedFrame.name, 
		               LogCategory.UI, 
		               LogSeverity.WARNING, 
		               "Dialogue", 
		               gameObject);

		if (_linkedFrame.conversation != m_currentConversation)
			m_currentConversation = _linkedFrame.conversation;
		
		m_currentFrame = _linkedFrame;

		StartCoroutine(DisplayFrame());
	}
	// ********************************************************************
    private IEnumerator ShowChoices(bool _show)
    {
        for (int i = 0; i < m_choices.Count; ++i)
        {
			m_choices[i].SetBool("Show", _show);
            yield return new WaitForSeconds(m_choicePopInDelay);
        }

		if (!_show)
        {
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < m_choices.Count; ++i)
            {
                GameObject.Destroy(m_choices[i].gameObject);
            }
            m_choices.Clear();
        }
        
        yield return null;
    }
    // ********************************************************************
    private void PrintText()
    {
		char currentChar = m_currentSection.text[m_displayIndex];

		m_textObject.text = m_previousSections + 
			  m_currentSection.text.Substring(0,m_displayIndex+1) 
			+ "<color=#0000>" 
			+ m_currentSection.text.Substring(m_displayIndex+1)
			+ "</color>";

        ++m_displayIndex;

        // Play a sound
		float minAudioDuration = 0.0f;
		if (m_currentTextSettings.textAudioMaxSpeed > 0.0f)
			minAudioDuration = 1.0f / (m_currentTextSettings.textAudioMaxSpeed*m_defaultTextSpeed);
		if (currentChar != '\n' && currentChar != ' ' && !m_shouldSkip && (m_lastAudioPlayed + minAudioDuration <= Time.time))
        {
            float randomPitchVariation = Mathf.PerlinNoise(((float)m_displayIndex) * 0.1f, (float)m_frameCount);
			m_audioInfo.pitch = m_currentTextSettings.textPitch + randomPitchVariation * m_currentTextSettings.textPitchVariation;
			AudioManager.Play(m_currentTextSettings.textAudio,m_audioInfo);
			m_lastAudioPlayed = Time.time;
        }
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
