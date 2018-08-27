using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SOManager
{
    
}

public class SOResources
{
    public void Create<T>(string assetPath) where T : ScriptableObject
    {
        T so = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(so, assetPath);
    }
}
public class ExcelSetting : ScriptableObject
{
    public string ExcelSrcRoot = "";

}
