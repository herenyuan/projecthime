using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zero 配置
/// 在 Resources 根目录中
/// </summary>
public class HIMZeroConfig : ScriptableObject {

    public string Date = "";
    public string Version = "1.0.0.17";
    public string FolderABResource = "default";

   
    public List<string> Entries = new List<string>()
    {
        "Config",
        "Material",
        "Prefab",
        "Shader",
        "Other",
        "Sound",
        "Sprite",
        "Texture",
        "Txt",
    };
}
