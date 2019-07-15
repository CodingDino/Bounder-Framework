// ************************************************************************ 
// File Name:   MoveToTarget.cs 
// Purpose:    	Moves the object to a specified target
// Project:		Armoured Engines
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bounder.Framework;


// ************************************************************************ 
// Class: DraggableTrainPart
// ************************************************************************
public class MoveToTarget : MonoBehaviour, ObjectPoolResetResponder {

	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private bool m_useQueue = false;
	[SerializeField]
	private bool m_useSeparateEasing = false;
	[SerializeField]
	[ShowInInspectorIf("m_useSeparateEasing",false)]
	private EasingFunction m_easingFunc;
	[SerializeField]
	[ShowInInspectorIf("m_useSeparateEasing",true)]
	private EasingFunction m_easingFuncX;
	[SerializeField]
	[ShowInInspectorIf("m_useSeparateEasing",true)]
	private EasingFunction m_easingFuncY;
	[SerializeField]
	private float m_duration = 0;
	[SerializeField]
	private float m_speed = 0;

	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Transform m_transform = null;
	private Vector3 m_originPoint = Vector3.zero;
	private Vector3 m_targetPoint = Vector3.zero;
	private Vector3 m_diff = Vector3.zero;
	private Queue<Vector3> m_targetQueue = new Queue<Vector3>();
	private float m_startTime = 0f;
	private bool m_moving = false;
	private float m_currentDuration = 0f;


	// ********************************************************************
	// Properties 
	// ********************************************************************
	public Vector3 targetPoint { get { return m_targetPoint; } }
	public bool hasTarget { get { return m_moving; } }
	public bool isMoving { get { return m_moving; } }
	public float duration { set { m_duration = value; }}
	public float speed { get {return m_speed;} set { m_speed = value; }}
	public EasingFunction easingFunc { set { m_easingFunc = value; }}
	public EasingFunction easingFuncX { set { m_easingFuncX = value; }}
	public EasingFunction easingFuncY { set { m_easingFuncY = value; }}


	// ********************************************************************
	#region Events 
	// ********************************************************************
	public delegate void ArrivedAtTarget(Vector3 _target);
	public event ArrivedAtTarget OnArrivedAtTarget;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region ObjectPoolResetResponder Methods 
	// ********************************************************************
	public void Reset()
	{
		m_originPoint = Vector3.zero;
		m_targetPoint = Vector3.zero;
		m_diff = Vector3.zero;
		m_targetQueue.Clear();
		m_startTime = 0;
		m_moving = false;
		m_currentDuration = 0;
		StopAllCoroutines();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	// Function:	MoveTo()
	// Purpose:		Set new target or add to queue
	// ********************************************************************
	public void MoveTo(Vector3 _target) 
	{ 
		if (m_moving && m_useQueue)
			m_targetQueue.Enqueue(_target);
		else
			SetTarget(_target);
	}
	public void ClearTargets()
	{
		m_targetQueue.Clear();
		m_moving = false;
		m_targetPoint = Vector3.zero;
	}


	// ********************************************************************
	// Function:	SetTarget()
	// Purpose:		Set a new target for the object
	// ********************************************************************
	private void SetTarget(Vector3 _target) 
	{ 
		if (m_transform == null)
			m_transform = transform;
		if (m_moving && _target == m_targetPoint)
			return;
		if (!m_moving && _target == m_transform.position)
			return;
		m_originPoint = m_transform.position;
		m_targetPoint = _target;
		m_diff = m_targetPoint - m_originPoint;
		m_startTime = Time.time;
		m_currentDuration = m_duration;
		if (m_currentDuration == 0 && m_speed != 0)
		{
			m_currentDuration = (m_targetPoint - m_originPoint).magnitude / m_speed;
		}
		if (!m_moving)
			StartCoroutine(Move());
	}


	// ********************************************************************
	// Function:	Move()
	// Purpose:		Handles movement towards an object
	// ********************************************************************
	IEnumerator Move() 
	{
		if (!m_useSeparateEasing)
		{
			m_easingFuncX = m_easingFunc;
			m_easingFuncY = m_easingFunc;
		}
		if (m_transform == null)
			m_transform = transform;
		m_moving = true;
		while (m_moving)
		{
			float currentTime = Time.time - m_startTime;
			if (currentTime >= m_currentDuration)
			{
				m_transform.position = m_targetPoint;
				m_moving = false;
				if (OnArrivedAtTarget != null)
					OnArrivedAtTarget(m_targetPoint);
				if (m_targetQueue.Count > 0)
					SetTarget(m_targetQueue.Dequeue());
			}
			else
			{
				float multX = Easing.GetFunc(m_easingFuncX)(currentTime,
				                                            0,
				                                            1,
				                                            m_currentDuration);
				float multY = Easing.GetFunc(m_easingFuncY)(currentTime,
				                                            0,
				                                            1,
				                                            m_currentDuration);
				Vector3 position = m_transform.position;
				position.x = m_originPoint.x + m_diff.x * multX;
				position.y = m_originPoint.y + m_diff.y * multY;
				m_transform.position = position;
			}

			yield return null;
		}

	}
}
