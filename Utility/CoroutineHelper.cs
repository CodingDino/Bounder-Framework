// ************************************************************************ 
// File Name:   CoroutineHelper.cs 
// Purpose:    	Helper functions for Coroutines
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: CoroutineHelper
// ************************************************************************
public static class CoroutineHelper 
{
	// ********************************************************************
	#region Helper Methods 
	// ********************************************************************
	public static void CompleteCoroutineImmediately(IEnumerator _coroutine)
	{
		bool finished = false;
		while(!finished)
			finished = !_coroutine.MoveNext();
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
