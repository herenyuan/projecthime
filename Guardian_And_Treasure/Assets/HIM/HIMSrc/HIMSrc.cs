using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HIM 资源管理
/// </summary>
public class HIMResources : SingleMono<HIMResources>
{
    public Action<string> onErrorCallBack;
    public AssetBundle MainBundle;
    private AssetBundleManifest Manifest { get; set; }
    public void Online()
    {
        MainBundle = AssetBundle.LoadFromFile(HIMPath.total + "StandaloneWindows");
        if (MainBundle == null)
        {
            if (onErrorCallBack != null)
            {
                onErrorCallBack.Invoke("MainBundle load error");
            }
        }
        Manifest = MainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        if (Manifest == null)
        {
            if (onErrorCallBack != null)
            {
                onErrorCallBack.Invoke("Manifest load error");
            }
        }
        string[] abnames = Manifest.GetAllAssetBundles();
        string[] abDependence = Manifest.GetAllAssetBundles();
        int a = 0;
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
    public T LoadSO<T>(string _Path, string _Name) where T : ScriptableObject
    {
        string relativeName = _Path + _Name;
        return Resources.Load<T>(relativeName);
    }
}

