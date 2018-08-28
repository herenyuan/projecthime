using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AB -----> 打包窗口
/// 资源目录整体打包
/// 采用资源分类式打包，包间存在依赖关系
/// 目标文件夹的子目录第一层为包名，之后为资源内容
/// 包名.资源内容
/// </summary>
public class HIMABEditorWindow : EditorWindow
{
    private string waterMark = "请填写目标资源路径";
    public UnityEngine.Object[] selected;
    public string pathAssets = "";
    public List<FolderInfo> folderCollection = new List<FolderInfo>();
    List<FileInfo> fileCollection = new List<FileInfo>();
    List<PkgInfo> pkgCollection = new List<PkgInfo>();
    public int srcOnChoose = 0;
    public DirectoryInfo AssetPath;
    public string ImportFolder = "";
    public bool viewDetail = false;
    public UnityEngine.Object[] SelectedObject;

    private BuildTarget buildTarget = BuildTarget.Android;

    public List<System.Type> SelectType = new List<System.Type>()
    {
        typeof(UnityEditor.DefaultAsset),
    };
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
        ".BAT"
    };

    Vector2 scrollPosition = Vector2.zero;
    public void Initialization()
    {
        pathAssets = Application.dataPath;
        ImportFolder = PlayerPrefs.GetString("ImportFolder");
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
        SelectedObject = Selection.objects;
        for (int i = 0; i < SelectedObject.Length; i++)
        {
            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(SelectedObject[i]));
        }

        EditorGUILayout.BeginHorizontal();

        bool refreshFolder = GUILayout.Button("刷新资源目录", GUILayout.ExpandWidth(true));
        if (refreshFolder)
        {
            folderCollection.Clear();
            //刷新工程文件夹
            AssetPath = new DirectoryInfo(Application.dataPath);
            DirectoryInfo[] dis = AssetPath.GetDirectories();
            for (int i = 0; i < dis.Length; i++)
            {
                FolderInfo folder = new FolderInfo();
                folder.root = Application.dataPath;
                folder.name = dis[i].FullName.Remove(0, Application.dataPath.Length);
                folder.name = folder.name.Replace(@"\", "/");
                folderCollection.Add(folder);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        for (int i = 0; i < folderCollection.Count;)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            EditorGUILayout.LabelField(string.Format("目录[{0}]", i), GUILayout.Width(50));
            EditorGUILayout.LabelField(folderCollection[i].root, GUILayout.ExpandWidth(true));
            GUI.color = Color.green;
            EditorGUILayout.LabelField(folderCollection[i].name, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            bool remove = GUILayout.Button("移除", GUILayout.Width(250));
            if (remove)
            {
                folderCollection.RemoveAt(i);
            }
            else
            {
                i++;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        if (folderCollection.Count > 0)
        {
            bool searchBegin = GUILayout.Button("开始搜索");
            if (searchBegin)
            {
                fileCollection.Clear();
                this.Search();
                this.Filter();
                this.SaveFolder();
            }
        }
        if (fileCollection.Count > 0)
        {
            this.ViewFileDetail();
            this.StartBuildAssetBundle();
        }
    }
    private void Search()
    {
        for (int i = 0; i < folderCollection.Count; i++)
        {
            FolderInfo folder = folderCollection[i];
            if (Directory.Exists(folder.FullName))
            {
                DirectoryInfo di = new DirectoryInfo(folder.FullName);
                this.SearchProcess(di);
            }
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
    void SaveFolder()
    {
        ImportFolder = "";
        for (int i = 0; i < folderCollection.Count; i++)
        {
            ImportFolder += folderCollection[i].FullName + ";";
        }
        if (!string.IsNullOrEmpty(ImportFolder))
        {
            PlayerPrefs.SetString("ImportFolder", ImportFolder);
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
        buildTarget = (BuildTarget)EditorGUILayout.EnumPopup(buildTarget, GUILayout.Width(200));
        bool beginPackage = GUILayout.Button("开始打包");
        GUILayout.EndHorizontal();

        if (beginPackage)
        {
            pkgCollection.Clear();
            for (int i = 0; i < fileCollection.Count; i++)
            {
                PkgInfo pkg = new PkgInfo();
                FileInfo fi = fileCollection[i];
                pkg.assetName = "Assets" + fi.FullName.Remove(0, Application.dataPath.Length);
                pkg.assetName = pkg.assetName.Replace(@"\", "/");
                string ext = Path.GetExtension(pkg.assetName);
                pkg.outputName = pkg.assetName.Replace(ext, "");
                Debug.Log(pkg.outputName);
                pkgCollection.Add(pkg);
            }
            string output = Application.dataPath + "/StreamingAssets/Android/";
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
                build.assetBundleName = pkgCollection[i].assetName;
                builds[i] = build;
            }
            if (!Directory.Exists(output)) { Directory.CreateDirectory(output); }
            BuildPipeline.BuildAssetBundles(output, builds, BuildAssetBundleOptions.None, buildTarget);
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