using System;
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
    public readonly string manifestName = "AssetBundleManifest";
    public Action<string> onMessageCallBack;
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
    public override void Online()
    {
        Manifest = mainBundle.LoadAsset<AssetBundleManifest>(manifestName);
        string[] bundleNames = Manifest.GetAllAssetBundles();//获取所有的包名

        for (int i = 0; i < bundleNames.Length; i++)
        {
            string bundleName = bundleNames[i];
            string[] dependencies = Manifest.GetAllDependencies(bundleName);//获取所有对应包名的依赖
            if (dependencies.Length > 0)
            {
                BundleDependence.Add(bundleName, dependencies);
            }
        }

        if (zeroBundle == null) { return; }
        this.Log("创建资源库入口");
        if (Entries == null) { Entries = new Dictionary<string, string>(); }
        HIMZeroConfig config = zeroBundle.LoadAsset<HIMZeroConfig>("Zero");
        for (int i = 0; i < config.Entries.Count; i++)
        {
            string key = config.Entries[i];
            string value = config.Paths[i];
            Entries.Add(key, value);
        }
    }
    public override void Offline()
    {
        
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
        this.Log(" Get -> " + bundleFullName);
        if (Bundles.ContainsKey(_BundleName))
        {
            bundleOut = Bundles[_BundleName];
        }
        else
        {
            //尚未加载
            bundleOut = AssetBundle.LoadFromFile(bundleFullName);
            if (bundleOut != null)
            {
                Bundles.Add(_BundleName, bundleOut);
            }
            else
            {
                if (onErrorCallBack != null)
                {
                    onErrorCallBack.Invoke(string.Format("AssetBundle <color=#00ff00>[{0}]</color> is not exist in -> <color=#ff00ff>{1}</color>", _BundleName, bundleFullName));
                }
            }
        }
        return bundleOut;
    }
    void LoadDependence(AssetBundle bundle)
    {
        if(bundle == null) { return; }
        string bundleFullName = Path.Combine(HIMPath.Src, bundle.name);
        if (BundleDependence.ContainsKey(bundle.name))
        {
            string[] bundleNames = BundleDependence[bundle.name];
            for (int i = 0; i < bundleNames.Length; i++)
            {
                this.Log(" load dependence -> " + bundleNames[i]);
                this.GetBundle(HIMPath.Src, bundleNames[i]);
            }
        }
    }

    public void Log(string msg)
    {
        if (onMessageCallBack != null)
        {
            onMessageCallBack.Invoke(msg);
        }
    }

    /// <summary>
    /// 加载AB库中的资源
    /// </summary>
    /// <param name="_Entry"></param>
    /// <param name="_Name"></param>
    public GameObject LoadPrefab(string _Entry, string _Name,string _Extension = ".prefab")
    {
        GameObject original = this.Load<GameObject>(_Entry, _Name, _Extension);
        GameObject clone = null;
        if (original!=null)
        {
            clone = GameObject.Instantiate(original);
        }
        return clone;
    }
    public TextAsset LoadText(string _Entry, string _Name, string _Extension)
    {
        // Json 不需要查找依赖关系
        TextAsset txt = this.Load<TextAsset>(_Entry, _Name, _Extension);
        return txt;
    }
    public Material LoadMaterial(string _Entry, string _Name, string _Extension = ".mat")
    {
        //查看AB是否加载
        Material mat = this.Load<Material>(_Entry, _Name, _Extension);
        return mat;
    }
    public Sprite LoadImage(string _Entry, string _Name, string _Extension = ".png")
    {
        // Sprite 不需要查找依赖关系
        Sprite sp = this.Load<Sprite>(_Entry, _Name, _Extension);
        return sp;
    }

    /// <summary>
    /// 加载 T 类型的数据到内存中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T Load<T>(string _Entry, string _Name, string _Extension) where T : UnityEngine.Object
    {
        string srcName = Path.Combine(_Entry, _Name);
        T ins = Resources.Load<T>(srcName);
        if (ins == null)
        {
            //Src 中不存在
            string bundleName = srcName + _Extension;
            AssetBundle bundle = this.GetBundle(HIMPath.Src, bundleName.ToLower());
            if (bundle != null)
            {
                this.LoadDependence(bundle);
                ins = bundle.LoadAsset<T>(_Name);
            }
        }
        if(ins == null)
        {
            if (onErrorCallBack != null)
            {
                onErrorCallBack.Invoke(string.Format("Src <color=#00ff00>[{0}]</color> is not exist.....", _Name));
            }
        }
        //完成 bundleName 的组装
        return ins;
    }
    public void UnLoad(UnityEngine.Object srcTarget)
    {
        Resources.UnloadAsset(srcTarget);//无法卸载 Public 绑定的资源
        Resources.UnloadUnusedAssets(); //没有引用才能正常卸载
    }
}

