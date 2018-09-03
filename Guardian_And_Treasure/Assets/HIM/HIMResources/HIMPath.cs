using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HIMPath
{
    public static readonly string total = Application.dataPath + "/StreamingAssets/StandaloneWindows/";
    //暂定 预设 资源路径
    public static readonly string Prefab = Application.dataPath + "/StreamingAssets/StandaloneWindows/src/prefab/";
    public static readonly string SO = "SO/";

#if UNITY_STANDALONE
    public static readonly string Src = Application.streamingAssetsPath + "/ABResources/";
#elif UNITY_ANDROID
    public static readonly string Src = Application.streamingAssetsPath + "/ABResources/";
#elif UNITY_IOS
    public static readonly string Src = Application.persistentDataPath + "/ABResources/";
#else
    public static readonly string Src = "";
#endif

    void Test()
    {
        
    }
}
