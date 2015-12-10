// ************************************************************************ 
// File Name:   ButtonImplementation.cs 
// Purpose:    	Functionality for a button
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Attributes 
// ************************************************************************ 


// ************************************************************************ 
// Class: ButtonImplementation
// ************************************************************************ 
public class ButtonImplementation : MonoBehaviour {


	// ********************************************************************
	// Function:	OnHoverEnter()
	// Purpose:		Called when the button is first hovered over
	// ********************************************************************
	public virtual void OnHoverEnter()
	{
	}


	// ********************************************************************
	// Function:	OnHoverExit()
	// Purpose:		Called when the button is no longer hovered over
	// ********************************************************************
	public virtual void OnHoverExit()
	{
	}
	
	
	// ********************************************************************
	// Function:	OnHoverStay()
	// Purpose:		Called every frame the button is hovered over
	// ********************************************************************
	public virtual void OnHoverStay()
	{
	}
	
	
	// ********************************************************************
	// Function:	OnClick()
	// Purpose:		Called when the button is clicked
	// ********************************************************************
	public virtual void OnClick()
	{
	}

}