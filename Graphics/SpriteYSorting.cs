// ************************************************************************ 
// File Name:   SpriteYSorting.cs 
// Purpose:    	Sort sprites based on their Y position
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2021 Bounder Games
// ************************************************************************
namespace Bounder.Framework
{

    // ************************************************************************ 
    #region Imports
    // ************************************************************************
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using System;
    #endregion
    // ************************************************************************

    // ************************************************************************ 
    #region Class: SpriteYSorting
    // ************************************************************************ 
    public class SpriteYSorting : MonoBehaviour
    {
        // ********************************************************************
        #region Exposed Data Members 
        // ********************************************************************
        [Tooltip("Will be sorted relative to other objects with same ID")]
        [SerializeField]
        private string m_id = "";
        [Tooltip("Order in layer will be set to this number + index in sorted list")]
        [SerializeField]
        private int m_startingOrderInLayer = 0;
        [Tooltip("List of renderers sorted based on this object's y value")]
        [SerializeField]
        private List<Renderer> m_renderers = new List<Renderer>();
        [Tooltip("List of sorting groups to be sorted based on this object's y value")]
        [SerializeField]
        private List<SortingGroup> m_groups = new List<SortingGroup>();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Data Members 
        // ********************************************************************
        public SpriteYSorting m_lowerNeighbor = null;
        public SpriteYSorting m_upperNeighbor = null;
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Static Data Members 
        // ********************************************************************
        private static Dictionary<string, List<SpriteYSorting>> s_toBeSorted = new Dictionary<string, List<SpriteYSorting>>();
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Monobehaviour Methods 
        // ********************************************************************
        private void OnEnable()
        {
            if (!s_toBeSorted.ContainsKey(m_id))
            {
                s_toBeSorted.Add(m_id, new List<SpriteYSorting>());
            }
            
            s_toBeSorted[m_id].Add(this);
            SortList();
        }
        // ********************************************************************
        private void OnDisable()
        {
            s_toBeSorted[m_id].Remove(this);
            SortList(); // avoid references to items that don't exist in lower/upper neighbors
        }
        // ********************************************************************
        private void Update()
        {
            // Check lower item
            if (m_lowerNeighbor && m_lowerNeighbor.transform.position.y > transform.position.y)
                // If we are lower, resort
                SortList();
            // Check higher item
            if (m_upperNeighbor && m_upperNeighbor.transform.position.y < transform.position.y)
                // If we are higher, resort
                SortList();
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Methods 
        // ********************************************************************
        private void SortList()
        {
            //Debug.LogWarning("SortList()");
            List<SpriteYSorting> ourList = s_toBeSorted[m_id];

            // Sort list based on y world position of attached object
            ourList.Sort((a, b) => (a.transform.position.y.CompareTo(b.transform.position.y)));
            
            SpriteYSorting previousItem = null;
            for (int i = 0; i < ourList.Count; ++i)
            {
                SpriteYSorting thisItem = ourList[i];
                // Set sorting order in renderers / sorting groups for all items in list
                foreach (Renderer renderer in thisItem.m_renderers)
                {
                    renderer.sortingOrder = m_startingOrderInLayer - i;
                }
                foreach (SortingGroup group in thisItem.m_groups)
                {
                    group.sortingOrder = m_startingOrderInLayer - i;
                }

                // Set upper / lower neighbors for all items in list
                thisItem.m_lowerNeighbor = previousItem;
                thisItem.m_upperNeighbor = null; // there might not be another item
                // If there was an item before this, set this one as it's upper neighbor.
                // Last item will get left as null
                if (previousItem)
                    previousItem.m_upperNeighbor = thisItem;

                previousItem = thisItem;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************
    }
    #endregion
    // ************************************************************************
}
