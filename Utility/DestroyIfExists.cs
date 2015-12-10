// ************************************************************************ 
// File Name:   DestroyIfExists.cs 
// Purpose:    	
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyIfExists : MonoBehaviour {

	[SerializeField]
	private string m_ID;

	private static List<string> s_ExistingIDs = new List<string>();

	void Start () {
		if (s_ExistingIDs.Contains(m_ID))
		{
			DestroyImmediate(gameObject);
		}
		else
		{
			s_ExistingIDs.Add(m_ID);
		}
	}
}
