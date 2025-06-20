using UnityEngine;
using UnityEditor;

public class MeshLengthTool : EditorWindow
{
    [MenuItem("Tools/Mesh Length Tool/Print Selected Mesh Length")]
    public static void PrintSelectedMeshLength()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        GameObject selected = Selection.activeGameObject;
        MeshFilter meshFilter = selected.GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning("Selected GameObject does not have a MeshFilter with a mesh.");
            return;
        }

        Bounds bounds = meshFilter.sharedMesh.bounds;
        float length = bounds.size.x * selected.transform.lossyScale.x;

        Debug.Log($"[MeshLengthTool] '{selected.name}' length (X): {length} units");
    }
}
