using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 编辑器环境配置
/// </summary>
public class HIMEditorConfig : ScriptableObject {

    public string ZeroPathFormation = @"Assets\{0}\Zero.asset";
    public string ImportABFolder = "Resources";
    public string ExportJsonFolder = "Config";
    public string ExportABFolder = "ABResources";
    public List<string> ExcelFolder = new List<string>();
}
