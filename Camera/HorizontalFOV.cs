// ************************************************************************ 
// File Name:   HorizontalFOV.cs 
// Purpose:    	Fixes camera width and scales height based on aspect ratio
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 

// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
#endregion
// ************************************************************************


// ************************************************************************
namespace Bounder.Framework 
{ 

// ************************************************************************
#region Class: HorizontalFOV
// ************************************************************************
[ExecuteInEditMode]
public class HorizontalFOV : MonoBehaviour
{
	// ********************************************************************
	#region Exposed Data Members
	// ********************************************************************
	[SerializeField]
	private float orthoSize = 5;
	[SerializeField]
	private Camera FOVCamera = null;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
    void Update()
    {
		if (FOVCamera.pixelHeight == 0 || FOVCamera.pixelWidth == 0)
			return;
		
        float aspectRatio = ((float)FOVCamera.pixelWidth) / ((float)FOVCamera.pixelHeight);

        FOVCamera.orthographicSize = orthoSize / aspectRatio;
	}
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************

}
// ************************************************************************
