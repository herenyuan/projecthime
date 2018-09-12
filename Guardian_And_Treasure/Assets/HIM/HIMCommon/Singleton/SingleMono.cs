using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleMono<T> : MonoBehaviour where T : Component, new()
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
            string showName = string.Format("[{0}]", type.Name);
            GameObject _donDestroyGo = new GameObject(showName);
            DontDestroyOnLoad(_donDestroyGo);
            single = _donDestroyGo.AddComponent<T>();
        }
        return single;
    }
    public abstract void Online();
    public abstract void Offline();
}
