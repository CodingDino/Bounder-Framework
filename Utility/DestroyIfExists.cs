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

	void Awake () {
		if (s_ExistingIDs.Contains(m_ID))
		{
//			Debug.Log("DestroyIfExists: Destroying "+gameObject.name + " " + gameObject.GetInstanceID() +" - ID " + m_ID + " already exists.");
			DestroyImmediate(gameObject);
		}
		else
		{
//			Debug.Log("DestroyIfExists: Adding ID " + m_ID + " for GameObject "+ gameObject.name + " " + gameObject.GetInstanceID() );
			s_ExistingIDs.Add(m_ID);
		}
	}
}
