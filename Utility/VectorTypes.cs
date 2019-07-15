// ************************************************************************ 
// File Name:   VectorTypes.cs 
// Purpose:    	Various types of vectors that are different from Unity's 
//				base vectors
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{

	// ******************************************************************** 
    #region Imports 
	// ******************************************************************** 
    using UnityEngine;
    #endregion
	// ******************************************************************** 


    // ******************************************************************** 
    #region Class: Vector2Bool
    // ******************************************************************** 
    [System.Serializable]
    public class Vector2Bool
    {
        // ****************************************************************
        #region Public Members
        // ****************************************************************
        public bool x = false;
        public bool y = false;
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Constructors
        // ****************************************************************
        public Vector2Bool(bool _x = false,
                            bool _y = false)
        {
            x = _x;
            y = _y;
        }
        // ****************************************************************
        public Vector2Bool(Vector3Bool _vec)
        {
            x = _vec.x;
            y = _vec.y;
        }
        // ****************************************************************
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Operator Overloads
        // ****************************************************************
        public bool this[int _key]
        {
            get
            {
                if (_key == 0)
                    return x;
                else if (_key == 1)
                    return y;
                else
                {
                    Debug.LogError("Vector2Bool get[] - no value for key " + _key);
                    return false;
                }
            }
            set
            {
                if (_key == 0)
                    x = value;
                else if (_key == 1)
                    y = value;
                else
                {
                    Debug.LogError("Vector2Bool set[] - no value for key " + _key);
                }
            }
        }
        // ****************************************************************
        #endregion
        // ****************************************************************
    }
    #endregion
    // ******************************************************************** 


    // ******************************************************************** 
    #region Class Vector3Bool
    // ******************************************************************** 
    [System.Serializable]
    public class Vector3Bool
    {
        // ****************************************************************
        #region Public Members
        // ****************************************************************
        public bool x = false;
        public bool y = false;
        public bool z = false;
        #endregion
        // ****************************************************************

        // ****************************************************************
        #region Constructors
        // ****************************************************************
        public Vector3Bool(bool _x = false,
                            bool _y = false,
                            bool _z = false)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        // ****************************************************************
        public Vector3Bool(Vector2Bool _vec)
        {
            x = _vec.x;
            y = _vec.y;
            z = false;
        }
        // ****************************************************************
        #endregion
        // ****************************************************************

        // ****************************************************************
        #region Operator Overloads
        // ****************************************************************
        public bool this[int _key]
        {
            get
            {
                if (_key == 0)
                    return x;
                else if (_key == 1)
                    return y;
                else if (_key == 2)
                    return z;
                else
                {
                    Debug.LogError("Vector3Bool get[] - no value for key " + _key);
                    return false;
                }
            }
            set
            {
                if (_key == 0)
                    x = value;
                else if (_key == 1)
                    y = value;
                else if (_key == 2)
                    z = value;
                else
                {
                    Debug.LogError("Vector3Bool set[] - no value for key " + _key);
                }
            }
        }
        // ****************************************************************
        #endregion
        // ****************************************************************
    }
    #endregion
    // ********************************************************************
}