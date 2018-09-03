using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AB -----> 打包窗口【暂不支持场景文件打包】
/// 资源目录整体打包
/// 采用资源分类式打包，包间存在依赖关系
/// 目标文件夹的子目录第一层为包名，之后为资源内容
/// 包名.资源内容
/// </summary>
public class HIMABEditorWindow : EditorWindow
{
    List<AssetInfo> assetCollection = new List<AssetInfo>();
    public string ImportFolder = "";
    public string ExportFolder = "";
    public bool viewDetail = false;
    public List<string> filterExName = new List<string>()
    {
        ".meta",
        ".cs",
        ".proto",
        ".txt",
        ".dll",
        ".ps1",
        ".xslt",
        ".pdb",
        ".exe",
        ".mdb",
        ".config",
        ".sh",
        ".scc",
        ".BAT",
        ".unity"
    };

    Vector2 scrollPosition = Vector2.zero;
    public void Initialization()
    {
        //ImportFolder = HIMEditorUtility.AssetPath + HIMEditorUtility.EdtConfig.ImportABFolder;
        ImportFolder = HIMEditorUtility.ImportPath;
        //ExportFolder = HIMEditorUtility.ProjectPath + HIMEditorUtility.EdtConfig.ExportABFolder + @"\" + HIMEditorUtility.BuildType.ToString() + @"\";
        ExportFolder = HIMEditorUtility.ExportPath;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("目标目录：", GUILayout.Width(60));
        EditorGUILayout.LabelField(HIMEditorUtility.AssetPath);
        EditorGUI.BeginDisabledGroup(true);
        GUI.color = Color.green;
        EditorGUILayout.TextArea(HIMEditorUtility.EdtConfig.ImportABFolder);
        GUI.color = Color.white;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        bool searchBegin = GUILayout.Button("开始搜索");
        if (searchBegin)
        {
            assetCollection.Clear();
            this.Search();
            this.Filter();
        }
        if (assetCollection.Count > 0)
        {
            this.ViewFileDetail();
            this.StartBuildAssetBundle();
        }
    }
    private void Search()
    {
        DirectoryInfo di = new DirectoryInfo(ImportFolder);
        if (di.Exists)
        {
            this.SearchProcess(di);
        }
    }
    void SearchProcess(DirectoryInfo root)
    {
        DirectoryInfo[] dis = root.GetDirectories();
        FileInfo[] fis = root.GetFiles();
        for (int i = 0; i < fis.Length; i++)
        {
            FileInfo file = fis[i];
            AssetInfo info = new AssetInfo();
            info.name = file.Name;
            info.rootPath = file.FullName.Remove(0, HIMEditorUtility.ProjectPath.Length);
            info.srcPath = file.FullName.Remove(0, HIMEditorUtility.ResPath.Length);
            info.extension = file.Extension;
            assetCollection.Add(info);
        }
        for (int i = 0; i < dis.Length; i++)
        {
            DirectoryInfo di = dis[i];
            this.SearchProcess(di);
        }
    }
    void Filter()
    {
        for (int i = 0; i < assetCollection.Count;)
        {
            AssetInfo info = assetCollection[i];
            bool remove = false;
            for (int j = 0; j < filterExName.Count; j++)
            {
                if (info.extension.Contains(filterExName[j]))
                {
                    remove = true;
                    break;
                }
            }
            if (remove)
            {
                assetCollection.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }
    void ViewFileDetail()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(string.Format("一共 [{0}] 个文件...", assetCollection.Count), GUILayout.Width(120));
        viewDetail = EditorGUILayout.Toggle("展开细节", viewDetail);
        EditorGUILayout.EndHorizontal();
        if (viewDetail)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();
            for (int i = 0; i < assetCollection.Count; i++)
            {
                AssetInfo info = assetCollection[i];

                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(info.rootPath, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(">>", GUILayout.Width(50));
                EditorGUILayout.LabelField(info.srcPath.Replace(@"\", "_"), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
    void StartBuildAssetBundle()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("目标平台：", GUILayout.Width(60));
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(HIMEditorUtility.BuildType.ToString(), GUILayout.Width(120));
        EditorGUI.EndDisabledGroup();
        GUI.color = Color.green;
        bool beginPackage = GUILayout.Button(string.Format("导出到 {0}", ExportFolder));
        GUI.color = Color.white;
        GUILayout.EndHorizontal();

      

        if (beginPackage)
        {

            AssetBundleBuild[] builds = new AssetBundleBuild[assetCollection.Count];
            for (int i = 0; i < assetCollection.Count; i++)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                AssetInfo info = assetCollection[i];
                string[] assetPaths = new string[]
                {
                    info.rootPath,
                };
                build.assetNames = assetPaths;
                build.assetBundleName = info.srcPath;
                builds[i] = build;
            }
            if (!Directory.Exists(ExportFolder)) { Directory.CreateDirectory(ExportFolder); }
            BuildPipeline.BuildAssetBundles(ExportFolder, builds, BuildAssetBundleOptions.UncompressedAssetBundle, HIMEditorUtility.BuildType);
            AssetDatabase.Refresh();
        }
    }
}

public class FolderInfo
{
    public string root;//Unity工程根目录
    public string relative;//Assets
    public string name;
    public string FullName
    {
        get { return root + name; }
    }

}

public class AssetInfo
{
    public string name;
    public string rootPath;
    public string srcPath;
    public string extension;
}
public class PkgInfo
{
    public string name;
    public string extension;
    public string md5;
    public long size;
}