using UnityEngine;
using UnityEditor;
using System.Linq;

public class SortChildrenAlphabetically : MonoBehaviour
{
    [MenuItem("Tools/Sort Selected GameObject's Children Alphabetically")]
    static void SortSelected()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Transform parent = go.transform;

            // Get direct children
            var children = parent.Cast<Transform>().ToList();

            // Sort alphabetically by name
            var sortedChildren = children.OrderBy(t => t.name).ToList();

            // Reassign sibling indexes
            for (int i = 0; i < sortedChildren.Count; i++)
            {
                sortedChildren[i].SetSiblingIndex(i);
            }

            Debug.Log($"Sorted {children.Count} children under {go.name}");
        }
    }
}

