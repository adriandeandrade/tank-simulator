using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ArmorRegion))]
public class ArmorRegionEditor : Editor
{
    private ArmorRegion armorRegion;

    private void OnEnable()
    {
        armorRegion = (ArmorRegion)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Armor Plate Mesh"))
        {
            Transform newMeshParent = armorRegion.transform;
            GameObject newMeshObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            newMeshObject.transform.SetParent(newMeshParent);

            newMeshObject.AddComponent<MeshStudy>();
        }

        //     mesh.objectReferenceValue = EditorGUILayout.ObjectField("Armor Mesh", newMesh, typeof(Mesh), false);
        //     meshCollider.objectReferenceValue = EditorGUILayout.ObjectField("Armor Mesh", collider, typeof(MeshCollider), false);
        //     EditorGUILayout.PropertyField(pointsList, true);

        //     if (GUILayout.Button("Generate Armor Mesh"))
        //     {
        //         GenerateArmorMesh();
        //     }

        //     serializedObject.ApplyModifiedProperties();
    }

    private void GenerateArmorMesh()
    {

    }
}
