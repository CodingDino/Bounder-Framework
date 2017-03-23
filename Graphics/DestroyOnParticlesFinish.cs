// ************************************************************************ 
// File Name:   DestroyOnParticlesFinish.cs 
// Purpose:    	
// Project:		
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************
namespace BounderFramework 
{ 

// ************************************************************************ 
#region Class: DestroyOnParticlesFinish
// ************************************************************************
[RequireComponent(typeof(ParticleSystem))]
public class DestroyOnParticlesFinish : MonoBehaviour 
{

	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
		Destroy(gameObject);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}
