using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HIMAssetBundleWindow : EditorWindow
{
    List<BundleInfo> bundleCollection = new List<BundleInfo>();
    public void Initialization()
    {

    }
    private void OnEnable()
    {
        Debug.Log("窗口 Enable");    
    }
    private void OnGUI()
    {
        Object [] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < objects.Length; i++)
        {
            EditorGUILayout.LabelField(objects[i].name);
        }
        EditorGUILayout.EndVertical();
    }
    
}
