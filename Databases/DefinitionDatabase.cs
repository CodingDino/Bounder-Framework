// ************************************************************************ 
// File Name:   DefinitionDatabase.cs 
// Purpose:    	Database class to be used to hold definition files, can auto-grab from asset folder
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{
    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using NaughtyAttributes;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    #endregion
    // ************************************************************************


    // ************************************************************************
    public class DefinitionDatabase<T> : BaseDefinitionDatabase<T> where T : DefinitionSO
    // ************************************************************************
    {
        // ********************************************************************
        #region Private Methods
        // ********************************************************************
#if UNITY_EDITOR
        [Button]
        private void GetDefinitionsFromProject()
        {
            m_listData.Clear();

            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);

                if (asset != null)
                {
                    m_listData.Add(asset);
                }
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif

    }
    // ********************************************************************
    #endregion
    // ********************************************************************


}
// ************************************************************************