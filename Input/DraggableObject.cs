// ************************************************************************ 
// File Name:   DraggableObject.cs 
// Purpose:    	Object can be click+dragged or touch+dragged around the 
//				screen
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


// ************************************************************************ 
// Class: DraggableObject
// ************************************************************************ 
public class DraggableObject : MonoBehaviour
{

	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************
	[SerializeField]
	private bool m_allowDragging = true;
	[SerializeField]
	private bool m_clampToScreenByRenderer = false;
	[SerializeField]
	private bool m_clampToScreenByLimits = false;
	[SerializeField]
	private Renderer m_renderer = null;
	[SerializeField]
	private Transform m_transform = null;
	[SerializeField]
	private bool m_restrictX = false;
	[SerializeField]
	private bool m_restrictY = false;
	[SerializeField]
	private bool m_restrictDragOffX = false;
	[SerializeField]
	private bool m_restrictDragOffY = false;
	[SerializeField]
	private Transform m_minLimit = null;
	[SerializeField]
	private Transform m_maxLimit = null;
	[SerializeField]
	private float m_deceleration = 10.0f;
	[SerializeField]
	private float m_maxSpeed = 10.0f;
	[SerializeField]
	private float m_dragThreshold = 5.0f;
	[SerializeField]
	private bool m_isUIElement = false;
	[SerializeField]
	private bool m_useVelocity = true;
	[SerializeField]
	private bool m_useManualDragStarting = false;


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private bool m_isDragging = false;
	private bool m_draggedThisFrame = false;
	private Vector3 m_dragScreenStartingPoint = Vector3.zero;
	private Vector3 m_colliderScreenStartingPoint = Vector3.zero;
	private Vector3 m_worldSpaceMouseColliderOffset = Vector3.zero;
	private Vector3 m_velocity = Vector3.zero;
	private bool m_ignoringThisDrag = false;
	
	
	// ********************************************************************
	// Properties 
	// ********************************************************************
	public float maxSpeed { set { m_maxSpeed = value; } }
	public Transform cachedTransform { set { m_transform = value; } }
	public bool useVelocity { set { m_useVelocity = value; } }
	public bool isDragging { get { return m_isDragging; } }
	public bool useManualDragging { set { m_useManualDragStarting = value; } }


	// ********************************************************************
	// Events 
	// ********************************************************************
	public delegate void DragStart(GameObject _gameObject);
	public static event DragStart OnDragStart;
	public delegate void DragEnd(GameObject _gameObject);
	public static event DragEnd OnDragEnd;
	public delegate void DragIgnore(GameObject _gameObject);
	public static event DragIgnore OnDragIgnore;



	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, after other functions.
	// ********************************************************************
	void LateUpdate () 
	{
		if (!m_draggedThisFrame && !m_isUIElement)
			m_isDragging = false;
		m_draggedThisFrame = false;

		if (m_useVelocity)
		{
			// Apply speed and deceleration
			if (!m_isDragging)
			{
				if(m_deceleration != 0) m_velocity = m_velocity / m_deceleration;
			}

			float clampedSpeed = Mathf.Min (m_velocity.magnitude, m_maxSpeed);
			m_velocity = m_velocity.normalized * clampedSpeed;
			Vector3 currentWorldPoint = m_transform.position + m_velocity * Time.deltaTime;
			m_transform.position = CheckBounds(currentWorldPoint);
		}

		// Manual drag start
		if (m_useManualDragStarting && m_isDragging)
		{
			if (Input.GetMouseButton(0))
				DragContinued();
			else
				DragEnded();
		}
	}
	
	
	// ********************************************************************
	// Function:	DragStarted()
	// Purpose:		Record initial positions for this drag
	// ********************************************************************
	public void DragStarted()
	{
		//Debug.LogError("DRAG START DETECTED");
		if (m_useManualDragStarting) 
		{
			m_draggedThisFrame = true;
			m_isDragging = true;

			if (OnDragStart != null) OnDragStart(gameObject);
		}
		
		m_ignoringThisDrag = false;

		m_dragScreenStartingPoint = Input.mousePosition;

		if (!m_isUIElement)
			m_colliderScreenStartingPoint = Camera.main.WorldToScreenPoint(m_transform.position);
		else
			m_colliderScreenStartingPoint = m_transform.position;
		
		if (!m_isUIElement)
			m_worldSpaceMouseColliderOffset = m_transform.position - Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_colliderScreenStartingPoint.z));
		else
			m_worldSpaceMouseColliderOffset = m_transform.position - Input.mousePosition;
	}
	
	
	// ********************************************************************
	// Function:	DragContinued()
	// Purpose:		Process drag information
	// ********************************************************************
	private void DragContinued()
	{
		if (!m_allowDragging || m_ignoringThisDrag)
			return;
		
		Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, 
		                                         Input.mousePosition.y, 
		                                         m_colliderScreenStartingPoint.z);

		Vector3 currentWorldPoint = 
			Camera.main.ScreenToWorldPoint(currentScreenPoint)
				+ m_worldSpaceMouseColliderOffset;
		if (m_isUIElement)
			currentWorldPoint = currentScreenPoint + m_worldSpaceMouseColliderOffset;
		
		if (!m_isDragging)
		{
			Vector2 dragDistance = Input.mousePosition - m_dragScreenStartingPoint;
			dragDistance = new Vector3(Mathf.Abs(dragDistance.x), Mathf.Abs(dragDistance.y), 0);
			if (m_restrictDragOffX)
			{
				Debug.LogWarning("PROCESSING DRAG - RESTRICT X ("+dragDistance.x+", "+dragDistance.y+")");

				// we've reached the threshold in the wrong direction, so we ignore this drag
				if (dragDistance.x >= m_dragThreshold)
				{
					Debug.LogError("DRAG TOO FAR IN WRONG DIRECTION - IGNORING");
					if (OnDragIgnore != null) OnDragIgnore(gameObject);
					m_ignoringThisDrag = true;
				}
				// we reached the threshold in the right direction, start dragging
				else if (dragDistance.y >= m_dragThreshold)
				{
					Debug.LogError("DRAG OVER THRESHOLD IN Y - DRAG STARTING");
					if (OnDragStart != null) OnDragStart(gameObject);
					m_isDragging = true;
				}
			}
			else if (m_restrictDragOffY)
			{
				Debug.LogWarning("PROCESSING DRAG - RESTRICT Y");

				// we've reached the threshold in the wrong direction, so we ignore this drag
				if (dragDistance.y >= m_dragThreshold)
					m_ignoringThisDrag = true;
				// we reached the threshold in the right direction, start dragging
				else if (dragDistance.x >= m_dragThreshold)
				{
					if (OnDragStart != null) OnDragStart(gameObject);
					m_isDragging = true;
				}
			}
			else
			{
				if (dragDistance.magnitude >= m_dragThreshold)
				{
//					Debug.LogWarning("PROCESSING DRAG "+dragDistance.magnitude+" >= "+m_dragThreshold);

					if (OnDragStart != null) OnDragStart(gameObject);
					m_isDragging = true;
				}
			}
		}
		
		
		if (m_isDragging && !m_ignoringThisDrag)
		{
			m_draggedThisFrame = true;
			
			Vector3 oldWorldPoint = m_transform.position;

			if (m_useVelocity)
				m_velocity = (currentWorldPoint-oldWorldPoint)/Time.deltaTime;
			else
				m_transform.position = CheckBounds(currentWorldPoint);
		}
	}
	
	
	// ********************************************************************
	// Function:	DragEnded()
	// Purpose:		Perform cleanup after a drag is over
	// ********************************************************************
	public void DragEnded()
	{
		//Debug.LogError("DRAG END DETECTED");

		if (m_isDragging)
		{
			m_isDragging = false;
			if (OnDragEnd != null) OnDragEnd(gameObject);
		}
	}


	// ********************************************************************
	// Function:	OnMouseDown()
	// Purpose:		Called when the user presses the mouse button over this 
	//				collider. 
	// ********************************************************************
	void OnMouseDown()
	{
		if (enabled)
			DragStarted();
	}

	
	// ********************************************************************
	// Function:	OnMouseDrag()
	// Purpose:		Called when the user clicks on the collider and holds 
	//				down the mouse button.
	// ********************************************************************
	void OnMouseDrag()
	{
		if (enabled)
			DragContinued();
	}
	
	
	// ********************************************************************
	// Function:	OnMouseUp()
	// Purpose:		Called when the user releases the mouse button.
	// ********************************************************************
	void OnMouseUp()
	{
		if (enabled && m_isDragging && !m_useManualDragStarting)
			DragEnded();
	}
	
	
	// ********************************************************************
	// Function:	CancelDrag()
	// Purpose:		Kills the current drag until we receive another mouse 
	//				down event.
	// ********************************************************************
	public void CancelDrag()
	{
		m_ignoringThisDrag = true;
	}

	
	// ********************************************************************
	// Function:	CheckBounds()
	// Purpose:		Returns nearest point within world bounds
	// ********************************************************************
	Vector3 CheckBounds(Vector3 toCheck)
	{
		Vector3 oldWorldPoint = m_transform.position;
		Vector3 currentWorldPoint = toCheck;

		if (m_restrictX)
			currentWorldPoint.x = oldWorldPoint.x;
		if (m_restrictY)
			currentWorldPoint.y = oldWorldPoint.y;
		
		if (m_clampToScreenByRenderer && m_renderer != null)
		{
			Vector3 minScreenPoint = new Vector3(0,0,m_colliderScreenStartingPoint.z);
			Vector3 maxScreenPoint = new Vector3(Screen.width,Screen.height,m_colliderScreenStartingPoint.z);
			Vector3 minWorldPoint = Camera.main.ScreenToWorldPoint(minScreenPoint);
			Vector3 maxWorldPoint = Camera.main.ScreenToWorldPoint(maxScreenPoint);
			
			m_transform.position = currentWorldPoint;
			
			if (   m_renderer.bounds.min.x < minWorldPoint.x 
			    || m_renderer.bounds.max.x > maxWorldPoint.x
			    || m_renderer.bounds.min.y < minWorldPoint.y
			    || m_renderer.bounds.max.y > maxWorldPoint.y)
				currentWorldPoint = oldWorldPoint;

			m_transform.position = oldWorldPoint;
		}
		else if (m_clampToScreenByLimits && m_minLimit != null && m_maxLimit != null)
		{
			Vector3 minScreenPoint = new Vector3(0,0,m_colliderScreenStartingPoint.z);
			Vector3 maxScreenPoint = new Vector3(Screen.width,Screen.height,m_colliderScreenStartingPoint.z);
			Vector3 minWorldPoint = Camera.main.ScreenToWorldPoint(minScreenPoint);
			Vector3 maxWorldPoint = Camera.main.ScreenToWorldPoint(maxScreenPoint);
			
			m_transform.position = currentWorldPoint;

			Vector3 minLimitPoint = m_minLimit.position;
			Vector3 maxLimitPoint = m_maxLimit.position;

			if (   minLimitPoint.x <= minWorldPoint.x 
			    || maxLimitPoint.x >= maxWorldPoint.x
			    || minLimitPoint.y <= minWorldPoint.y
			    || maxLimitPoint.y >= maxWorldPoint.y )
				currentWorldPoint = oldWorldPoint;
			
			m_transform.position = oldWorldPoint;
		}

		return currentWorldPoint;
	}
}
