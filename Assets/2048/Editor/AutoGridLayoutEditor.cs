using UnityEditor;
using _2048._Scripts;

namespace Assets.Editor
{
    [CustomEditor(typeof(AutoGridLayout),false)]
    [CanEditMultipleObjects]
    public class AutoGridLayoutGroupEditor : UnityEditor.Editor
    {
        SerializedProperty _mColumn;
        SerializedProperty _mRow;
        SerializedProperty _mPadding;
        SerializedProperty _mSpacing;
        SerializedProperty _mStartCorner;
        SerializedProperty _mStartAxis;
        SerializedProperty _mChildAlignment;

        public SerializedProperty MbIsColumn { get; set; }

        protected virtual void OnEnable()
        {
            MbIsColumn = serializedObject.FindProperty("_isColumn"); 
            _mColumn = serializedObject.FindProperty("_column");
            _mRow = serializedObject.FindProperty("_row");
            _mPadding = serializedObject.FindProperty("m_Padding");
            _mSpacing = serializedObject.FindProperty("m_Spacing");
            _mStartCorner = serializedObject.FindProperty("m_StartCorner");
            _mStartAxis = serializedObject.FindProperty("m_StartAxis");
            _mChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(MbIsColumn, true);
            if (!MbIsColumn.hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(MbIsColumn.boolValue ? _mColumn : _mRow, true);
            }
        
            EditorGUILayout.PropertyField(_mPadding, true);
            EditorGUILayout.PropertyField(_mSpacing, true);
            EditorGUILayout.PropertyField(_mStartCorner, true);
            EditorGUILayout.PropertyField(_mStartAxis, true);
            EditorGUILayout.PropertyField(_mChildAlignment, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}