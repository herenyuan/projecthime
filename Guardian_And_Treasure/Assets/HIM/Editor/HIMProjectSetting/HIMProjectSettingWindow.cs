using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HIMProjectSettingWindow : EditorWindow
{
    public string assetPath = "Assets/Resources/SO/{0}.asset";
    public string resPath = "Resources/SO/{0}.asset";
    HIMSOProject projectData;
    HIMSOPath pathData;
    public void Initialization()
    {
        projectData = this.Load<HIMSOProject>("HIMSOProject");
        if(projectData == null) { projectData = this.Create<HIMSOProject>("HIMSOProject"); }
        pathData = this.Load<HIMSOPath>("HIMSOPath");
        if (pathData == null) { pathData = this.Create<HIMSOPath>("HIMSOPath"); }
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField(HIMAssetBundleOption.Current.ToString());
        projectData.Date = EditorGUILayout.TextField("日期：", projectData.Date);
        projectData.Version = EditorGUILayout.TextField("版本号：", projectData.Version);
        pathData.SO = EditorGUILayout.TextField("资源路径：", pathData.SO);
        pathData.CAMERA = EditorGUILayout.TextField("资源路径：", pathData.CAMERA);
        pathData.STAGE = EditorGUILayout.TextField("资源路径：", pathData.STAGE);
        pathData.SKILL = EditorGUILayout.TextField("资源路径：", pathData.SKILL);
        pathData.SCENE = EditorGUILayout.TextField("资源路径：", pathData.SCENE);
        pathData.PREFAB = EditorGUILayout.TextField("资源路径：", pathData.PREFAB);
        pathData.MAT = EditorGUILayout.TextField("资源路径：", pathData.MAT);
        pathData.ICON = EditorGUILayout.TextField("资源路径：", pathData.ICON);
        pathData.ITEM = EditorGUILayout.TextField("资源路径：", pathData.ITEM);
        pathData.GUI = EditorGUILayout.TextField("资源路径：", pathData.GUI);
        pathData.CHARACTER = EditorGUILayout.TextField("资源路径：", pathData.CHARACTER);
        pathData.CONFIG = EditorGUILayout.TextField("资源路径：", pathData.CONFIG);
        pathData.EFFECT = EditorGUILayout.TextField("资源路径：", pathData.EFFECT);
        pathData.ENVIROMENT = EditorGUILayout.TextField("资源路径：", pathData.ENVIROMENT);
    }

    public T Create<T>(string soName) where T : ScriptableObject
    {
        string assetName = string.Format(resPath, soName);
        string fullName = Path.Combine(Application.dataPath, assetName);
        string assetSavePath = string.Format(assetPath, soName);
        Object data = CreateInstance<T>();
        FileInfo fi = new FileInfo(fullName);
        if (!fi.Directory.Exists) { fi.Directory.Create(); }
        AssetDatabase.CreateAsset(data, assetSavePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return (T)data;
    }
    public T Load<T>(string soName) where T : ScriptableObject
    {
        string assetName = string.Format(resPath, soName);
        return Resources.Load<T>(assetName);
    }
}
