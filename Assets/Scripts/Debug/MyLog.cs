using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class MyLog 
{
    public static void Log(string className, string text)
    {
        UnityEngine.Debug.Log("[" + className + "] " + "JOHN CENA. " + text);
    }
}
