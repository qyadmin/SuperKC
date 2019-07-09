using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(StyleSetting))]
public class EditorStyleHide : Editor
{

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		EditorGUI.BeginDisabledGroup (true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty ("TextMark"));
		EditorGUI.EndDisabledGroup ();
		serializedObject.ApplyModifiedProperties();
	}

}