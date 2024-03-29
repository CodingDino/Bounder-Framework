﻿// ************************************************************************ 
// File Name:   TransformExtension.cs 
// Purpose:    	Extends the Transform class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: TransformExtension
// ************************************************************************
public static class TransformExtension 
{

	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static void ResetTransform(this Transform _trans)
	{
		_trans.position = Vector3.zero;
		_trans.localRotation = Quaternion.identity;
		_trans.localScale = new Vector3(1, 1, 1);
	}
	// ********************************************************************
	public static void DestroyChildren(this Transform _trans)
	{
		for (int i = 0; i < _trans.childCount; ++i)
		{
			GameObject.Destroy (_trans.GetChild(i).gameObject);
		}
    }
    // ********************************************************************
    public static void DestroyChildrenImmediate(this Transform _trans)
    {
        for (int i = 0; i < _trans.childCount; ++i)
        {
            GameObject.DestroyImmediate(_trans.GetChild(i).gameObject);
        }
    }
    // ********************************************************************
    public static void SetPosX(this Transform _trans, float _pos)
	{
		_trans.position = new Vector3(_pos,
		                              _trans.position.y,
		                              _trans.position.z);
	}
	// ********************************************************************
	public static void SetPosY(this Transform _trans, float _pos)
	{
		_trans.position = new Vector3(_trans.position.x,
		                              _pos,
		                              _trans.position.z);
	}
	// ********************************************************************
	public static void SetPosZ(this Transform _trans, float _pos)
	{
		_trans.position = new Vector3(_trans.position.x,
		                              _trans.position.y,
		                              _pos);
	}
	// ********************************************************************
	public static void SetLocalPosX(this Transform _trans, float _pos)
	{
		_trans.localPosition = new Vector3(_pos,
		                                   _trans.localPosition.y,
		                                   _trans.localPosition.z);
	}
	// ********************************************************************
	public static void SetLocalPosY(this Transform _trans, float _pos)
	{
		_trans.localPosition = new Vector3(_trans.localPosition.x,
		                                   _pos,
		                                   _trans.localPosition.z);
	}
	// ********************************************************************
	public static void SetLocalPosZ(this Transform _trans, float _pos)
	{
		_trans.localPosition = new Vector3(_trans.localPosition.x,
		                                   _trans.localPosition.y,
		                                   _pos);
	}
	// ********************************************************************
	public static Vector3 Direction(this Transform _trans, Vector3 _relativeTo)
	{
		return (_trans.rotation*_relativeTo).normalized;
	}
	// ********************************************************************
	public static void SetGlobalScale (this Transform _transform, Vector3 _globalScale)
	{
		_transform.localScale = Vector3.one;
		_transform.localScale = new Vector3 (_globalScale.x/_transform.lossyScale.x, 
		                                     _globalScale.y/_transform.lossyScale.y, 
		                                     _globalScale.z/_transform.lossyScale.z);
    }
    // ********************************************************************
    public static void SetRotationAngle(this Transform _trans, float _rot)
    {
        _trans.eulerAngles = Vector3.forward * _rot;
    }
    // ********************************************************************
    public static void SetLocalRotationAngle(this Transform _trans, float _rot)
    {
        _trans.localRotation = Quaternion.Euler(Vector3.forward * _rot);
    }
    // ********************************************************************
    #endregion
    // ********************************************************************

}
#endregion
// ************************************************************************
