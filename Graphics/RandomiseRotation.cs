using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
// ************************************************************************ 
// File Name:   RandomiseRotation.cs 
// Purpose:    	Randomise rotation between two angles
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************
namespace BounderFramework { 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BounderFramework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: RandomiseRotation
// ************************************************************************ 
public class RandomiseRotation : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private float m_lowerAngleBound = 0f;
	[SerializeField]
	private float m_upperAngleBound = 0f;
	[SerializeField]
	[Tooltip("Should the random rotation be applied on start")]
	private bool m_applyOnStart = true;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Monobehavior Methods 
	// ********************************************************************
	void Start () 
	{
		if (m_applyOnStart)
			ApplyRandomRotation();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ApplyRandomRotation()
	{
		float angle = Random.Range(m_lowerAngleBound,m_upperAngleBound);
		transform.Rotate(0,0,angle);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************

}
