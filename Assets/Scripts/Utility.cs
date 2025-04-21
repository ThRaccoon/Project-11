using UnityEngine;

public static class Utility 
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



    public static bool ObjectToggle(GameObject obj) // Activates object if its inactive and vice versa. Returns: state of the object as bool;
    {
        if (obj.activeInHierarchy)
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
}
