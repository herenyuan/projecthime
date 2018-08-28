using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HIM 的全局设置
/// </summary>
public class HIMProject : ScriptableObject {

    public string AssetBundleRoot = "HOA";
    public string AssetBundlePlatform = "StandaloneWindows";
    /// <summary>
    /// src 路径
    /// </summary>
    public string AssetBundlePath = "{0}/{1}/src/";
}
