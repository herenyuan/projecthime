using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HIM 资源管理
/// 无法在 Editor 模式下使用
/// </summary>
public class HIMResources : SingleMono<HIMResources>
{
    public Action<string> onErrorCallBack;
    public HIMZeroConfig zero;
    private AssetBundleManifest Manifest { get; set; }

    public Dictionary<string, string> Entries = new Dictionary<string, string>();
    public Dictionary<string, AssetBundle> Bundles = new Dictionary<string, AssetBundle>();
    public Dictionary<string, string[]> BundleDependence = new Dictionary<string, string[]>();

    public string SrcPath = "";

    public void Online()
    {
        SrcPath = Application.streamingAssetsPath + @"\ABResources\";
        AssetBundle main = this.LoadFromFile("", "ABResources", ""); //加载AB信息，包含依赖和索引信息
        AssetBundle zero = this.LoadFromFile("", "Zero.asset", ""); //加载0号配置
        this.BuildDependence(main);
        this.BuildEntries(zero);
    }
    void BuildDependence(AssetBundle _ABResources)
    {
        AssetBundleManifest manifest = _ABResources.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] bundleNames = manifest.GetAllAssetBundles();//获取所有的包名

        for (int i = 0; i < bundleNames.Length; i++)
        {
            string bundleName = bundleNames[i];
            string[] dependencies = manifest.GetAllDependencies(bundleName);//获取所有对应包名的依赖
            if(dependencies.Length > 0)
            {
                BundleDependence.Add(bundleName, dependencies);
            }
        }
    }


    void BuildEntries(AssetBundle _Zero)
    {
        HIMZeroConfig config = _Zero.LoadAsset<HIMZeroConfig>("Assets/Resources/Zero.asset");

    }

    /// <summary>
    /// 加载 AB 进内存
    /// </summary>
    /// <param name="entry"> 库入口 </param>
    /// <param name="name"> 文件名字 </param>
    /// <param name="extension"> 扩展名 </param>
    /// <returns></returns>
    AssetBundle LoadFromFile(string entry, string name,string extension)
    {
        AssetBundle bundle = null;
        string bundleName = entry + name + extension;
        if (string.IsNullOrEmpty(SrcPath)) { return bundle; }
        string fullName = SrcPath + bundleName;
        if (!Bundles.ContainsKey(bundleName))
        {
            bundle = AssetBundle.LoadFromFile(fullName);
            Bundles.Add(bundleName, bundle);
        }
        return bundle;
    }

    public void LoadPrefab(string _Path, string _Name)
    {
        if (Manifest == null) { onErrorCallBack.Invoke("HIMResources is not online..."); return; }
        string extension = ".prefab";
        string fullName = _Path + _Name + extension;
        
        AssetBundle bundle = AssetBundle.LoadFromFile(fullName);
    
        string[] deps = Manifest.GetAllDependencies(_Name + extension);
        List<AssetBundle> bundleList = new List<AssetBundle>();
        for (int i = 0; i < deps.Length; ++i)
        {
            AssetBundle temp = AssetBundle.LoadFromFile(HIMPath.total + deps[i]);
            bundleList.Add(temp);
        }

        GameObject original = bundle.LoadAsset<GameObject>(_Name + extension);
        GameObject clone = GameObject.Instantiate(original);
    }
}

