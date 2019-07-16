// ************************************************************************ 
// File Name:   ConditionCollection.cs 
// Purpose:    	A collection of Conditions, evaluated like a single condition
// Project:		Framework
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
#region Class: ConditionCollection
// ************************************************************************
[CreateAssetMenu(fileName = "ConditionCollection", menuName = "Bounder/Conditions/ConditionCollection", order = 0)]
public class ConditionCollection : Condition 
{
	// ********************************************************************
	#region Enum: Operation
	// ********************************************************************
	public enum Operation
	{
		AND,
		OR
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	[SerializeField]
	private Operation m_operation = Operation.AND;
	[SerializeField]
	private Condition[] m_conditions = null;
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

			// Listen to children
			if (_register)
				m_conditions[i].OnConditionTriggered += OnChildConditionTriggered;
			else
				m_conditions[i].OnConditionTriggered -= OnChildConditionTriggered;
		}
	}
	// ********************************************************************
	public override bool Evaluate() 
	{
		bool evaluation = false;

		// Default different based on operation
		switch (m_operation)
		{
			case Operation.AND:
				evaluation = true;
				break;
			case Operation.OR:
				evaluation = false;
				break;
		}

		// Check all sub-conditions, based on operation
		for (int i = 0; i < m_conditions.Length; ++i)
		{
			switch (m_operation)
			{
				case Operation.AND:
					evaluation = evaluation && m_conditions[i].Evaluate();
					break;
				case Operation.OR:
					evaluation = evaluation || m_conditions[i].Evaluate();
					break;
			}
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


	// ********************************************************************
	#region Private Functions
	// ********************************************************************
	private void OnChildConditionTriggered(Condition _condition)
	{
		if (Evaluate() == true)
			Trigger();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
