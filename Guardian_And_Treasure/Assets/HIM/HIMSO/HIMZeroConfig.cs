using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zero 配置
/// 在 Resources 根目录中
/// (有待修改)
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
    public List<string> Paths = new List<string>()
    {
        "Config",
        "Material",
        "Prefab",
        "Shader",
        "Other",
        "Sound",
        "Sprite",
        "Texture",
        "Txt/Json",
    };
    public void Add(string _Entry, string _Path)
    {
        this.Entries.Add(_Path);
        this.Paths.Add(_Path);
    }
}
