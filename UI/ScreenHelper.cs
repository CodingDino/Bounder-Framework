// ************************************************************************ 
// File Name:   ScreenHelper.cs 
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Enum: Anchor
// ************************************************************************
public enum Anchor
{
	TOP_LEFT, 		TOP, 		TOP_RIGHT,
	LEFT,			CENTER,		RIGHT,
	BOTTOM_LEFT,	BOTTOM,		BOTTOM_RIGHT
}


// ************************************************************************
// Class: ScreenHelper
// ************************************************************************
public class ScreenHelper
{
	
	// ************************************************************************
	public static Vector2 GetScreenPointFromAnchor(Anchor anchor)
	{
		switch (anchor)
		{

		case Anchor.TOP_LEFT :
			return new Vector2 ( 0.0f,				Screen.height );
		case Anchor.TOP :
			return new Vector2 ( Screen.width / 2,	Screen.height );
		case Anchor.TOP_RIGHT :
			return new Vector2 ( Screen.width,		Screen.height );

		case Anchor.LEFT :
			return new Vector2 ( 0.0f,				Screen.height / 2 );
		case Anchor.CENTER :
			return new Vector2 ( Screen.width / 2,	Screen.height / 2 );
		case Anchor.RIGHT :
			return new Vector2 ( Screen.width,		Screen.height / 2 );

		case Anchor.BOTTOM_LEFT :
			return new Vector2 ( 0.0f,				0 );
		case Anchor.BOTTOM :
			return new Vector2 ( Screen.width / 2,	0 );
		case Anchor.BOTTOM_RIGHT :
			return new Vector2 ( Screen.width,		0 );

		default :

			return Vector2.zero;
		}
	}
	// ************************************************************************
}