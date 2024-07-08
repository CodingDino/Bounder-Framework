// **************************************************************************** 
// File Name:   TimeScaleLock.cs 
// Purpose:    	Handler for changing Time.timescale
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2024 Bounder Games
// ****************************************************************************
namespace Bounder.Framework
{
    using System.Collections.Generic;

    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************
    // Class: TimeScaleLock 
    // ************************************************************************
    public static class TimeScaleLock
    {
        // ********************************************************************
        #region Struct TimeLockEntry
        // ********************************************************************
        public struct TimeLockEntry
        {

            public object lockingObject;
            public float timeScale;

            public TimeLockEntry(object _lockingObject, float _timeScale)
            {
                lockingObject = _lockingObject;
                timeScale = _timeScale;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Static Variables
        // ********************************************************************
        static float s_originalTimeScale = 1f;
        static bool s_initialised = false;
        static List<TimeLockEntry> s_activeLocks = new();
        // ********************************************************************
        #endregion
        // ********************************************************************



        // ********************************************************************
        #region Public Functions
        // ********************************************************************
        public static void SetTimeScale(object _lockingObject, float _newTimeScale)
        {
            Initialise();
            Time.timeScale = _newTimeScale;
            s_activeLocks.Add(new TimeLockEntry(_lockingObject, _newTimeScale));
        }
        // ********************************************************************
        public static void ReleaseTimeScale(object _lockingObject)
        {
            // Check if this was the most recent active lock
            bool wasMostRecentLock = false;

            // Remove the lock
            for (int i = 0; i < s_activeLocks.Count; ++i)
            {
                if (s_activeLocks[i].lockingObject == _lockingObject)
                {
                    if (i == s_activeLocks.Count - 1)
                        wasMostRecentLock = true;
                    s_activeLocks.RemoveAt(i);
                    break;
                }
            }

            // If there are no more locks, return to original time
            if (s_activeLocks.Count == 0)
            {
                Time.timeScale = s_originalTimeScale;
            }
            // if this was the most recent lock, activate the next lock's time
            else if (wasMostRecentLock)
            {
                Time.timeScale = s_activeLocks.Back().timeScale;
            }
            // Otherwise no need to do anything with timeScale as our lock wasn't the one active.
        }
        // ********************************************************************
        #endregion
        // ********************************************************************



        // ********************************************************************
        #region Private Functions
        // ********************************************************************
        private static void Initialise()
        {
            if (!s_initialised)
            {
                s_originalTimeScale = Time.timeScale;
                s_initialised = true;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************
    }
    // ************************************************************************
}
