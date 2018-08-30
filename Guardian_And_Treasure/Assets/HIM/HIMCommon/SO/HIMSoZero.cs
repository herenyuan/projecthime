using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0号配置
/// 游戏最基本的配置
/// 默认存在Resources/SO/HIMSOZero.asset
/// </summary>
public class HIMSoZero : ScriptableObject {

    public string Date = "";
    public string Version = "1.0.0.17";
    /// <summary>
    /// 发布后AB的存放文件夹名字
    /// </summary>
    public string ExportABFolder = "StreammingAssets";
    public List<string> FolderName = new List<string>();
}
