using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReviewGames
{
    [CustomEditor(typeof(AddInvertedMeshCollider))]
    public class AddInvertedMeshColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            AddInvertedMeshCollider script = (AddInvertedMeshCollider)target;
            if (GUILayout.Button("Create Inverted Mesh Collider"))
            {
                script.CreateInvertedMeshCollider();
            }
        }
    }
}
