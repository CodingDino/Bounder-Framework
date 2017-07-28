using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClosePanel : MonoBehaviour {


	// ********************************************************************
	#region Public Methods 
	// ********************************************************************
	public void ClosePanel (string _id) 
	{
		PanelManager.ClosePanel(_id);
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
	
}
