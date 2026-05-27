// ************************************************************************ 
// File Name:   Definition.cs 
// Purpose:    	Base definition class for use in databases
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2026 Bounder Games
// ************************************************************************
namespace Bounder.Framework
{
    // ********************************************************************
    #region Imports
    // ********************************************************************
    using UnityEngine;
    using NaughtyAttributes;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    // ********************************************************************
    #endregion
    // ********************************************************************


    // ********************************************************************
    public class Definition : ScriptableObject
    // ********************************************************************
    {
        // ****************************************************************
        #region Editor Data
        // ****************************************************************
        [field: SerializeField]
        [field: Tooltip("Auto-generated. Don't mess with this unless you know what you're doing!")]
        public int ID { get; private set; } = -1;
        // ****************************************************************
        #endregion
        // ****************************************************************

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (ID < 0)
                {
                    string[] guids = AssetDatabase.FindAssets("t:Object");

                    ID = 1;

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        Definition asset = AssetDatabase.LoadAssetAtPath(path, this.GetType()) as Definition;

                        if (asset != null && asset != this)
                        {
                            ID = Mathf.Max(ID, asset.ID+1);
                        }
                    }

                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssets();

                    UnityEngine.Debug.Log($"Assigned ItemID {ID} to {name}");
                }
            }
        }
#endif
    }
    // ********************************************************************
}
