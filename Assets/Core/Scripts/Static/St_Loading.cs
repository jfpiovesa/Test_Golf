using System;
using UnityEngine;


public static class St_Loading
{
    public static Action<string> LoadIngSce {  get; private set; } 

    public static void Loading( string value)
    {
        LoadIngSce?.Invoke(value);
    }
 
}
