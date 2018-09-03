﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// HIM 资源管理
/// 无法在 Editor 模式下使用
/// </summary>
public class HIMResources : SingleMono<HIMResources>
{
    public readonly string abResources = "ABResources";
    public readonly string zeroConfig = "Zero.asset";
    public readonly string manifestName = "AssetBundleManifest";
    public Action<string> onErrorCallBack;
    private AssetBundleManifest Manifest { get; set; }

    public Dictionary<string, string> Entries { get; set; }
    public Dictionary<string, AssetBundle> Bundles = new Dictionary<string, AssetBundle>();
    public Dictionary<string, string[]> BundleDependence = new Dictionary<string, string[]>();
    private AssetBundle mainBundle;
    private AssetBundle zeroBundle;

    public bool Ready
    {
        get
        {
            return mainBundle != null && zeroBundle != null;
        }
    }
    public void Online()
    {
        this.LoadMain();
        this.LoadZero();
        this.BuildDependence();
        this.BuildEntries();

        //this.LoadPrefab("ball");
        this.LoadPrefab("Prefab","ball");
        this.LoadPrefab("Prefab/GUI", "ball2");
        int a = 0;
    }
    void LoadMain()
    {
        //加载AB信息，包含依赖和索引信息
        mainBundle = this.GetBundle(HIMPath.Src, abResources);
    }
    void LoadZero()
    {
        //加载0号配置
        zeroBundle = this.GetBundle(HIMPath.Src, zeroConfig);
    }
    void BuildDependence()
    {
        if(mainBundle == null) { return; }
        Manifest = mainBundle.LoadAsset<AssetBundleManifest>(manifestName);
        string[] bundleNames = Manifest.GetAllAssetBundles();//获取所有的包名

        for (int i = 0; i < bundleNames.Length; i++)
        {
            string bundleName = bundleNames[i];
            string[] dependencies = Manifest.GetAllDependencies(bundleName);//获取所有对应包名的依赖
            if(dependencies.Length > 0)
            {
                BundleDependence.Add(bundleName, dependencies);
            }
        }
    }

    void BuildEntries()
    {
        if(zeroBundle == null) { return; }
        Debug.Log("------------------------- 创建资源库入口 --------------------------");
        if (Entries == null) { Entries = new Dictionary<string, string>(); }
        HIMZeroConfig config = zeroBundle.LoadAsset<HIMZeroConfig>("Zero");
        for (int i = 0; i < config.Entries.Count; i++)
        {
            string key = config.Entries[i];
            string value = config.Paths[i];
            Entries.Add(key, value);
        }
    }

    /// <summary>
    /// 获取bundle
    /// </summary>
    /// <param name="_Path"> 资源目录根 </param>
    /// <param name="_BundleName"> bundle 的相对目录 </param>
    /// <returns></returns>
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

    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="_Entry"></param>
    /// <param name="_Name"></param>
    /// <param name="_Extension"></param>
    void LoadBundle(string _Entry, string _Name, string _Extension)
    {
        string fileName = Path.Combine(_Name, _Extension);
        string bundleName = Path.Combine(_Entry, fileName);
    }
    void LoadDependence(AssetBundle bundle)
    {
        string bundleFullName = Path.Combine(HIMPath.Src, bundle.name);
        if (BundleDependence.ContainsKey(bundle.name))
        {
            string[] bundleNames = BundleDependence[bundle.name];
            for (int i = 0; i < bundleNames.Length; i++)
            {
                string bundleName = Path.Combine(HIMPath.Src, bundleNames[i]);
                Debug.Log(" load dependence---------------------------> " + bundleName);
                AssetBundle.LoadFromFile(bundleName);
            }
        }
    }

    public void LoadPrefab(string _Name)
    {
        if (!Ready) { return; }

        StringBuilder sb = new StringBuilder();
        sb.Append(_Name);
        sb.Append(".prefab");

        string bundleName = Path.Combine(Entries["Prefab"], sb.ToString());
        AssetBundle bundle = this.GetBundle(HIMPath.Src, bundleName);
        this.LoadDependence(bundle);
        //查看AB是否加载
        GameObject gameObject = bundle.LoadAsset<GameObject>(_Name);
        GameObject clone = GameObject.Instantiate(gameObject);
    }
    /// <summary>
    /// 加载相对路径的资源
    /// </summary>
    /// <param name="_Entry"></param>
    /// <param name="_Name"></param>
    public void LoadPrefab(string _Entry, string _Name)
    {
        if (!Ready) { return; }

        StringBuilder sb = new StringBuilder();
        sb.Append(_Name);
        sb.Append(".prefab");

        string bundleName = Path.Combine(_Entry, sb.ToString());
        AssetBundle bundle = this.GetBundle(HIMPath.Src, bundleName);
        this.LoadDependence(bundle);
        //查看AB是否加载
        GameObject gameObject = bundle.LoadAsset<GameObject>(_Name);
        GameObject clone = GameObject.Instantiate(gameObject);
    }
}

