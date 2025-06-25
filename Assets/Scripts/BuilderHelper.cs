using UnityEngine;
using UnityEditor;
using System.Linq;

public class BuilderHelper : MonoBehaviour
{
    [MenuItem("Tools/Builder Helper/Sort Selected GameObject Children Alphabetically")]
    static void SortSelected()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            var children = gameObject.transform.Cast<Transform>().ToList();
            var sortedChildren = children.OrderBy(t => t.name).ToList();

            for (int i = 0; i < sortedChildren.Count; i++)
            {
                sortedChildren[i].SetSiblingIndex(i);
            }

            Debug.Log($"Sorted {children.Count} children under {gameObject.name}");
        }
    }


    [MenuItem("Tools/Builder Helper/Rename Selected GameObject + Children To Mesh Names")]
    static void RenameSelectedGameAndChildrenToMeshNames()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected.");
            return;
        }

        int renamedCount = 0;

        foreach (GameObject selectedObject in Selection.gameObjects)
        {
            renamedCount += RenameRecursively(selectedObject);
        }

        Debug.Log($"Renamed {renamedCount} GameObject(s) to their mesh names.");
    }

    static int RenameRecursively(GameObject obj)
    {
        int renamed = 0;

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

        Mesh mesh = null;
        if (meshFilter != null)
        {
            mesh = meshFilter.sharedMesh;
        }
        else if (skinnedMeshRenderer != null)
        {
            mesh = skinnedMeshRenderer.sharedMesh;
        }

        if (mesh != null)
        {
            Undo.RecordObject(obj, "Rename GameObject");
            obj.name = mesh.name;
            renamed++;
        }

        foreach (Transform child in obj.transform)
        {
            renamed += RenameRecursively(child.gameObject);
        }

        return renamed;
    }


    [MenuItem("Tools/Builder Helper/Check Selected GameObject Children Pos, Rot, Scl")]
    static void CheckChildrenPositionRotationScale()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No GameObject(s) selected.");
            return;
        }

        int matchedCount = 0;
        int mismatchedCount = 0;

        foreach (GameObject parent in Selection.gameObjects)
        {
            foreach (Transform child in parent.transform)
            {
                bool positionOk = child.localPosition == Vector3.zero;
                bool rotationOk = child.localRotation == Quaternion.identity;
                bool scaleOk = child.localScale == Vector3.one;

                if (positionOk && rotationOk && scaleOk)
                {
                    matchedCount++;
                }
                else
                {
                    mismatchedCount++;
                    Debug.Log(
                        $"Mismatch in child '{child.name}' of parent '{parent.name}': " +
                        $"Position {(positionOk ? "OK" : child.localPosition.ToString())}, " +
                        $"Rotation {(rotationOk ? "OK" : child.localRotation.eulerAngles.ToString())}, " +
                        $"Scale {(scaleOk ? "OK" : child.localScale.ToString())}",
                        child.gameObject
                    );
                }
            }
        }

        int selectedCount = Selection.gameObjects.Length;
        Debug.Log($"[BuilderHelper] Number of selected GameObjects: {selectedCount}");
        Debug.Log($"[BuilderHelper] {matchedCount} matched, {mismatchedCount} mismatched children.");
    }


    [MenuItem("Tools/Builder Helper/Check Selected GameObjects Collider Rules")]
    static void CheckSelectedGameObjectsColliderRules()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No GameObject(s) selected.");
            return;
        }

        int matchCount = 0;
        int mismatchCount = 0;

        foreach (GameObject parent in Selection.gameObjects)
        {
            Collider parentCollider = parent.GetComponent<Collider>();
            if (parentCollider != null)
            {
                mismatchCount++;
                Debug.Log($"Mismatch: Parent '{parent.name}' SHOULD NOT have a collider but has '{parentCollider.GetType().Name}'", parent);
            }
            else
            {
                matchCount++;
            }

            var children = parent.transform.Cast<Transform>().ToList();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                Collider childCollider = child.GetComponent<Collider>();

                if (i == 0)
                {
                    if (childCollider == null)
                    {
                        mismatchCount++;
                        Debug.Log($"Mismatch: First child '{child.name}' SHOULD have a collider but does NOT.", child.gameObject);
                    }
                    else
                    {
                        matchCount++;
                    }
                }
                else
                {
                    if (childCollider != null)
                    {
                        mismatchCount++;
                        Debug.Log($"Mismatch: Child '{child.name}' SHOULD NOT have a collider but has '{childCollider.GetType().Name}'.", child.gameObject);
                    }
                    else
                    {
                        matchCount++;
                    }
                }

                foreach (Transform descendant in child.GetComponentsInChildren<Transform>(true))
                {
                    if (descendant == child) continue;

                    Collider descCollider = descendant.GetComponent<Collider>();
                    if (descCollider != null)
                    {
                        mismatchCount++;
                        Debug.Log($"Mismatch: Descendant '{descendant.name}' under '{child.name}' SHOULD NOT have any collider but has '{descCollider.GetType().Name}'.", descendant.gameObject);
                    }
                    else
                    {
                        matchCount++;
                    }
                }
            }
        }

        int selectedCount = Selection.gameObjects.Length;
        Debug.Log($"[BuilderHelper] Number of selected GameObjects: {selectedCount}");
        Debug.Log($"[BuilderHelper] {matchCount} matched, {mismatchCount} mismatched.");
    }


    [MenuItem("Tools/Builder Helper/Print Selected GameObject Mesh Length")]
    static void PrintSelectedMeshLength()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        GameObject selected = Selection.activeGameObject;

        MeshFilter meshFilter = selected.GetComponent<MeshFilter>();

        if (meshFilter == null)
        {
            Debug.LogWarning($"The selected GameObject '{selected.name}' does not have a MeshFilter component.");
            return;
        }

        if (meshFilter.sharedMesh == null)
        {
            Debug.LogWarning($"The MeshFilter on '{selected.name}' does not have a mesh assigned.");
            return;
        }

        Bounds meshBounds = meshFilter.sharedMesh.bounds;

        float meshLengthLocalX = meshBounds.size.x;
        float objectScaleX = selected.transform.lossyScale.x;
        float meshLengthWorldX = meshLengthLocalX * objectScaleX;

        Debug.Log($"[BuilderHelper] '{selected.name}' length (X axis): {meshLengthWorldX} units");
    }

}
