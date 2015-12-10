// ************************************************************************ 
// File Name:   TileScroll.cs 
// Purpose:    	Control a scrolling array of tiles
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;


// ************************************************************************ 
// Class: SpriteToColour
// ************************************************************************ 
public class SpriteToColour : MonoBehaviour {
	

	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private Color m_colour;


	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.material.shader = Shader.Find("GUI/Text Shader");
		renderer.color = m_colour;
	}

}
