using UnityEditor;
using UnityEngine;
using UnityEditor.UI;

namespace Wrapper
{
    [CustomEditor(typeof(LoginButton), true)]
    [CanEditMultipleObjects]
    public class LoginButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Custom Elements", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("field"), new GUIContent("Field", "The Input Field linked to this button"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loginGO"), new GUIContent("LoginScreen GameObject", "The Login Screen Game Object"), true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Default Button Elements", EditorStyles.boldLabel);

            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}
