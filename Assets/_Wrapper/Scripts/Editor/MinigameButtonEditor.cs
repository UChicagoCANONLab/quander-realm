using UnityEditor;
using UnityEngine;
using UnityEditor.UI;

namespace Wrapper
{
    [CustomEditor(typeof(MinigameButton), true)]
    [CanEditMultipleObjects]
    public class MinigameButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Custom Elements", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minigame"), new GUIContent("Minigame", "Minigame to load"), true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Default Button Elements", EditorStyles.boldLabel);

            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}
