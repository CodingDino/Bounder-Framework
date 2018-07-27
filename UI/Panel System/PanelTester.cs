using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTester : MonoBehaviour {

	public Panel m_panel;

	// Use this for initialization
	IEnumerator Start () {
		yield return null;
		PanelManager.OpenPanel(m_panel);
	}
}
