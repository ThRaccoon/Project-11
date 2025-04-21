using System.Runtime.CompilerServices;
using UnityEngine;

public static class NullChecker
{
    public static bool Check<T>(
        T obj,
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