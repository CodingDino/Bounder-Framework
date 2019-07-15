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
                if (s_instance == null)
                    Debug.LogError("Attempt to access null singleton - did you forget to create an asset in the project?");
                return s_instance;
            }
        }
        // ********************************************************************
        public static bool initialized { get { return instance != null; } }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Unity Methods
        // ********************************************************************
        protected void Awake()
        {
            if (s_instance != null)
            {
                Debug.LogError("Second copy of singleton found. New copy: "+name+". Original: "+s_instance.name);
            }
            else
            {
                s_instance = this as T;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
    #endregion
    // ************************************************************************ 

}
