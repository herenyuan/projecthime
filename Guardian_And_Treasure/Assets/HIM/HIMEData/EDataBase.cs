using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// E数据
/// </summary>
public abstract class HIMEData
{
    public int index;//索引
    public bool Available = false;
    public bool Modify = false;
    public bool Remove = false;
    public System.Type type;
    /// <summary>
    /// 初始化后将自己存入管理器
    /// </summary>
    public HIMEData()
    {
        HIMEDataManager.Ins.Push(this);
    }
}
