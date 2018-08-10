// ************************************************************************ 
// File Name:   DraggableObject.cs 
// Purpose:    	Object can be click+dragged or touch+dragged around the 
//				screen
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
#endregion
// ************************************************************************ 


// ************************************************************************
namespace BounderFramework 
{ 

// ************************************************************************
#region Class: DraggableObject
// ************************************************************************
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[Header("Drag")]
	[SerializeField]
	[Tooltip("The transform that will be dragged. Defaults to this transform.")]
	private Transform m_transform = null;
	[SerializeField]
	[Tooltip("Allow dragging in a specific axis")]
	private Vector2Bool m_allowDragDirection = new Vector2Bool(true, true);
	[SerializeField]
	[Tooltip("Allow dragging in a particular axis to cause the drag to be started.")]
	private Vector2Bool m_allowDragStartDirection = new Vector2Bool(true, true);
	[SerializeField]
	[Tooltip("Distance drag needed to start the object dragging")]
	private float m_dragStartThreshold = 5.0f;
	[SerializeField]
	[Tooltip("Distance drag needed in wrong direction to ignore the drag. 0 = same as Drag Start Threshold.")]
	private float m_dragIgnoreThreshold = 0.0f;
	// ********************************************************************
	[Header("Clamp")]
	[SerializeField]
	[Tooltip("Force the object to be clamped to screen. Uses screen bounds unless bounds are set")]
	private bool m_clamp = false;
	[SerializeField]
	[Tooltip("Padding for clamping, to keep parts of the object from going out of bounds")]
	private Vector2 m_clampPadding;
	[SerializeField]
	[Tooltip("Bounds for clamping. Uses screen bounds if zero.")]
	private Rect m_clampBounds;
	// ********************************************************************
	[Header("Movement")]
	[SerializeField]
	[Tooltip("If velocity should be used to calculate the object's position, else it should move instantly.")]
	private bool m_useVelocity = true;
	[SerializeField]
	[Tooltip("How quickly the object decelerates once dragging is finished.")]
	private float m_deceleration = 10.0f;
	[SerializeField]
	[Tooltip("Max speed the object can move while dragging.")]
	private float m_maxSpeed = 10.0f;
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public Transform draggedTransform { set { m_transform = value; } }
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private bool m_isDragging = false;
	private Vector2 m_dragScreenStartingPoint = Vector3.zero;
	private Vector3 m_dragPointOffset = Vector3.zero;
	private Vector3 m_velocity = Vector3.zero;
	private bool m_ignoringThisDrag = false;
	private Canvas m_canvas = null;
	private bool m_bForcingDrag = false;
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public bool isDragging { get { return m_isDragging; } }
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Events 
	// ********************************************************************
	public delegate void DragEvent(Vector2 _eventData);
	public event DragEvent OnDragStart;
	public event DragEvent OnDragEnd;
	public event DragEvent OnDragIgnore;
	// ********************************************************************
	public delegate void DragEventGlobal(GameObject _gameObject, Vector2 _eventData);
	public static event DragEventGlobal OnDragStartGlobal;
	public static event DragEventGlobal OnDragEndGlobal;
	public static event DragEventGlobal OnDragIgnoreGlobal;
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Awake()
	{
		if (m_transform == null)
			m_transform = transform;
		m_canvas = GetComponentInParent<Canvas>();
		if (m_dragIgnoreThreshold == 0)
			m_dragIgnoreThreshold = m_dragStartThreshold;
	}
	// ********************************************************************
	void LateUpdate () 
	{
		if (m_bForcingDrag)
		{
			m_isDragging = Input.GetMouseButton(0);
			m_bForcingDrag = m_isDragging;
			if (m_isDragging)
			{
				DragProcess(Input.mousePosition);
			}
			else
			{
				DragEnd(Input.mousePosition);
			}
		}

		// Apply speed and deceleration
		if (!m_isDragging)
		{
			if(m_deceleration != 0) m_velocity = m_velocity / m_deceleration;
		}

		if (m_useVelocity)
		{
			float clampedSpeed = Mathf.Min (m_velocity.magnitude, m_maxSpeed);
			m_velocity = m_velocity.normalized * clampedSpeed;
			Vector3 currentWorldPoint = m_transform.position + m_velocity * Time.deltaTime;
			ApplyDragPosition(currentWorldPoint);

			currentWorldPoint[0] = 0;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region IDragHandler Methods
	// ********************************************************************
	public void OnBeginDrag(PointerEventData _eventData)
	{
		DragStart(_eventData.position);
	}
	// ********************************************************************
	public void OnDrag(PointerEventData _eventData)
	{
		if (m_ignoringThisDrag)
			return;
		
		LogManager.Log("OnDrag()",
		               LogCategory.INPUT,
		               LogSeverity.LOG, 
		               "DraggableObject",
		               gameObject);

		// Determine if we should start dragging
		if (!m_isDragging)
		{
			Vector2 dragScreenDistance = _eventData.position - m_dragScreenStartingPoint;
			dragScreenDistance = new Vector3(Mathf.Abs(dragScreenDistance.x), Mathf.Abs(dragScreenDistance.y), 0);

			if (!m_allowDragStartDirection.x)
			{
				if (dragScreenDistance.x >= m_dragIgnoreThreshold)
				{
					m_ignoringThisDrag = true;
				}
				dragScreenDistance.x = 0;
			}

			if (!m_allowDragStartDirection.y)
			{
				if (dragScreenDistance.y >= m_dragIgnoreThreshold)
				{
					m_ignoringThisDrag = true;
				}
				dragScreenDistance.y = 0;
			}

			if (m_ignoringThisDrag)
			{
				LogManager.Log("Dragged Ignored",
				               LogCategory.INPUT, 
				               LogSeverity.LOG, 
				               "DraggableObject",
				               gameObject);
				if (OnDragIgnore != null) 
					OnDragIgnore(_eventData.position);
				if (OnDragIgnoreGlobal != null) 
					OnDragIgnoreGlobal(gameObject, _eventData.position);
				return;
			}

			if (dragScreenDistance.magnitude >= m_dragStartThreshold)
			{
				DragOff(_eventData.position);
			}
		}

		// Process dragging
		else // if m_isDragging
		{
			DragProcess(_eventData.position);
		}
	}
	// ********************************************************************
	public void OnEndDrag(PointerEventData _eventData)
	{
		if (!m_ignoringThisDrag && m_isDragging)
			DragEnd(_eventData.position);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Methods
	// ********************************************************************
	public void ForceDragStart(Vector2 _mousePosition)
	{
		m_bForcingDrag = true;
		DragStart(_mousePosition);
		DragOff(_mousePosition);
  	}
	// ********************************************************************
	public void CancelDrag()
	{
		LogManager.Log("CancelDrag()",
		               LogCategory.INPUT, 
		               LogSeverity.LOG, 
		               "DraggableObject",
		               gameObject);
		m_ignoringThisDrag = true;
		m_isDragging = false;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Methods
	// ********************************************************************
	private void DragStart(Vector2 _screenPos)
	{
		LogManager.Log("DragStart()",
		               LogCategory.INPUT, 
		               LogSeverity.LOG, 
		               "DraggableObject",
		               gameObject);

		m_ignoringThisDrag = false;
		m_dragScreenStartingPoint = _screenPos;
		Vector3 worldDragStartingPoint = ScreenToDragPoint(m_dragScreenStartingPoint);
		Vector3 currentPoint = m_transform.position;
		m_dragPointOffset = currentPoint - worldDragStartingPoint;
	}
	// ********************************************************************
	private void DragOff(Vector2 _screenPos)
	{
		m_isDragging = true;

		LogManager.Log("DragOff()",
		               LogCategory.INPUT, 
		               LogSeverity.LOG, 
		               "DraggableObject",
		               gameObject);

		if (OnDragStart != null) 
			OnDragStart(_screenPos);
		if (OnDragStartGlobal != null) 
			OnDragStartGlobal(gameObject, _screenPos);
	}
	// ********************************************************************
	private void DragProcess(Vector2 _screenPos)
	{
		Vector3 currentDragPoint = ScreenToDragPoint(_screenPos);
		Vector3 oldDragPoint = m_transform.position;

		LogManager.Log("Processing Drag s:"+_screenPos+" d:"+currentDragPoint,
		               LogCategory.INPUT, 
		               LogSeverity.SPAMMY_LOG, 
		               "DraggableObject",
		               gameObject);

		if (m_useVelocity)
			m_velocity = (currentDragPoint-oldDragPoint)/Time.deltaTime;
		else
			ApplyDragPosition(currentDragPoint);
	}
	// ********************************************************************
	private void DragEnd(Vector2 _screenPos)
	{
		LogManager.Log("DragEnd()",
		               LogCategory.INPUT, 
		               LogSeverity.LOG, 
		               "DraggableObject",
		               gameObject);

		m_isDragging = false;
		if (OnDragEnd != null) 
			OnDragEnd(_screenPos);
		if (OnDragEndGlobal != null) 
			OnDragEndGlobal(gameObject, _screenPos);
	}
	// ********************************************************************
	private Vector3 ScreenToDragPoint(Vector2 _screenPoint)
	{
		if (m_canvas)
		{
			return m_canvas.ScreenToCanvasPoint(Camera.main, _screenPoint);
		}
		else
		{
			return Camera.main.ScreenToWorldPoint(_screenPoint);
		}
	}
	// ********************************************************************
	private void ApplyDragPosition(Vector3 _newPos)
	{
		Vector3 adjustedPos = _newPos;

		// Apply offset
		adjustedPos += m_dragPointOffset;

		// Check Bounds
		if (m_clamp)
		{
			Vector3 oldPoint = m_transform.position;
			Vector3 currentPoint = adjustedPos;

			if (!m_allowDragDirection.x)
				currentPoint.x = oldPoint.x;
			if (!m_allowDragDirection.y)
				currentPoint.y = oldPoint.y;

			Rect bounds = m_clampBounds;

			// Get Screen Bounds
			// NOTE: Done every frame since screen size can change
			if (bounds == Rect.zero)
			{
				bounds.min = ScreenToDragPoint(Vector2.zero);
				bounds.max = ScreenToDragPoint(new Vector2(Screen.width, Screen.height));
			}

			// Add Padding
			bounds.min += m_clampPadding;
			bounds.max += m_clampPadding;

			// Check Bounds
			if (!bounds.Contains(currentPoint))
				currentPoint = oldPoint; // TODO: Instead, allow movement along non-bounded axis!

			adjustedPos = currentPoint;
		}

		// Fix Z value to current
		adjustedPos.z = m_transform.position.z;

		// Apply
		m_transform.position = adjustedPos;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ********************************************************************
}