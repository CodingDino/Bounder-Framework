// ************************************************************************ 
// File Name:   ParticleGroup.cs 
// Purpose:    	Specify a group of particles for various purposes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
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
#region Class: ParticleGroup
// ************************************************************************ 
public class ParticleGroup : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("ID for this")]
	private string m_id = "";
	[SerializeField]
	[Tooltip("List of particles")]
	private List<ParticleSystem> m_particles = new List<ParticleSystem>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public string id { get { return m_id; } }
	public List<ParticleSystem> particles { get { return m_particles; } }
	#endregion
	// ********************************************************************


}
// ************************************************************************
#endregion
// ************************************************************************

}
