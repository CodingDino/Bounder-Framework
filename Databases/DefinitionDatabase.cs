// ************************************************************************ 
// File Name:   DefinitionDatabase.cs 
// Purpose:    	Database base class to be used to hold definition files
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2016 Bounder Games
// ************************************************************************ 
namespace Bounder.Framework
{
    // ************************************************************************ 
    #region Imports
    using NaughtyAttributes;
    using System.Collections.Generic;
    using UnityEditor;
    // ************************************************************************
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************
    public class DefinitionDatabase<T> : SingletonSO<DefinitionDatabase<T>> where T : Definition
    // ************************************************************************
    {
        // ********************************************************************
        #region Exposed Data Members
        // ********************************************************************
        [SerializeField]
        [ReadOnly]
        protected List<T> m_listData = new();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Internal Data Members
        // ********************************************************************
        protected Dictionary<int, T> m_data = new();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Monobehavior Methods
        // ********************************************************************
        protected virtual void OnEnable()
        {
            PopulateDictionary();
        }
        // ********************************************************************
        #endregion
        // ********************************************************************



        // ********************************************************************
        #region Public Methods
        // ********************************************************************
        public static bool HasData(int _id)
        {
            return instance.m_data.ContainsKey(_id);
        }
        // ********************************************************************
        public static T GetData(int _id)
        {
            if (instance.m_data.ContainsKey(_id))
            {
                return instance.m_data[_id];
            }
            else
            {
                Debug.LogError("Database.GetData(" + _id + "): Database does not contain key.");
                return default(T);
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Methods
        // ********************************************************************
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
        // ********************************************************************
        private void PopulateDictionary()
        {
            Debug.Log($"Populating dictionary...");
            m_data.Clear();
            for (int i = 0; i < m_listData.Count; ++i)
            {
                Debug.Log($"Adding item {m_listData[i]} to the databse dictionary");
                Debug.Log($"Adding item {m_listData[i].ID} to the databse dictionary");
                m_data[m_listData[i].ID] = m_listData[i];
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
    // ************************************************************************

}
// ************************************************************************