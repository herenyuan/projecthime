using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 资源管理
/// </summary>
public class HIMAssetBundle
{
    private AssetBundleManifest Manifest { get; set; }
    public Action<string> onErrorCallBack;

    public Dictionary<string, string> Entries = new Dictionary<string, string>();
    public Dictionary<string, AssetBundle> Bundles = new Dictionary<string, AssetBundle>();
    public Dictionary<string, string[]> BundleDependence = new Dictionary<string, string[]>();
    AssetBundle main;
    AssetBundle zero;

    public bool Ready
    {
        get
        {
            return main != null && zero != null;
        }
    }
    void Check()
    {
        main = this.GetBundle(HIMPath.Src,"ABResources");
        zero = this.GetBundle(HIMPath.Src, "Zero.asset");
        AssetBundleManifest manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] bundleNames = manifest.GetAllAssetBundles();//获取所有的包名

        Debug.Log("------------------------- 创建资源依赖 --------------------------");
        for (int i = 0; i < bundleNames.Length; i++)
        {
            string bundleName = bundleNames[i];
            string[] dependencies = manifest.GetAllDependencies(bundleName);//获取所有对应包名的依赖
            if (dependencies.Length > 0)
            {
                BundleDependence.Add(bundleName, dependencies);
            }
        }

        Debug.Log("------------------------- 创建资源库入口 --------------------------");
        HIMZeroConfig config = zero.LoadAsset<HIMZeroConfig>("Assets/Resources/Zero.asset");
        for (int i = 0; i < config.Entries.Count; i++)
        {
            string key = config.Entries[i];
            string value = key;
            Entries.Add(key, value);
        }
    }
    AssetBundle GetBundle(string _Path, string _BundleName)
    {
        string bundleFullName = Path.Combine(_Path, _BundleName);
        //完成 bundleName 的组装
        AssetBundle bundleOut = null;
        Debug.Log(" Get ----------------------------> " + bundleFullName);
        if (Bundles.ContainsKey(_BundleName))
        {
            bundleOut = Bundles[_BundleName];
        }
        else
        {
            try
            {
                //尚未加载
                bundleOut = AssetBundle.LoadFromFile(bundleFullName);
            }
            catch(Exception ex)
            {
                //资源不存在
                if (onErrorCallBack != null)
                {
                    onErrorCallBack.Invoke(string.Format("AssetBundle <color=#00ff00>[{0}]</color> is not exist in -> <color=#ff00ff>{1}</color>", _BundleName, bundleFullName));
                }
            }
        }
        return bundleOut;
    }
    public void LoadDependence(string bundleName)
    {
        Debug.Log(" load dependence---------------------------> " + bundleName);
        string bundleFullName = Path.Combine(HIMPath.Src, bundleName);
        AssetBundle.LoadFromFile(bundleFullName);
    }
}
