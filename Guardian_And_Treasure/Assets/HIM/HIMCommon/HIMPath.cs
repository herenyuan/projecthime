using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HIMPath
{
#if UNITY_STANDALONE
    public static readonly string Src = Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID
    public static readonly string Src = Application.streamingAssetsPath + "/";
#elif UNITY_IOS
    public static readonly string Src = Application.persistentDataPath + "/";
#else
    public static readonly string Src = "";
#endif

    void Test()
    {
        
    }
}
