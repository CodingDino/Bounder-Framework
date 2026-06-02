// ****************************************************************************
// File Name:   SingletonSO.cs 
// Purpose:    	Singleton pattern, for scriptable objects
// Project:		Bounder Framework
// Author:      Sarah Herzog  
// Copyright: 	2019 Bounder Games
// ****************************************************************************
namespace Bounder.Framework
{
    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************ 
    #region Class: SingletonSO
    // ************************************************************************ 
    public class SingletonSO<T> : ScriptableObject where T : ScriptableObject
    {
        // ********************************************************************
        #region Static Data Members
        // ********************************************************************
        protected static T s_instance;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Properties
        // ********************************************************************
        public static T instance
        {
            get
            {
                if (s_instance != null && s_instance)
                {
                    return s_instance;
                }

                T[] results = Resources.LoadAll<T>("");

                if (results.Length == 0)
                {
                    Debug.LogError($"No {typeof(T).Name} found in Resources.");
                    return null;
                }

                if (results.Length > 1)
                {
                    Debug.LogError($"Multiple {typeof(T).Name} found in Resources.");
                }

                s_instance = results[0];
                return s_instance;
            }
        }
        // ********************************************************************
        public static bool initialized { get { return instance != null; } }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
    #endregion
    // ************************************************************************ 

}
