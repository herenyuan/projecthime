using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 编辑器环境配置
/// </summary>
public class HIMEditorConfig : ScriptableObject {

    public string Date = "";
    public string Version = "1.0.0.17";
    public string ImportABFolder = "Resources";
    public string ExportABFolder = "StreammingAssets";
    public string ExportJsonFolder = "Config";
    public List<string> ExcelFolder = new List<string>();
    public List<string> FolderName = new List<string>()
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
}
