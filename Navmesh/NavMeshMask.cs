#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Bounder.Framework
{
    [CustomPropertyDrawer(typeof(NavMeshMaskAttribute))]
    public class NavMeshMaskDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var width = position.width;
            position.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(position, label);

            var areaNames = GameObjectUtility.GetNavMeshAreaNames();
            var mask = serializedProperty.intValue;
            position.x += EditorGUIUtility.labelWidth;
            position.width = width - EditorGUIUtility.labelWidth;

            EditorGUI.BeginChangeCheck();
            mask = EditorGUI.MaskField(position, mask, areaNames);
            if (EditorGUI.EndChangeCheck())
                serializedProperty.intValue = mask;
        }
    }

    public class NavMeshMaskAttribute : PropertyAttribute { }
}
#endif