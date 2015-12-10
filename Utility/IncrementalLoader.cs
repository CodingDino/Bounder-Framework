// ************************************************************************ 
// File Name:   IncrementalLoader.cs 
// Purpose:    	Interface for checking progress on incremental loading of 
//				game assets
// Project:		Framework
// Author:      Sarah Herzog
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using System.Collections;


// ************************************************************************ 
// Interface: IncrementalLoader
// ************************************************************************ 
public interface IncrementalLoader {

	
	// ********************************************************************
	// Function:	GetProgress()
	// Purpose:		Returns progress of loader, between 0 and 1.
	// ********************************************************************
	float GetProgress();
	
	
	// ********************************************************************
	// Function:	GetCurrentAction()
	// Purpose:		Returns text describing current action
	// ********************************************************************
	string GetCurrentAction();

}
