
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HIMConfigWindow : EditorWindow
{
    private HIMSoZero zero;
    private HIMSoResource src;
    public void Initialization()
    {
        zero = HIMEditorUtility.LoadAsset<HIMSoZero>("Assets/Resources/HIMSoZero.asset");
        src = HIMEditorUtility.LoadAsset<HIMSoResource>("Assets/Resources/HIMSoResource.asset");

    }
    public bool viewPath = true;
    private void OnGUI()
    {
        if (zero == null)
        {
            bool createZero = GUILayout.Button("创建本地配置");
            if(createZero)
            {
               
                if (zero == null) { zero = HIMEditorUtility.Create<HIMSoZero>("Assets/Resources/HIMSoZero.asset"); }
            }
        }
        else
        {
            EditorGUILayout.LabelField("[1]基本设置");
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("目标平台：", HIMAssetBundleOption.Current.ToString());
            zero.Date = EditorGUILayout.TextField("当前日期：", zero.Date);
            zero.Version = EditorGUILayout.TextField("版本号：", zero.Version);
            EditorGUILayout.Space();
        }

        if(src == null)
        {
            bool createPath = GUILayout.Button("创建本地配置");
            if(createPath)
            {
                if (src == null) { src = HIMEditorUtility.Create<HIMSoResource>("Assets/Resources/HIMSoResource.asset"); }
            }
            
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("[2]资源库设置", GUILayout.Width(120));
            viewPath = EditorGUILayout.Toggle("展开详细", viewPath);
            EditorGUILayout.EndHorizontal();
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            if (viewPath)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("根路径：", GUILayout.Width(60));
                src.ABResources = EditorGUILayout.TextField(src.ABResources);
                bool check = GUILayout.Button("检测");
                if (check)
                {
                    //检测文件夹
                    HIMEditorUtility.CheckPath(src.ABResources, src.value);
                }
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < src.key.Count;)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("资源路径：", GUILayout.Width(60));
                    src.key[i] = EditorGUILayout.TextArea(src.key[i], GUILayout.Width(120));
                    src.value[i] = EditorGUILayout.TextArea(src.value[i], GUILayout.ExpandWidth(true));
                    GUI.color = Color.red;
                    bool remove = GUILayout.Button("移除[-]", GUILayout.Width(60));
                    GUI.color = Color.white;
                    if (remove)
                    {
                        src.RemvoeAt(i);
                    }
                    else
                    {
                        i++;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUI.color = Color.green;
                bool addNew = GUILayout.Button("新增路径[+]");
                GUI.color = Color.white;
                if (addNew)
                {
                    src.AddNew();
                }
            }
            else
            {
                EditorGUILayout.LabelField(string.Format("以配置 {0} 个资源路径", src.key.Count), GUILayout.Width(120));
            }
        }
    }
}

