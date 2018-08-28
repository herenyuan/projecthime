using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HIMProjectSettingWindow : EditorWindow
{
    HIMProject projectData;
    public void Initialization()
    {
        projectData = Resources.Load<HIMProject>("SO/HIMProject");
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField(HIMPlatform.Current.ToString());
        if (projectData == null)
        {
            bool create = GUILayout.Button("创建工程设置");
            if (create)
            {
                string path = "Assets/Resources/SO/HIMProject.asset";
                projectData = ScriptableObject.CreateInstance<HIMProject>();
                if (!Directory.Exists(Application.dataPath + "/Resources/SO/")) { Directory.CreateDirectory(Application.dataPath + "/Resources/SO/"); AssetDatabase.Refresh(); }
                AssetDatabase.CreateAsset(projectData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
