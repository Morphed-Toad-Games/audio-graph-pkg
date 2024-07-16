using Josephus.AudioGraph.Models;
using System;
using UnityEditor;
using UnityEngine;
 
[CustomPropertyDrawer(typeof(SerializableGuid))]
public class SerializableGuidPropertyDrawer : PropertyDrawer {
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Start property draw
        EditorGUI.BeginProperty(position, label, property);

        // Get property
        SerializedProperty serializedGuid = property.FindPropertyRelative("serializedGuid");

        var guidString = serializedGuid.stringValue;

        // Draw label
        position = EditorGUI.PrefixLabel(new Rect(position.x, position.y, position.width, position.height), GUIUtility.GetControlID(FocusType.Passive), label);

        // // Draw fields - passs GUIContent.none to each so they are drawn without labels
        Rect pos = new Rect(position.xMin, position.yMin, position.width, position.height);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.TextField(pos, guidString);
        EditorGUI.EndDisabledGroup();
    }
}