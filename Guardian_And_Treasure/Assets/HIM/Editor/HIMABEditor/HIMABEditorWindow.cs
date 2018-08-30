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
    public HIMSoResource src;
    public List<FolderInfo> folderCollection = new List<FolderInfo>();
    List<FileInfo> fileCollection = new List<FileInfo>();
    List<PkgInfo> pkgCollection = new List<PkgInfo>();
    public int srcOnChoose = 0;
    public DirectoryInfo AssetPath;
    public string ImportFolder = "";
    public bool viewDetail = false;
    public UnityEngine.Object[] SelectedObject;

    private BuildTarget buildTarget = BuildTarget.Android;

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
        src = HIMEditorUtility.LoadAsset<HIMSoResource>("Assets/Resources/HIMSoResource.asset");
        buildTarget = HIMAssetBundleOption.Current;

        ImportFolder = HIMEditorUtility.AssetPath + src.ABResources;
        string[] paths = ImportFolder.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < paths.Length; i++)
        {
            FolderInfo folder = new FolderInfo();
            folder.root = Application.dataPath;

            folder.name = paths[i].Remove(0, Application.dataPath.Length);
            folderCollection.Add(folder);
        }
    }

    private void OnGUI()
    {
        if (src == null)
        {
            EditorGUILayout.LabelField("请完成【基本设置】");
            return;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("目标目录：", GUILayout.Width(80));
        GUI.color = Color.green;
        EditorGUILayout.LabelField(ImportFolder);
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        bool searchBegin = GUILayout.Button("开始搜索");
        if (searchBegin)
        {
            fileCollection.Clear();
            this.Search();
            this.Filter();
        }
        if (fileCollection.Count > 0)
        {
            this.ViewFileDetail();
            this.StartBuildAssetBundle();
        }
    }
    private void Search()
    {
        DirectoryInfo di = new DirectoryInfo(ImportFolder);
        if(di.Exists)
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
            fileCollection.Add(fis[i]);
        }
        for (int i = 0; i < dis.Length; i++)
        {
            DirectoryInfo di = dis[i];
            this.SearchProcess(di);
        }
    }
    void Filter()
    {
        for (int i = 0; i < fileCollection.Count;)
        {
            FileInfo fi = fileCollection[i];
            bool remove = false;
            for (int j = 0; j < filterExName.Count; j++)
            {
                if (fi.Extension.Contains(filterExName[j]))
                {
                    remove = true;
                    break;
                }
            }
            if (remove)
            {
                fileCollection.RemoveAt(i);
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
        EditorGUILayout.LabelField(string.Format("一共 [{0}] 个文件...", fileCollection.Count), GUILayout.Width(120));
        viewDetail = EditorGUILayout.Toggle("展开细节", viewDetail);
        EditorGUILayout.EndHorizontal();
        if (viewDetail)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();
            for (int i = 0; i < fileCollection.Count; i++)
            {
                FileInfo fi = fileCollection[i];

                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(fi.DirectoryName, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(fi.Name, GUILayout.ExpandWidth(true));
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
        EditorGUILayout.TextField(HIMAssetBundleOption.Current.ToString(), GUILayout.Width(120));
        EditorGUI.EndDisabledGroup();
        bool beginPackage = GUILayout.Button("开始打包");
        GUILayout.EndHorizontal();

        if (beginPackage)
        {
            pkgCollection.Clear();
            for (int i = 0; i < fileCollection.Count; i++)
            {
                PkgInfo pkg = new PkgInfo();
                FileInfo fi = fileCollection[i];
                pkg.assetName = fi.FullName.Remove(0, HIMEditorUtility.ProjectPath.Length);
                pkg.assetName = pkg.assetName.Replace(@"\", "/");
                string abResources = HIMEditorUtility.AssetPath + src.ABResources+"/";
                string saveName = fi.FullName.Remove(0, abResources.Length);
                saveName = saveName.Replace(@"\","_");
                pkg.outputName = saveName;
                pkgCollection.Add(pkg);
            }
            string output = HIMEditorUtility.ProjectPath + src.ABResources+"/" + buildTarget.ToString() + "/";
            AssetBundleBuild[] builds = new AssetBundleBuild[pkgCollection.Count];
            for (int i = 0; i < pkgCollection.Count; i++)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                string[] assetNames = new string[]
                {
                    pkgCollection[i].assetName,
                };
                //工程中的文件路径
                build.assetNames = assetNames;
                //会产生文件路径
                build.assetBundleName = pkgCollection[i].outputName;
                builds[i] = build;
            }
            
            if (!Directory.Exists(output)) { Directory.CreateDirectory(output); }
            BuildPipeline.BuildAssetBundles(output, builds, BuildAssetBundleOptions.None, HIMAssetBundleOption.Current);
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

public class PkgInfo
{
    public string assetName;
    public string outputName;
    public string extension;
}