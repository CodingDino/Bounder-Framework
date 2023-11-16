// ************************************************************************ 
// File Name:   ListExtension.cs 
// Purpose:    	Extends the List class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ArrayExtension
// ************************************************************************
public static class ArrayExtension
{
    // ********************************************************************
    #region Extension Methods 
    // ********************************************************************
    public static T RandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
            return default(T);
        return array[Random.Range(0, array.Length)];
    }
    // ********************************************************************
    #endregion
    // ********************************************************************

}
#endregion
// ************************************************************************
