using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源相关的 So 配置
/// </summary>
public class HIMSoResource : ScriptableObject
{
    [HideInInspector]
    public string ABResources = "ABResources";
    [HideInInspector]
    public List<string> key = new List<string>()
    {
        "Config",
        "Material",
        "Prefab",
        "Shader",
        "SO",
        "Sound",
        "Sprite",
        "Texture",
        "Txt",
    };
    [HideInInspector]
    public List<string> value = new List<string>()
    {
        "/Config/",
        "/Material/",
        "/Prefab/",
        "/Shader/",
        "/SO/",
        "/Sound/",
        "/Sprite/",
        "/Texture/",
        "/Txt/",
    };
    public void RemvoeAt(int index)
    {
        key.RemoveAt(index);
        value.RemoveAt(index);
    }
    public void AddNew()
    {
        key.Add("");
        value.Add("");
    }
}
