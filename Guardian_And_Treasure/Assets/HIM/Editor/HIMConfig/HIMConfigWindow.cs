
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HIMConfigWindow : EditorWindow
{
    public void Initialization()
    {

    }
    public bool viewPath = true;
    private void OnGUI()
    {
        
        EditorGUILayout.LabelField("[1]基本设置");
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("目标平台：", HIMEditorUtility.BuildType.ToString());
        //config.Date = EditorGUILayout.TextField("当前日期：", config.Date);
        HIMEditorUtility.ZroConfig.Version = EditorGUILayout.TextField("版本号：", HIMEditorUtility.ZroConfig.Version);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("[2]资源库设置", GUILayout.Width(120));
        viewPath = EditorGUILayout.Toggle("展开详细", viewPath);
        EditorGUILayout.EndHorizontal();

        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        if (viewPath)
        {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("导入库", GUILayout.Width(60));
            EditorGUILayout.LabelField(HIMEditorUtility.ImportPath);


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("导出库", GUILayout.Width(60));
            EditorGUILayout.LabelField(HIMEditorUtility.ExportPath);
            EditorGUILayout.EndHorizontal();
            bool check = GUILayout.Button("创建");
            if (check)
            {
                this.Fix(HIMEditorUtility.ImportPath, HIMEditorUtility.ZroConfig.Paths);
                EditorUtility.SetDirty(HIMEditorUtility.ZroConfig);
                AssetDatabase.Refresh();
            }
            for (int i = 0; i < HIMEditorUtility.ZroConfig.Entries.Count;)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(string.Format("库目录[{0}]", i), GUILayout.Width(80));
                string inputEntry = EditorGUILayout.TextField(HIMEditorUtility.ZroConfig.Entries[i], GUILayout.Width(120));
                if (inputEntry != HIMEditorUtility.ZroConfig.Entries[i])
                {
                    HIMEditorUtility.ZroConfig.Entries[i] = inputEntry;
                    EditorUtility.SetDirty(HIMEditorUtility.ZroConfig);
                }
                string inputPath = EditorGUILayout.TextField(HIMEditorUtility.ZroConfig.Paths[i]);
                if (inputPath != HIMEditorUtility.ZroConfig.Paths[i])
                {
                    HIMEditorUtility.ZroConfig.Paths[i] = inputPath;
                    EditorUtility.SetDirty(HIMEditorUtility.ZroConfig);
                }
                //HIMEditorUtility.ZroConfig.Entries[i] = EditorGUILayout.TextField(HIMEditorUtility.ZroConfig.Entries[i], GUILayout.Width(120));
                //HIMEditorUtility.ZroConfig.Paths[i] = EditorGUILayout.TextField(HIMEditorUtility.ZroConfig.Paths[i]);
                GUI.color = Color.red;
                bool remove = GUILayout.Button("移除[-]", GUILayout.Width(100));
                GUI.color = Color.white;
                if (remove)
                {
                    HIMEditorUtility.ZroConfig.Entries.RemoveAt(i);
                    HIMEditorUtility.ZroConfig.Paths.RemoveAt(i);
                    EditorUtility.SetDirty(HIMEditorUtility.ZroConfig);
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

            if (addNew)
            {
                HIMEditorUtility.ZroConfig.Entries.Add("");
                HIMEditorUtility.ZroConfig.Paths.Add("");
            }
        }
        else
        {
            EditorGUILayout.LabelField(string.Format("以配置 {0} 个资源路径", HIMEditorUtility.ZroConfig.Entries.Count), GUILayout.Width(120));
        }
        EditorGUILayout.Space();
        //导表设置
        EditorGUILayout.LabelField("[3]配置设置", GUILayout.Width(120));
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        for (int i = 0; i < HIMEditorUtility.EdtConfig.ExcelFolder.Count;)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Format("配置路径[{0}]：", i), GUILayout.Width(80));
            string inputString = EditorGUILayout.TextField(HIMEditorUtility.EdtConfig.ExcelFolder[i]);
            if(inputString != HIMEditorUtility.EdtConfig.ExcelFolder[i])
            {
                HIMEditorUtility.EdtConfig.ExcelFolder[i] = inputString;
                EditorUtility.SetDirty(HIMEditorUtility.EdtConfig);
            }
            GUI.color = Color.red;
            bool remove = GUILayout.Button("移除[-]", GUILayout.Width(100));
            GUI.color = Color.white;
            if (remove)
            {
                HIMEditorUtility.EdtConfig.ExcelFolder.RemoveAt(i);
                EditorUtility.SetDirty(HIMEditorUtility.EdtConfig);
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
            HIMEditorUtility.EdtConfig.ExcelFolder.Add("");
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

