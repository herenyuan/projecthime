using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug模块，缓存输出到列表，用以读取显示
/// </summary>
public class HIMDebug : SingleMono<HIMDebug>
{
    private List<string> Message = new List<string>();
    public int maxLine = 1000;
    public int lineLength = 250;
    public Action<string> onLogCallBack;
    public override void Online()
    {
        //Debug模块上线
    }
    public override void Offline()
    {
        
    }
    public void Log(object _object)
    {
        #if UNITY_EDITOR
        Debug.Log(_object);
        #endif
        this.Push(_object.ToString());
    }
    void Push(string msg)
    {
        if (msg.Length >= lineLength)
        {
            msg = msg.Substring(lineLength) + "...";
        }
        if (Message.Count > maxLine)
        {
            Message.RemoveAt(0);
        }
        Message.Add(msg);
        if (onLogCallBack != null) { onLogCallBack.Invoke(msg); }
    }
}
