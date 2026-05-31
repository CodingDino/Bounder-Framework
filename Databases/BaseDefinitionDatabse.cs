// ************************************************************************ 
// File Name:   BaseDefinitionDatabase.cs 
// Purpose:    	Database base class to be used to hold definition files
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
    using System.Collections.Generic;
    using UnityEngine;
    #endregion
    // ************************************************************************


    // ************************************************************************
    public class BaseDefinitionDatabase<T> : SingletonSO<BaseDefinitionDatabase<T>> where T : IDefinition
        // ************************************************************************
    {
        // ********************************************************************
        #region Exposed Data Members
        // ********************************************************************
        [SerializeField]
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
                return default;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Methods
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