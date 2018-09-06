using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AssetBundle 管理
/// </summary>
public class HIMEditorAssetBundle
{
    string[] bundleNames;
    string[] dependencies;
    public void Search(Object _FolderObject)
    {
        List<BundleInfo> bundles = new List<BundleInfo>();
        string path = Path.Combine(HIMEditorUtility.AssetPath , AssetDatabase.GetAssetPath(_FolderObject));
        DirectoryInfo di = new DirectoryInfo(path);
        if (di.Exists)
        {
            FileInfo [] fis = di.GetFiles();
        }
        //获取当前的bundle设置
        bundleNames = AssetDatabase.GetAllAssetBundleNames();
        dependencies = AssetDatabase.GetAssetBundleDependencies("", false);//查询依赖，不递归查询
        bool result = AssetDatabase.RemoveAssetBundleName("", false);//移除，bundleName

    }
}
public class BundleInfo
{
    public List<string> Assets { get; set; }
    public string Entry { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
}