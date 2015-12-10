// ************************************************************************ 
// File Name:   DropShadow_Text.cs 
// Purpose:    	Creates and maintains a drop shadow for a text mesh
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 
[ExecuteInEditMode]


// ************************************************************************ 
// Class: DropShadow_Text
// ************************************************************************ 
public class DropShadow_Text : MonoBehaviour {


	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private bool m_shouldReInitialize = false;
	[SerializeField]
	private TextMesh m_parentTextMesh = null;
	[SerializeField]
	private TextMesh m_dropShadow = null;
	[SerializeField]
	private Color m_color = Color.black;
	[SerializeField]
	private Vector2 m_offset = new Vector2(2,2);

	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	[SerializeField]
	private bool m_isInitialized = false;

	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
		if (m_shouldReInitialize)
			m_isInitialized = false;

		if (!m_isInitialized)
			Initialize();
	}
	
	
	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update () {
		if (m_shouldReInitialize)
			m_isInitialized = false;

		if (!m_isInitialized)
			Initialize();

		if (m_isInitialized)
		{
			m_dropShadow.text = m_parentTextMesh.text;

			m_dropShadow.GetComponent<Renderer>().sortingLayerName = m_parentTextMesh.GetComponent<Renderer>().sortingLayerName;
			m_dropShadow.GetComponent<Renderer>().sortingOrder = m_parentTextMesh.GetComponent<Renderer>().sortingOrder - 1;
		}
	}


	// ********************************************************************
	// Function:	Initialize()
	// Purpose:		Sets up the drop shadow object
	// ********************************************************************
	void Initialize () {
		if (m_parentTextMesh == null)
			m_parentTextMesh = GetComponent<TextMesh>();

		if (m_parentTextMesh == null)
			return;

		// Create a drop shadow based on parent
		if (m_dropShadow == null)
			m_dropShadow = (GameObject.Instantiate(m_parentTextMesh.gameObject) as GameObject).GetComponent<TextMesh>();
		
		if (m_dropShadow == null)
			return;

		DropShadow_Text extraDropShadow = m_dropShadow.GetComponent<DropShadow_Text>();
		if (extraDropShadow != null)
			DestroyImmediate (extraDropShadow);
		LayerSelecter extraLayerSelecter = m_dropShadow.GetComponent<LayerSelecter>();
		if (extraLayerSelecter != null)
			DestroyImmediate (extraLayerSelecter);

		m_dropShadow.transform.parent = m_parentTextMesh.transform;
		m_dropShadow.transform.localPosition = new Vector3( m_offset.x, m_offset.y, 0);
		m_dropShadow.transform.localScale = new Vector3( 1, 1, 1);
		m_dropShadow.GetComponent<Renderer>().sortingLayerName = m_parentTextMesh.GetComponent<Renderer>().sortingLayerName;
		m_dropShadow.GetComponent<Renderer>().sortingOrder = m_parentTextMesh.GetComponent<Renderer>().sortingOrder - 1;
		m_dropShadow.color = m_color;
		m_dropShadow.name = m_parentTextMesh.name + "-DropShadow";

		m_isInitialized = true;
		m_shouldReInitialize = false;
	}
}
