// ************************************************************************ 
// File Name:   Entity.cs 
// Purpose:    	Facilitates movement of objects.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 


// ************************************************************************ 
// Class: Entity
// ************************************************************************ 
public class Entity : MonoBehaviour {
	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private float m_startingMoveSpeed;
	[SerializeField]
	private float m_startingTurnSpeed;
	[SerializeField]
	private bool m_rotateToMatchFacing;
	[SerializeField]
	private bool m_useRigidBodyVelocity;


    // ********************************************************************
    // Private Data Members 
	// ********************************************************************
	private float m_moveSpeed;
	private float m_turnSpeed;
	private Vector2 m_positionLastFrame;
	private float m_facing = 0.0f; // in degrees
	private bool m_movedLastFrame = false;
	private bool m_isMoving = false;

	// Move Target
	private bool m_moveTargetPossessed = false;
	private Vector2 m_moveTargetPosition;
	private bool m_moveTargetNotifyArrival = false;

	// Turn Target
	private bool m_turnTargetPossessed = false;
	private float m_turnTargetAngle = 0.0f; // in degrees
	private bool m_turnTargetNotifyArrival = false;
	
	
    // ********************************************************************
    // Properties 
	// ********************************************************************
	public float moveSpeed {
		get { return m_moveSpeed; }
		set { m_moveSpeed = value; }
	}
	public float turnSpeed {
		get { return m_turnSpeed; }
		set { m_turnSpeed = value; }
	}
	public float facing {
		get { return m_facing; }
	}

	// Is Moving
	public bool isMoving {
		get {
			if (m_isMoving)
			{
				return true;
			}
			else
				return false;
		}
	}

	// Move to target
	public bool moveTargetArrived {
		get { 
			if (m_moveTargetNotifyArrival)
			{
				m_moveTargetNotifyArrival = false;
				return true;
			}
			else
				return false;
		}
	}
	public bool hasTarget {
		get {
			return m_moveTargetPossessed;
		}
	}
	
	// Turn to target
	public bool turnTargetArrived {
		get { 
			if (m_turnTargetNotifyArrival)
			{
				m_turnTargetNotifyArrival = false;
				return true;
			}
			else
				return false;
		}
	}
	
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Start () {
		m_moveSpeed = m_startingMoveSpeed;
		m_turnSpeed = m_startingTurnSpeed;
		m_positionLastFrame = transform.position;
		m_facing = transform.rotation.eulerAngles.z;
	}
	
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {

		// Process continual movement
		if (m_moveTargetPossessed) ProcessMoveToTarget();
		if (m_turnTargetPossessed) ProcessTurnToTarget();
		
		// Record position for next frame
		m_positionLastFrame = transform.position;
	}


	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, after Update calls are finished.
	// ********************************************************************
	void LateUpdate()
	{
		if (m_movedLastFrame)
			m_isMoving = true;
		else
			m_isMoving = false;

		m_movedLastFrame = false;
	}


	// ********************************************************************
	// Function:	Move()
	// Purpose:		Moves the entity in the assigned direction for this
	//				frame.
	// ********************************************************************
	public void Move (Vector2 _direction)
	{ Move (_direction, m_moveSpeed); }
	public void Move (Vector2 _direction, float _speed) 
	{
		// Normalize the direction vector
		Vector2 normDirection = _direction.normalized;
		
		// Determine velocity in this direction
		Vector2 velocity = normDirection*_speed;

		if (m_useRigidBodyVelocity && GetComponent<Rigidbody2D>() != null && !GetComponent<Rigidbody2D>().isKinematic)
		{
			// Update the rigidbody's velocity
			GetComponent<Rigidbody2D>().velocity = velocity;
		}
		else
		{
			// Update position manually
			transform.position = new Vector3 (
				transform.position.x + velocity.x*Time.deltaTime,
				transform.position.y + velocity.y*Time.deltaTime,
				transform.position.z );
		}

		m_movedLastFrame = true;
	}


	// ********************************************************************
	// Function:	MoveX()
	// Purpose:		Moves the entity along the x axis, leaving y unchanged.
	// ********************************************************************
	public void MoveX (float _direction)
	{ MoveX (_direction, m_moveSpeed); }
	public void MoveX (float _direction, float _speed) 
	{
		// Normalize the direction
		float sign;
		if (_direction != 0)
			sign = _direction / Mathf.Abs (_direction);
		else
			sign = 0;
		
		// Determine velocity in this direction
		float velocity = sign*_speed;
		
		if (m_useRigidBodyVelocity && GetComponent<Rigidbody2D>() != null && !GetComponent<Rigidbody2D>().isKinematic)
		{
			// Update the rigidbody's velocity
			GetComponent<Rigidbody2D>().velocity = new Vector2(
				velocity,
				GetComponent<Rigidbody2D>().velocity.y );
		}
		else
		{
			// Update position manually
			transform.position = new Vector3 (
				transform.position.x + velocity * Time.deltaTime,
				transform.position.y,
				transform.position.z );
		}
		
		m_movedLastFrame = true;
	}
	
	
	// ********************************************************************
	// Function:	MoveY()
	// Purpose:		Moves the entity along the y axis, leaving x unchanged.
	// ********************************************************************
	public void MoveY (float _direction)
	{ MoveY (_direction, m_moveSpeed); }
	public void MoveY (float _direction, float _speed) 
	{
		// Normalize the direction
		float sign;
		if (_direction != 0)
			sign = _direction / Mathf.Abs (_direction);
		else
			sign = 0;
		
		// Determine velocity in this direction
		float velocity = sign*_speed;
		
		if (m_useRigidBodyVelocity && GetComponent<Rigidbody2D>() != null && !GetComponent<Rigidbody2D>().isKinematic)
		{
			// Update the rigidbody's velocity
			GetComponent<Rigidbody2D>().velocity = new Vector2(
				GetComponent<Rigidbody2D>().velocity.x,
				velocity );
		}
		else
		{
			// Update position manually
			transform.position = new Vector3 (
				transform.position.x,
				transform.position.y + velocity * Time.deltaTime,
				transform.position.z );
		}
		
		m_movedLastFrame = true;
	}



	// ********************************************************************
	// Function:	MoveToTarget()
	// Purpose:		Sets up the entity to automatically move to the
	//				provided target position.
	// ********************************************************************
	public void MoveToTarget (Vector2 _targetPosition)
	{ MoveToTarget(_targetPosition, m_moveSpeed); }
	public void MoveToTarget (
		Vector2 _targetPosition, 
		float _speed ) 
	{
		m_moveTargetPossessed = true;
		m_moveTargetNotifyArrival = false;
		m_moveTargetPosition = _targetPosition;
		m_moveSpeed = _speed;
	}
	
	
	// ********************************************************************
	// Function:	ProcessMoveToTarget()
	// Purpose:		Processes the movement to the target for this frame
	// ********************************************************************
	public void ProcessMoveToTarget () 
	{
		// Get current position and distance
		float distanceToTarget = 
			(m_moveTargetPosition-m_positionLastFrame).magnitude;

		// Move towards the target
		Move (m_moveTargetPosition-m_positionLastFrame);

		// If there was overshoot, we have arrived
		Vector2 positionThisFrame = transform.position;
		float distanceTraveled = 
			(positionThisFrame-m_positionLastFrame).magnitude;
		if (distanceTraveled > distanceToTarget)
		{
			m_moveTargetPossessed = false;
			m_moveTargetNotifyArrival = true;
			transform.position = new Vector3(
				m_moveTargetPosition.x, 
				m_moveTargetPosition.y, 
				transform.position.z );
		}

		m_movedLastFrame = true;
	}


	// ********************************************************************
	// Function:	TurnToFaceDirection()
	// Purpose:		Turns to face a specific direction over time.
	// ********************************************************************
	public void TurnToFaceDirection ( Vector2 _targetDirection)
	{ TurnToFaceDirection(_targetDirection,m_turnSpeed); }
	public void TurnToFaceDirection (
		Vector2 _targetDirection, 
		float _turnSpeed ) 
	{
		float angle = Mathf.Atan2(_targetDirection.normalized.y, 
		                          _targetDirection.normalized.x )
			*(180.0f/Mathf.PI);
		TurnToFaceDirection(angle, _turnSpeed);
	}
	public void TurnToFaceDirection ( float _angle)
	{ TurnToFaceDirection(_angle,m_turnSpeed); }
	public void TurnToFaceDirection (
		float _angle, 
		float _turnSpeed ) 
	{

		m_turnTargetPossessed = true;
		m_turnTargetNotifyArrival = false;
		m_turnTargetAngle = _angle;
		m_turnSpeed = _turnSpeed;

		// Account for wrap around
		if (m_turnTargetAngle < 0 ) m_turnTargetAngle += 360.0f;
		if (m_turnTargetAngle > 360.0f ) m_turnTargetAngle -= 360.0f;
	}


	// ********************************************************************
	// Function:	TurnToFaceDirectionInstant()
	// Purpose:		Turns to face a specific direction instantly.
	// ********************************************************************
	public void TurnToFaceDirectionInstant (Vector2 _targetDirection) 
	{
		float angle = Mathf.Atan2(_targetDirection.normalized.y, 
		                          _targetDirection.normalized.x )
			*(180.0f/Mathf.PI);
		TurnToFaceDirectionInstant(angle);
	}
	public void TurnToFaceDirectionInstant (float _angle) 
	{
		m_facing = _angle;
		
		// Account for wrap around
		if (m_facing < 0 ) m_facing += 360.0f;
		if (m_facing > 360.0f ) m_facing -= 360.0f;

		// Apply to object
		if (m_rotateToMatchFacing)
			transform.rotation = Quaternion.Euler(0,0,m_facing);
	}
	
	
	// ********************************************************************
	// Function:	ProcessTurnToTarget()
	// Purpose:		Processes the turn to the target for this frame
	// ********************************************************************
	public void ProcessTurnToTarget () 
	{
		// Define local variables
		float 	angleDistanceLeft, 	angleDistanceRight, 
				angleDistance, 		angleDirection;

		// Get current facing
		m_facing = transform.rotation.eulerAngles.z;

		// Determine direction and distance to target
		angleDistanceLeft = m_turnTargetAngle - m_facing;
		if (angleDistanceLeft < 0 ) angleDistanceLeft += 360.0f;
		angleDistanceRight = m_facing - m_turnTargetAngle;
		if (angleDistanceRight < 0 ) angleDistanceRight += 360.0f;
		if (angleDistanceLeft < angleDistanceRight)
		{
			angleDistance = angleDistanceLeft;
			angleDirection = 1.0f; // Turn counter-clockwise
		}
		else
		{
			angleDistance = angleDistanceRight;
			angleDirection = -1.0f; // Turn clockwise
		}

		// If there was overshoot, we have arrived
		if ( m_turnSpeed * Time.deltaTime >= angleDistance )
		{
			m_facing = m_turnTargetAngle;
			m_turnTargetPossessed = false;
			m_turnTargetNotifyArrival = true;
		}

		// Otherwise, increment.
		else
			m_facing += angleDirection * m_turnSpeed * Time.deltaTime;

		// Adjust for wrap around
		if (m_facing < 0 ) m_facing += 360.0f;
		if (m_facing > 360.0f ) m_facing -= 360.0f;

		// Apply the facing to the object
		if (m_rotateToMatchFacing)
			transform.rotation = Quaternion.Euler(0,0,m_facing);

	}

}