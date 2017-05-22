// ************************************************************************ 
// File Name:   ColorExtension.cs 
// Purpose:    	Extends the Color class
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
#region Class: ColorExtension
// ************************************************************************
public static class ColorExtension 
{
	// ********************************************************************
	#region Extension Methods 
	// ********************************************************************
	public static string ToHex(this Color _color)
	{
		Color32 byteColor = _color;
		return byteColor.ToHex();
	}
	// ********************************************************************
	public static string ToHex(this Color32 _color)
	{
		return "#" + _color.r.ToString("X2") + _color.g.ToString("X2") + _color.b.ToString("X2");
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Static Methods 
	// ********************************************************************
	public static Color CreateFromHex(string _hex)
	{
		if (_hex.NullOrEmpty())
		{
			Debug.LogError("CreateFromHex failed - string empty");
			return Color.clear;
		}
		_hex = _hex.Replace("#","");
		_hex = _hex.Replace("0x","");
		if (_hex.Length < 6)
		{
			Debug.LogError("CreateFromHex failed - too short string "+_hex);
			return Color.clear;
		}

		_hex = _hex.ToUpper();
		Color color = Color.black;
		for (int i = 0; i < 4 && i*2 < _hex.Length; ++i)
		{
			color[i] = ((float)byte.Parse(_hex.Substring(i*2,2), System.Globalization.NumberStyles.HexNumber))/255.0f;
		}

		return color;
	}
	// ********************************************************************
	#endregion
	// ********************************************************************

}
#endregion
// ************************************************************************
