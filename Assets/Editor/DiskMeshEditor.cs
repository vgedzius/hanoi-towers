using UnityEditor;
using UnityEngine;

namespace HanoiTowers
{
    [CustomEditor(typeof(DiskMesh))]
    public class DiskMeshEditor : Editor
    {
        SerializedProperty minRadius;
        SerializedProperty maxRadius;
        SerializedProperty innerRadius;
        SerializedProperty height;
        SerializedProperty numberOfSegments;

        bool showGeometry = true;
        bool showPreview = true;
        int previewSize = 5;
        int previewMaxDisks = 10;
        
        readonly GUIContent segmentsLabel = new GUIContent("Segments");

        void OnEnable()
        {
            minRadius = serializedObject.FindProperty("minRadius");
            maxRadius = serializedObject.FindProperty("maxRadius");
            innerRadius = serializedObject.FindProperty("innerRadius");
            height = serializedObject.FindProperty("height");
            numberOfSegments = serializedObject.FindProperty("numberOfSegments");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawGeometrySection();
            DrawPreviewSection();
            
            serializedObject.ApplyModifiedProperties();
        }

        void DrawPreviewSection()
        {
            DiskMesh diskMesh = ((DiskMesh) target);

            PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(diskMesh.gameObject);
            // TODO: @Investigate I thought this should work opposite way, am I using this properly?
            if (status == PrefabInstanceStatus.NotAPrefab) return;  
            
            showPreview = EditorGUILayout.Foldout(showPreview, "Preview");
            if (!showPreview) return;

            previewSize = EditorGUILayout.IntField("Size", previewSize);
            previewMaxDisks = EditorGUILayout.IntField("Max Disks", previewMaxDisks);

            if (GUILayout.Button("Preview"))
            {
                
                diskMesh.Build(previewSize, previewMaxDisks);
            }
        }

        void DrawGeometrySection()
        {
            showGeometry = EditorGUILayout.Foldout(showGeometry, "Geometry");
            
            if (!showGeometry) return;
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel("Outer Radius");
            EditorGUILayout.PropertyField(minRadius, GUIContent.none);
            EditorGUILayout.PropertyField(maxRadius, GUIContent.none);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(innerRadius);
            EditorGUILayout.PropertyField(height);
            EditorGUILayout.PropertyField(numberOfSegments, segmentsLabel);
        }
    }
}