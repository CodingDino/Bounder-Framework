// ************************************************************************ 
// File Name:   LoadingSpinnerPanel.cs 
// Purpose:    	Show a loading spinner and block input
// Project:		Armorued Engines
// Author:      Sarah Herzog  
// Copyright: 	2018 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BounderFramework;
using UnityEngine.UI;
using Fiftytwo;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: LoadingSpinnerPanelData
// ************************************************************************
[System.Serializable]
public class LoadingSpinnerPanelData : PanelData 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public string loadingText;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Constructors 
	// ********************************************************************
	public LoadingSpinnerPanelData(string _loadingText = "",
	                         PanelState _startingState = PanelState.HIDDEN,
	                         PanelLimitOverride _limitOverride = PanelLimitOverride.REPLACE)
		: base(_startingState, _limitOverride)
	{
		loadingText = _loadingText;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: LoadingSpinnerPanel
// ************************************************************************
public class LoadingSpinnerPanel : Panel 
{

	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private Text m_loadingText;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Panel Methods 
	// ********************************************************************
	protected override void _Initialise(PanelData _data) 
	{
		LoadingSpinnerPanelData castData = _data as LoadingSpinnerPanelData;
		if (castData != null)
		{
			m_loadingText.text = castData.loadingText;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
