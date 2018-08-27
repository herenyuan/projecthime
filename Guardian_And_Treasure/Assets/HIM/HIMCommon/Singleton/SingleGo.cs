using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CORE;

/// <summary>
/// 单一场景物体
/// 需要加载预设物体
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleGo<T> : MonoBehaviour where T : Component, new()
{
    static T single;
    public static T Ins
    {
        get { return CreateOrSearch(); }
    }
    private static T CreateOrSearch()
    {
        if (single == null)
        {
            System.Type type = typeof(T);
            string _name = string.Format("[{0}]", type.Name);
            GameObject _donDestroyGo;
            RESOURCESEx.Error error = RESOURCESEx.MakeGo(Path.MODULE, type.Name, out _donDestroyGo, null);
            if (error == RESOURCESEx.Error.none)
            {
                _donDestroyGo.name = _name;
                DontDestroyOnLoad(_donDestroyGo);
                single = _donDestroyGo.AddComponent<T>();
            }
            else
            {
                throw new System.Exception("不存在----------------------> [" + type.Name + "] 的模块资源...");
            }
        }
        return single;
    }
}
