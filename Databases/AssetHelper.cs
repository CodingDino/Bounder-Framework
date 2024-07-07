// ************************************************************************ 
// File Name:   AssetHelper.cs 
// Purpose:     Static class for finding and working with unity assets
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2024 Bounder Games
// ************************************************************************ 
#if UNITY_EDITOR
namespace Bounder.Framework
{

    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using System.Collections.Generic;
    using UnityEditor;
    #endregion
    // ************************************************************************


    // ************************************************************************
    // Class: AssetHelper 
    // ************************************************************************
    public class AssetHelper
    {

        // ********************************************************************
        #region Public Functions
        // ********************************************************************
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();

            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).ToString().Replace("UnityEngine.", "")));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
    // ************************************************************************
}
#endif