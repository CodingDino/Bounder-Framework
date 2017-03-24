// ************************************************************************ 
// File Name:   HorizontalScrollSnap.cs 
// Purpose:    	Object can be click+dragged or touch+dragged around the 
//				screen
// Project:		Framework
// Author:      Sarah Herzog (based on work by BinaryX)   
// Copyright: 	2015 Bounder Games
// ************************************************************************


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BounderFramework;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 
[RequireComponent(typeof(ScrollRect))]


// ************************************************************************ 
// Class: HorizontalScrollSnap
// ************************************************************************ 
public class HorizontalScrollSnap : MonoBehaviour
	, IBeginDragHandler
	, IEndDragHandler
	, IDragHandler
{
	
	// ********************************************************************
	// Serialized Data Members 
	// ********************************************************************	
	[SerializeField]
	[Tooltip("Layout group being controlled by scroller, used for pagination.")]
	private HorizontalLayoutGroup m_layoutGroup;
	[SerializeField]
	[Tooltip("Deceleration for the scroller, applied after the scroller is released.")]
	private float m_deceleration = 0.001f;
	[SerializeField]
	[Tooltip("Whether we allow objects to be dragged off the scroller")]
	private bool m_allowDragOff = false;
	[SerializeField]
	[Tooltip("Distance drag needed to drag an object off the scroller")]
	private float m_dragOffThreshold = 5;
	[SerializeField]
	[Tooltip("Page of the layout group to start on.")]
	private int m_startingPage = 0;
	[SerializeField]
	[Tooltip("How long it takes to scroll to a target.")]
	private float m_lerpDuration = 0.25f;


	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Transform m_screensContainer;
	private int m_screens = 1;
	private System.Collections.Generic.List<Vector3> m_positions;
	private ScrollRect m_scrollRect;
	private Vector3 m_lerpTarget;
	private Vector3 m_lerpStartPos;
	private float m_lerpStartTime;
	private bool m_lerp;
	private bool m_momentumEffectivelyStopped = false;
	private bool m_ignoringThisDrag = false;
	private bool m_allowingThisDrag = false;
	private bool m_currentlyDragging = false;
	private Vector2 m_touchStartPoint;
	private Vector2 m_contentsStartPoint;


	// ********************************************************************
	// Properties 
	// ********************************************************************
	public int numScreens { get { return m_screens;}}
	public bool isStopped { get { return m_momentumEffectivelyStopped; } }
	public int startingPage { set {m_startingPage = value; } }
	
	
	// ********************************************************************
	// Events 
	// ********************************************************************
	public delegate void DragStart(GameObject _callingObject);
	public static event DragStart OnDragStart;
	public delegate void DragEnd(GameObject _callingObject);
	public static event DragEnd OnDragEnd;
	public delegate void DragIgnore(GameObject _callingObject);
	public static event DragIgnore OnDragIgnore;
	public delegate void DragOff(GameObject _callingObject, GameObject _draggedOff, PointerEventData _eventData);
	public static event DragOff OnDragOff;

	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Use this for initialization.
	// ********************************************************************
	void Start()
	{
		InitializeScreens();
	}


	// ********************************************************************
	// Function:	InitializeScreens()
	// Purpose:		Sets up screens, should be called when screen content 
	//				is modified
	// ********************************************************************
	public void InitializeScreens()
	{
		m_scrollRect = gameObject.GetComponent<ScrollRect>();
		m_screensContainer = m_scrollRect.content;

		m_screens = m_screensContainer.childCount;

		DistributePages();
		
		m_positions = new System.Collections.Generic.List<Vector3>();
		
		if (m_screens > 0)
		{
			float spacing = m_layoutGroup.spacing;
			float xPos = ((float)(m_screens-1))*0.5f * spacing;
			for (int i = 0; i < m_screens; ++i)
			{
				Vector3 pos = m_screensContainer.localPosition;
				pos.x = xPos;
				m_positions.Add(pos);
				xPos -= spacing;
			}

			if (m_startingPage >= m_positions.Count)
				m_startingPage = m_positions.Count - 1;
			m_screensContainer.localPosition = m_positions[m_startingPage];
		}
	}


	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update()
	{
	}
	
	
	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, after other objects have 
	//				updated.
	// ********************************************************************
	void LateUpdate()
	{
		if (m_lerp)
		{
			if (Time.time - m_lerpStartTime < m_lerpDuration)
			{
				float xPos = Easing.Apply(EasingFunction.QuadEaseOut,
				                          Time.time - m_lerpStartTime,
				                          m_lerpStartPos.x,
				                          m_lerpTarget.x - m_lerpStartPos.x,
				                          m_lerpDuration);
				m_screensContainer.localPosition = new Vector3(xPos, m_screensContainer.localPosition.y, m_screensContainer.localPosition.z);
			}
			else
			{
				m_lerp = false;
				m_momentumEffectivelyStopped = true;
				m_contentsStartPoint = m_layoutGroup.transform.position; // to keep current drag in right place
			}
		}
		else if (m_allowDragOff && !m_allowingThisDrag && m_currentlyDragging )
		{
			m_layoutGroup.transform.position = m_contentsStartPoint;
		}
	}


	// ********************************************************************
	// Function:	NextScreen()
	// Purpose:		Function for switching screens with buttons.
	// ********************************************************************
	public void NextScreen()
	{
		Vector2 targetPos = m_screensContainer.localPosition;
		targetPos.x = targetPos.x - m_layoutGroup.spacing;
		LerpToTarget(FindClosestFrom(targetPos, m_positions));
	}


	// ********************************************************************
	// Function:	PreviousScreen()
	// Purpose:		Function for switching screens with buttons.
	// ********************************************************************
	public void PreviousScreen()
	{
		Vector2 targetPos = m_screensContainer.localPosition;
		targetPos.x = targetPos.x + m_layoutGroup.spacing;
		LerpToTarget(FindClosestFrom(targetPos, m_positions));
	}

	// ********************************************************************

	public int GetCenterItemIndex()
	{
		if (m_screensContainer.transform.childCount == 0)
			return 0;
		int screenIndex = Mathf.RoundToInt(((float)m_screens - 1.0f)*0.5f-transform.position.x/m_layoutGroup.spacing);
		if (screenIndex < 0)
			screenIndex = 0;
		if (screenIndex >= m_screensContainer.transform.childCount)
			screenIndex = m_screensContainer.transform.childCount-1;
		return screenIndex;
	}

	// ********************************************************************

	public GameObject GetCenterItem()
	{
		if (m_screensContainer.transform.childCount == 0)
			return null;
		return m_screensContainer.transform.GetChild(GetCenterItemIndex()).gameObject;
	}

	// ********************************************************************

	public void ScrollToTarget(GameObject _object)
	{
		int screenIndex = -1;
		for (int i = 0; i < m_screens; ++i)
		{
			if (m_screensContainer.transform.GetChild(i).gameObject == _object)
			{
				screenIndex = i;
				break;
			}
		}

		if (screenIndex == -1)
		{
			Debug.LogError("Failed to scroll to target - not found in screens");
			return;
		}
		Vector2 targetPos = m_screensContainer.localPosition;
		targetPos.x =  m_layoutGroup.spacing * 0.5f * (float)(m_screens - (2*screenIndex+1));
		LerpToTarget(FindClosestFrom(targetPos, m_positions));
	}

	
	// ********************************************************************
	// Function:	FindClosestFrom()
	// Purpose:		Find the closest registered point to the point.
	// ********************************************************************
	private Vector3 FindClosestFrom(Vector3 start, System.Collections.Generic.List<Vector3> positions)
	{
	    Vector3 closest = Vector3.zero;
	    float distance = Mathf.Infinity;

	    foreach (Vector3 position in m_positions)
	    {
	        if (Vector3.Distance(start, position) < distance)
	        {
	            distance = Vector3.Distance(start, position);
	            closest = position;
	        }
	    }

	    return closest;
	}



	// ********************************************************************
	// Function:	CurrentScreen()
	// Purpose:		Returns the current screen.
	// ********************************************************************
	public int CurrentScreen()
	{
		int item = 0;
		float distance = Mathf.Infinity;

		for (int i = 0; i < m_positions.Count; ++i)
		{
			Vector3 position = m_positions[i];
			if (Vector3.Distance(m_screensContainer.localPosition, position) < distance)
			{
				distance = Vector3.Distance(m_screensContainer.localPosition, position);
				item = i;
			}
		}

		return item;
	}


	// ********************************************************************
	// Function:	DistributePages()
	// Purpose:		Used for changing between screen resolutions.
	// ********************************************************************
	private void DistributePages()
	{
	    int _offset = 0;
	    int _step = Screen.width;
	    int _dimension = 0;

	    int currentXPosition = 0;

	    for (int i = 0; i < m_screensContainer.transform.childCount; i++)
	    {
	        RectTransform child = m_screensContainer.transform.GetChild(i).GetComponent<RectTransform>();
	        currentXPosition = _offset + i * _step;
	        child.anchoredPosition = new Vector2(currentXPosition, 0f);
	        child.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y);
	    }

	    _dimension = currentXPosition + _offset * -1;

	    m_screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(_dimension, 0f);
	}

	public void LerpToTarget(Vector3 _lerpTarget)
	{
		m_lerp = true;
		m_momentumEffectivelyStopped = false;
		m_lerpTarget = _lerpTarget;
		m_lerpStartPos = m_screensContainer.localPosition;
		m_lerpStartTime = Time.time;
	}

#region Interfaces

	// ********************************************************************
	// Function:	OnBeginDrag()
	// Purpose:		Handling for when the content is beging being dragged.
	// ********************************************************************
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (OnDragStart != null) OnDragStart(gameObject);

		LogManager.Log("OnBeginDrag()",
		               LogCategory.INPUT,
		               LogSeverity.LOG, 
		               "HorizontalScrollSnap",
		               gameObject);

		m_currentlyDragging = true;
		m_ignoringThisDrag = false;
		m_allowingThisDrag = false;

		m_touchStartPoint = Input.mousePosition;
		m_contentsStartPoint = m_layoutGroup.transform.position;
	}


	// ********************************************************************
	// Function:	OnEndDrag()
	// Purpose:		Called by a BaseInputModule when a drag is ended.
	// ********************************************************************
	public void OnEndDrag(PointerEventData eventData)
	{
		if (OnDragEnd != null) OnDragEnd(gameObject);

		LogManager.Log("OnEndDrag()",
		               LogCategory.INPUT,
		               LogSeverity.LOG, 
		               "HorizontalScrollSnap",
		               gameObject);

		if (m_scrollRect.horizontal && !m_ignoringThisDrag)
	    {
			float v = m_scrollRect.velocity.x;
			float t = 10.0f;
			float c = m_deceleration;
			float d = 0;
			float numTimeSteps = 100.0f;
			float timeStep = t/numTimeSteps;
			for (int i = 0; i <= numTimeSteps; ++i)
			{
				d += v*timeStep;
				v = (c/timeStep)*v;
			}
			if (d > m_layoutGroup.spacing)
				d += m_layoutGroup.spacing * 0.5f * Mathf.Sign(d);

			Vector2 targetPos = m_screensContainer.localPosition;
			targetPos.x = targetPos.x + d;
			LerpToTarget(FindClosestFrom(targetPos, m_positions));
	    }
		else
			m_momentumEffectivelyStopped = true;

		m_ignoringThisDrag = false;
		m_allowingThisDrag = false;
		m_currentlyDragging = false;
	}


	// ********************************************************************
	// Function:	OnDrag()
	// Purpose:		When draging is occuring this will be called every time 
	//				the cursor is moved.
	// ********************************************************************
	public void OnDrag(PointerEventData eventData)
	{
		LogManager.Log("OnDrag()",
		               LogCategory.INPUT,
		               LogSeverity.LOG, 
		               "HorizontalScrollSnap",
		               gameObject);

		if (m_allowDragOff)
		{
			Vector2 currentScreenPoint = Input.mousePosition;
			
			if (!m_allowingThisDrag && !m_ignoringThisDrag)
			{
				Vector2 dragScreenDistance = currentScreenPoint - m_touchStartPoint;
				dragScreenDistance = new Vector3(Mathf.Abs(dragScreenDistance.x), Mathf.Abs(dragScreenDistance.y), 0);

				if (dragScreenDistance.y >= m_dragOffThreshold)
				{
					m_ignoringThisDrag = true;
				}
				else if (dragScreenDistance.x >= m_dragOffThreshold)
				{
					m_allowingThisDrag = true;
				}

				if (m_ignoringThisDrag)
				{
					LogManager.Log("Ignoring drag",
					   LogCategory.INPUT,
					   LogSeverity.LOG, 
					   "HorizontalScrollSnap",
					   gameObject);
					
					if (OnDragIgnore != null) 
						OnDragIgnore(gameObject);

					// Check children for draggable objects, manually start drag for whicherever one original touch was on
					for (int i = 0; i < m_screensContainer.transform.childCount; i++)
					{
						RectTransform childTransform = m_screensContainer.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>();
						Rect childRect = childTransform.rect;
						Vector2 touchStartPointLocal = m_touchStartPoint - (Vector2)Camera.main.WorldToScreenPoint(childTransform.position);
						if (childRect.Contains(touchStartPointLocal))
						{
							if (OnDragOff != null) 
								OnDragOff(gameObject, m_screensContainer.transform.GetChild(i).gameObject, eventData);
							break;
						}
					}
				}
			}
		}
	}

#endregion

}