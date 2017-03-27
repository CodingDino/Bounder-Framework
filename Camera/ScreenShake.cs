// ************************************************************************ 
// File Name:   ScreenShake.cs 
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
using BounderFramework;
#endregion
// ************************************************************************

// ************************************************************************ 
#region Class: ScreenShakeEvent
// ************************************************************************
public class ScreenShakeEvent : GameEvent 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public float magnitude = 1.0f;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Constructors 
	// ********************************************************************
	public ScreenShakeEvent (float _magnitude = 1.0f) 
	{
		magnitude = _magnitude;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ScreenShake
// ************************************************************************
[RequireComponent(typeof(MoveToTarget))]
public class ScreenShake : MonoBehaviour 
{
	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Vector3 m_startingPoint;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods 
	// ********************************************************************
	void OnEnable () 
	{
		Events.AddListener<ScreenShakeEvent>(OnScreenShakeEvent);
	}
	// ********************************************************************
	void OnDisable () 
	{
		Events.RemoveListener<ScreenShakeEvent>(OnScreenShakeEvent);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods 
	// ********************************************************************
	private void OnScreenShakeEvent (ScreenShakeEvent _event) 
	{
		MoveToTarget mover = GetComponent<MoveToTarget>();
		int numShakes = Mathf.CeilToInt(2.0f * _event.magnitude);
		float distance = 0.05f * _event.magnitude;
		if (!mover.hasTarget)
			m_startingPoint = transform.position;
		for (int i = 0; i < numShakes; ++i)
		{
			// shake to
			Vector2 direction = Vector2.one.Randomise();
			Vector3 target = m_startingPoint;
			target.x += direction.x * distance;
			target.y += direction.y * distance;

			mover.MoveTo(target);

			// shake back
			mover.MoveTo(m_startingPoint);
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************