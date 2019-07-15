// ************************************************************************ 
// File Name:   RandomiseFacing.cs 
// Purpose:    	Sets random facing on start
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************
namespace Bounder.Framework { 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bounder.Framework;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: RandomiseFacing
// ************************************************************************ 
public class RandomiseFacing : MonoBehaviour 
{
	// ********************************************************************
	#region MonoBehaviour Methods
	// ********************************************************************
	void Start()
	{
		bool flip = Random.value > 0.5f;
		if (flip)
		{
			Vector3 scale = transform.localScale;
			scale.x = -1.0f;
			transform.localScale = scale;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************

}
