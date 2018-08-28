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

}

public class HIMSOPath : ScriptableObject
{
    public string SO = "SO/";
    public string PREFAB = "Prefab/";
    public string GUI = "Prefab/GUI/";
    public string STAGE = "Prefab/Stage/";
    public string CAMERA = "Prefab/Camera/";
    public string ENVIROMENT = "Prefab/Env/";
    public string EFFECT = "Prefab/Effect/";
    public string ITEM = "Prefab/Item/";
    public string SKILL = "Prefab/Skill/";
    public string ICON = "Src/icon/";
    public string SCENE = "Src/scene/";
    public string CHARACTER = "Prefab/Character/";//临时，路径有待规划
    public string CONFIG = "Config/";
    public string MAT = "Mat/";
    public List<string> Key = new List<string>();
    public List<string> Value = new List<string>();
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