
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HIMConfigWindow : EditorWindow
{
    private HIMEditorConfig config;
    public void Initialization()
    {
        config = HIMEditorUtility.LoadAsset<HIMEditorConfig>(HIMEditorUtility.PathConfig);
        if (config != null && config.ExcelFolder.Count == 0)
        {
            config.ExcelFolder.Add("");
        }
    }
    public bool viewPath = true;
    private void OnGUI()
    {
        if (config == null)
        {
            bool createConfig = GUILayout.Button("开始HIM配置");
            if (createConfig)
            {
                config = HIMEditorUtility.Create<HIMEditorConfig>(HIMEditorUtility.PathConfig);
            }
        }
        else
        {
            EditorGUILayout.LabelField("[1]基本设置");
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("目标平台：", HIMEditorUtility.BuildType.ToString());
            config.Date = EditorGUILayout.TextField("当前日期：", config.Date);
            config.Version = EditorGUILayout.TextField("版本号：", config.Version);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("[2]资源库设置", GUILayout.Width(120));
            viewPath = EditorGUILayout.Toggle("展开详细", viewPath);
            EditorGUILayout.EndHorizontal();
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            if (viewPath)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(1000));
                EditorGUILayout.LabelField(HIMEditorUtility.AssetPath, GUILayout.ExpandWidth(true));
                config.ImportABFolder = EditorGUILayout.TextField(config.ImportABFolder, GUILayout.Width(120));
                bool check = GUILayout.Button("修复", GUILayout.Width(120));
                if (check)
                {
                    this.Fix(HIMEditorUtility.AssetPath + config.ImportABFolder, config.FolderName);
                    AssetDatabase.Refresh();
                }
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < config.FolderName.Count;)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(HIMEditorUtility.AssetPath + config.ImportABFolder + @"\");
                    config.FolderName[i] = EditorGUILayout.TextArea(config.FolderName[i], GUILayout.Width(120));
                    GUI.color = Color.red;
                    bool remove = GUILayout.Button("移除[-]", GUILayout.Width(60));
                    GUI.color = Color.white;
                    if (remove)
                    {
                        config.FolderName.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUI.color = Color.green;
                bool addNew = GUILayout.Button("[+]~新增路径");
                GUI.color = Color.white;
                EditorGUILayout.TextField(config.ExportABFolder);
                if (addNew)
                {
                    config.FolderName.Add("");
                }
            }
            else
            {
                EditorGUILayout.LabelField(string.Format("以配置 {0} 个资源路径", config.FolderName.Count), GUILayout.Width(120));
            }
            EditorGUILayout.Space();
            //导表设置
            EditorGUILayout.LabelField("[3]配置设置", GUILayout.Width(120));
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            for (int i = 0; i < config.ExcelFolder.Count;)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(string.Format("配置路径[{0}]：", i), GUILayout.Width(80));
                config.ExcelFolder[i] = EditorGUILayout.TextArea(config.ExcelFolder[i]);
                GUI.color = Color.red;
                bool remove = GUILayout.Button("[-]移除", GUILayout.Width(80));
                GUI.color = Color.white;
                if (remove)
                {
                    config.ExcelFolder.RemoveAt(i);
                }
                else
                {
                    i++;
                }
                EditorGUILayout.EndHorizontal();
            }
            GUI.color = Color.green;
            bool AddPath = GUILayout.Button("[+]~点击这里增加配置路径", GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            if (AddPath)
            {
                config.ExcelFolder.Add("");
            }
        }
    }


    


    public void Fix(string _Path,List<string> _Folders)
    {
        if (!Directory.Exists(_Path)) { Directory.CreateDirectory(_Path); }
        if (!_Path.EndsWith(@"\")) { _Path += @"\"; }
        for (int i = 0; i < _Folders.Count; i++)
        {
            string fullName = _Path + _Folders[i];
            if (!Directory.Exists(fullName)) { Directory.CreateDirectory(fullName); }
        }
    }
}

