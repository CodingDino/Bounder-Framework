// ************************************************************************ 
// File Name:   ConditionCollection.cs 
// Purpose:    	A collection of Conditions, evaluated like a single condition
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 
// TODO: Allow different arrangements such as && and ||

// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ConditionCollection
// ************************************************************************
[CreateAssetMenu(fileName = "ConditionCollection", menuName = "Bounder/Conditions/ConditionCollection", order = 0)]
public class ConditionCollection : Condition 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[SerializeField]
	private Condition[] m_conditions;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public override int quantity { 
		get {
			int quantity = 0;
			for (int i = 0; i < m_conditions.Length; ++i)
			{
				quantity += m_conditions[i].quantity;
			}
			return quantity;
		} 
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Condition Functions 
	// ********************************************************************
	public override void Register(bool _register) 
	{
		for (int i = 0; i < m_conditions.Length; ++i)
		{
			m_conditions[i].Register(_register);
		}
	}
	// ********************************************************************
	public override bool Evaluate() 
	{
		bool evaluation = true;
		for (int i = 0; i < m_conditions.Length; ++i)
		{
			evaluation = evaluation && m_conditions[i].Evaluate();
		}
		return evaluation; 
	}
	// ********************************************************************
	public override int GetProgress() 
	{
		int progress = 0;
		for (int i = 0; i < m_conditions.Length; ++i)
		{
			progress += m_conditions[i].GetProgress();
		}
		return progress;
	}
	// ********************************************************************
	public override void AddProgress(int _toAdd = 1) 
	{
		for (int i = 0; i < m_conditions.Length; ++i)
		{
			m_conditions[i].AddProgress(_toAdd);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
