using UnityEngine;
using UnityEditor;

public class RenameChildrenByMesh : MonoBehaviour
{
    [MenuItem("Tools/Rename Children To Mesh Names")]
    static void RenameSelectedParentChildren()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        GameObject parent = Selection.activeGameObject;

        int renamedCount = 0;

        foreach (Transform child in parent.transform)
        {
            Mesh mesh = null;

            MeshFilter mf = child.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
                mesh = mf.sharedMesh;

            if (mesh == null)
            {
                SkinnedMeshRenderer smr = child.GetComponent<SkinnedMeshRenderer>();
                if (smr != null && smr.sharedMesh != null)
                    mesh = smr.sharedMesh;
            }

            if (mesh != null)
            {
                Undo.RecordObject(child.gameObject, "Rename GameObject");
                child.name = mesh.name;
                renamedCount++;
            }
        }

        Debug.Log($"Renamed {renamedCount} child object(s) to their mesh names.");
    }
}