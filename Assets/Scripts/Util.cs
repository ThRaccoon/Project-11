using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Util
{
    public static GameObject FindSceneObjectByTag(string tag)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {

            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }

        return null;
    }

    // Activates object if its inactive and vice versa.
    // Returns the state of the object as bool.
    public static bool ObjectToggle(GameObject obj)
    {
        if (obj != null && obj.activeInHierarchy)
        {
            obj.SetActive(false);
            return false;

        }
        else
        {
            obj.SetActive(true);
            return true;
        }
    }


    private static readonly HashSet<string> _loggedLocations = new HashSet<string>();

    public static bool IsNotNull<T>
    (T obj,
    [CallerFilePath] string file = "",
    [CallerLineNumber] int line = 0)

    where T : class
    {
        if (obj == null)
        {
            Debug.LogWarning($"[NullChecker] Null reference in {System.IO.Path.GetFileName(file)} at line {line}");
            return false;
        }
        return true;
    }
}