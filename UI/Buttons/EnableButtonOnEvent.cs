using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ************************************************************************ 
#region Class: ButtonEnableEvent
// ************************************************************************
public class ButtonEnableEvent : GameEvent 
{
	public string buttonName;
	public bool enable = true;
	public ButtonEnableEvent(string _buttonName, bool _enable)
	{
		buttonName = _buttonName;
		enable= _enable;
	}
}
#endregion
// ************************************************************************

[RequireComponent(typeof(Button))]
public class EnableButtonOnEvent : MonoBehaviour 
{
	void OnEnable()
	{
		Events.AddListener<ButtonEnableEvent>(OnDisableEvent);
	}

	void OnDisable()
	{
		Events.RemoveListener<ButtonEnableEvent>(OnDisableEvent);
	}

	void OnDisableEvent(ButtonEnableEvent _event)
	{
		if (_event.buttonName == name)
		{
			GetComponent<Button>().interactable = _event.enable;
		}
	}
}
