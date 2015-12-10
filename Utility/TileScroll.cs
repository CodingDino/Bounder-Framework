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
// Class: TileScroll
// ************************************************************************ 
public class TileScroll : MonoBehaviour {
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private Transform[] m_tiles;
	[SerializeField]
	private Vector3 m_loopPoint;
	[SerializeField]
	private Vector3 m_spacing;
	[SerializeField]
	private Vector3 m_scrollSpeed;

	
	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update ()
	{
		for (int i = 0; i < m_tiles.Length; ++i)
		{
			m_tiles[i].position = m_tiles[i].position + m_scrollSpeed * Time.deltaTime;
			if (m_tiles[i].position.x < m_loopPoint.x || m_tiles[i].position.y < m_loopPoint.y)
			{
				m_tiles[i].position = m_tiles[i].position + m_spacing * m_tiles.Length;
			}
		}
	}
}
