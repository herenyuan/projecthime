using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIM
{
    /// <summary>
    /// 程序启动时调用
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void Online()
    {
        Debug.Log("HIM online test...");
        
        //HIMSoResource data = HIMResources.Ins.LoadSO<HIMSoResource>("SO/", "HIMSoResource");
        //for (int i = 0; i < data.key.Count; i++)
        //{
        //    Path.Add(data.key[i], data.value[i]);
        //}
        //int a = 0;
    }
}
