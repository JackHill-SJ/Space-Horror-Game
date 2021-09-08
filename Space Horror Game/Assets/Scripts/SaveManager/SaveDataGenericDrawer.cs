using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(SaveDataGeneric<>))]
public class SaveDataGenericDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        if (EditorApplication.isPlaying && Time.realtimeSinceStartup > 1f)
        {
            Rect name = new Rect(position.x, position.y, position.width * .45f, position.height);
            EditorGUI.PropertyField(name, property.FindPropertyRelative("name"), GUIContent.none);
            Rect value = new Rect(position.x + position.width * .55f, position.y, position.width * .45f, position.height);
            EditorGUI.PropertyField(value, property.FindPropertyRelative("DirectValue"), GUIContent.none);
        }
        else
        {
            Rect name = new Rect(position.x, position.y, position.width, position.height);
            EditorGUI.TextField(name, "Can only view SaveDataGeneric values in play.");
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
#else
//This is active when not in editor, don't know if it needs to be, but just in case
public class SaveDataGenericDrawer : MonoBehaviour
{

}
#endif