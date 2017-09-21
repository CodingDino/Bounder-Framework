// ************************************************************************ 
// File Name:   TriggerRandomAnimation.cs 
// Purpose:    	Triggers a random animation from a set at an interval
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
#region Class: TriggerRandomAnimation
// ************************************************************************ 
[RequireComponent(typeof(Animator))]
public class TriggerRandomAnimation : MonoBehaviour 
{
	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	[Tooltip("List of parameters to randomly choose from")]
	private List<AnimatorControllerParameterData> m_parameters = new List<AnimatorControllerParameterData>();
	[SerializeField]
	[Tooltip("Frequency range of triggers")]
	private Vector2 m_triggerFrequencyRange;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void OnEnable()
	{
		StartCoroutine(TriggerOverTime());
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods
	// ********************************************************************
	private IEnumerator TriggerOverTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(m_triggerFrequencyRange.x, m_triggerFrequencyRange.y));
			m_parameters.Random().Apply();
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************

}
