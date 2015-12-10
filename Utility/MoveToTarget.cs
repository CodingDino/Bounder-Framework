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


// ************************************************************************ 
// Class: DraggableTrainPart
// ************************************************************************
public class MoveToTarget : MonoBehaviour {

	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private bool m_useQueue;
	[SerializeField]
	private EasingFunction m_easingFunc;
	[SerializeField]
	private float m_duration;

	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Transform m_transform;
	private Vector3 m_originPoint;
	private Vector3 m_targetPoint;
	private Vector3 m_diff;
	private Queue<Vector3> m_targetQueue = new Queue<Vector3>();
	private float m_startTime;
	private bool m_moving = false;


	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () 
	{
		m_transform = transform;
	}


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


	// ********************************************************************
	// Function:	SetTarget()
	// Purpose:		Set a new target for the object
	// ********************************************************************
	private void SetTarget(Vector3 _target) 
	{ 
		m_originPoint = m_transform.position;
		m_targetPoint = _target;
		m_diff = m_targetPoint - m_originPoint;
		m_startTime = Time.time;
		if (!m_moving)
			StartCoroutine(Move());
	}


	// ********************************************************************
	// Function:	Move()
	// Purpose:		Handles movement towards an object
	// ********************************************************************
	IEnumerator Move() {
		m_moving = true;
		while (m_moving)
		{

			float currentTime = Time.time - m_startTime;
			if (currentTime >= m_duration)
			{
				m_transform.position = m_targetPoint;
				if (m_targetQueue.Count > 0)
					SetTarget(m_targetQueue.Dequeue());
				else
					m_moving = false;
			}
			else
			{
				float mult = Easing.GetFunc(m_easingFunc)(currentTime,
				                                          0,
				                                          1,
				                                          m_duration);
				m_transform.position = m_originPoint + m_diff * mult;
			}

			yield return 1;
		}

	}
}
