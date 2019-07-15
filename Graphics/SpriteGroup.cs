// ************************************************************************ 
// File Name:   SpriteGroup.cs 
// Purpose:    	Specify a group of sprites for various purposes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************
namespace Bounder.Framework { 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: SpriteGroup
// ************************************************************************ 
public class SpriteGroup : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("ID for this")]
	private string m_id = "";
	[SerializeField]
	[Tooltip("List of sprites")]
	private List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public string id { get { return m_id; } }
	public List<SpriteRenderer> sprites { get { return m_sprites; } }
	#endregion
	// ********************************************************************


}
// ************************************************************************
#endregion
// ************************************************************************

}
