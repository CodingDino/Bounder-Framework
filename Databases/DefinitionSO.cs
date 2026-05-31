// ************************************************************************ 
// File Name:   DefinitionSO.cs 
// Purpose:    	Scriptable object definition class for use in databases
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
    public class DefinitionSO : ScriptableObject, IDefinition
    // ********************************************************************
    {
        // ****************************************************************
        #region Editor Data
        // ****************************************************************
        public int ID
        {
            get => _ID;
            private set => _ID = value;
        }
        [SerializeField]
        [Tooltip("Auto-generated. Don't mess with this unless you know what you're doing!")]
        private int _ID = -1;

        // ****************************************************************
        #endregion
        // ****************************************************************

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (_ID < 0)
                {
                    string[] guids = AssetDatabase.FindAssets("t:Object");

                    _ID = 1;

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        DefinitionSO asset = AssetDatabase.LoadAssetAtPath(path, this.GetType()) as DefinitionSO;

                        if (asset != null && asset != this)
                        {
                            _ID = Mathf.Max(_ID, asset._ID + 1);
                        }
                    }

                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssets();

                    UnityEngine.Debug.Log($"Assigned ItemID {_ID} to {name}");
                }
            }
        }
#endif
    }
    // ********************************************************************
}
