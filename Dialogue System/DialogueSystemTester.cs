using UnityEngine;
using System.Collections;

public class DialogueSystemTester : MonoBehaviour {

	public DialogueConversation m_conversation;
	public DialoguePanel m_dialoguePanel;

	// Use this for initialization
	IEnumerator Start () {
		yield return null;
		PanelManager.OpenPanel(m_dialoguePanel, new DialoguePanelData(m_conversation));
	}
}
