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


		private ParticleSystem particles;
	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	IEnumerator Start () 
	{
			particles = GetComponent<ParticleSystem>();
			while (particles.IsAlive())
				yield return null;
			Destroy(gameObject);
			Debug.Log("Destroying Particles");
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}
