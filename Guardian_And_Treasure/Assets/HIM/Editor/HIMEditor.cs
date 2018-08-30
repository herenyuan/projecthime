using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HIMEditor
{
    static EditorWindow Current;
    [MenuItem("游戏设计器/HIM配置")]
    public static void Tool0()
    {
        
        HIMConfigWindow win = EditorWindow.GetWindow<HIMConfigWindow>("工程设置");
        win.Initialization();
        Show(win);
    }

    [MenuItem("游戏设计器/导表工具")]
    public static void Tool1()
    {
        WINExcelToJson win = EditorWindow.GetWindow<WINExcelToJson>("导表工具");
        win.Initialization();
        Show(win);
    }
    [MenuItem("游戏设计器/资源打包工具")]
    public static void Tool2()
    {
        HIMABEditorWindow win = EditorWindow.GetWindow<HIMABEditorWindow>("资源打包工具");
        win.Initialization();
        Show(win);
    }
    static void Show(EditorWindow target)
    {
        if (Current != null && !Current.Equals(target))
        {
            Current.Close();
        }
        Current = target;
        Current.Show();
    }
}

public class HIMEditorUtility
{
    public static readonly string PathConfig = "Assets/HIM/Editor/HIMEditorConfig.asset";
    public static BuildTarget BuildType
    {
        get
        {
#if UNITY_ANDROID
            return BuildTarget.Android;
#elif UNITY_STANDALONE_WIN
            return BuildTarget.StandaloneWindows;
#elif UNITY_IOS
            return BuildTarget.iOS;
#endif
        }
    }
    /// <summary>
    /// 工程目录
    /// </summary>
    public static string ProjectPath = System.Environment.CurrentDirectory + @"\";
    public static string AssetPath = System.Environment.CurrentDirectory + @"\Assets\";
    public static string ResPath = System.Environment.CurrentDirectory + @"\Assets\Resources\";
    public static string ImportABFolder = "ABResources";
    public static T Create<T>(string _localPath, string assetName) where T : ScriptableObject
    {
        string fileName = string.Format(_localPath, assetName);
        string fullName = Path.Combine(ProjectPath, fileName);
        string assetSavePath = string.Format(_localPath, assetName);
        Object data = ScriptableObject.CreateInstance<T>();
        FileInfo fi = new FileInfo(fullName);
        if (!fi.Directory.Exists) { fi.Directory.Create(); }
        AssetDatabase.CreateAsset(data, assetSavePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return (T)data;
    }
    public static T Create<T>(string assetName) where T : ScriptableObject
    {
        string fullName = Path.Combine(ProjectPath, assetName);
        Object data = ScriptableObject.CreateInstance<T>();
        FileInfo fi = new FileInfo(fullName);
        if (!fi.Directory.Exists) { fi.Directory.Create(); }
        AssetDatabase.CreateAsset(data, assetName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return (T)data;
    }
    public static T LoadAsset<T>(string assetPath, string assetName) where T : ScriptableObject
    {
        string assetFullName = string.Format(assetPath, assetName);
        return AssetDatabase.LoadAssetAtPath<T>(assetFullName);
    }
    public static T LoadAsset<T>(string assetName) where T : ScriptableObject
    {
        return AssetDatabase.LoadAssetAtPath<T>(assetName);
    }


    public static void CheckPath(string mainFolder, List<string> subfolders)
    {
        string root = AssetPath + mainFolder;
        if (!Directory.Exists(root)) { Directory.CreateDirectory(root); }
        for (int i = 0; i < subfolders.Count; i++)
        {
            string sub = root + "/" + subfolders[i];
            if (!Directory.Exists(sub)) { Directory.CreateDirectory(sub); }
        }
    }
}
