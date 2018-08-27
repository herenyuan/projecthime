using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    public static T SINGLE;
    public static T Ins
    {
        get
        {
            return Check();
        }
    }
    public static T Check()
    {
        if (SINGLE == null)
        {
            SINGLE = InsAndGetSingleton();
        }
        return SINGLE;
    }

    public static T InsAndGetSingleton()
    {
        string ClassName = typeof(T).ToString();
        T TempSingleton = new T();
        return TempSingleton;
    }
}
