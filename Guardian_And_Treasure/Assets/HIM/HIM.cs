using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIM
{
    public static string AssetBundleRoot = "HOA";
    public static string AssetBundlePlatform = "StandaloneWindows";
    /// <summary>
    /// {0} = 可读写目录名字
    /// {1} = 目标平台资源文件夹名字
    /// </summary>
    public static string AssetBundlePath = Application.persistentDataPath + "/{0}/{1}/src/";
    /// <summary>
    /// src 路径
    /// </summary>
    public static string AssetBundleSrc = "{0}/{1}/src/";
}
