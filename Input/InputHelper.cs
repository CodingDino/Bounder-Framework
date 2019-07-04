// ************************************************************************ 
// File Name:   InputHelper.cs 
// Purpose:    	Wraps various common input tasks
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2019 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using Rewired;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: InputHelper
// ************************************************************************
public static class InputHelper
{
    // ********************************************************************
    #region Extension Methods 
    // ********************************************************************
    public static string GetDisplayNameForAction(string _action, int _playerIndex = 0)
    {
        Player player = Rewired.ReInput.players.GetPlayer(_playerIndex);

        // If set to true, only enabled maps will be returned
        bool skipDisabledMaps = true;

        // Get the first ActionElementMap of any type with the _action
        return player.controllers.maps.GetFirstElementMapWithAction(_action, skipDisabledMaps).elementIdentifierName;
    }
    // ********************************************************************
    #endregion
    // ********************************************************************

}
#endregion
// ************************************************************************
