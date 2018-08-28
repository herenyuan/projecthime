using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HIM 工程的全局设置
/// </summary>
public class HIMSOProject : ScriptableObject
{
    public string Date = "";
    public string Version = "1.0.0.17";
    public string PathAssets = "Assets/";
    public string PathResources = "Assets/Resources/";
    public string PathABExport = "ABResources/";

    public List<HIMPath_> Path = new List<HIMPath_>()
    {
        new HIMPath_("SO","/SO/"),
        new HIMPath_("Prefab","/Prefab/"),
    };
}
[System.Serializable]
public class HIMPath_
{
    public string key;
    public string value;
    public HIMPath_(string _key, string _value)
    {
        key = _key;
        value = _value;
    }
}
public class HIMPath
{
    public static readonly string total = Application.dataPath + "/StreamingAssets/StandaloneWindows/";
    //暂定 预设 资源路径
    public static readonly string Prefab = Application.dataPath + "/StreamingAssets/StandaloneWindows/src/prefab/";
    public static readonly string SO = "SO/";
}