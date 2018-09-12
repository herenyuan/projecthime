using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// E数据库
/// 键 -> 列表结构存储
/// </summary>
public sealed class HIMEDataManager : SingleMono<HIMEDataManager>
{
    private List<HIMEData> onSearching;
    private Dictionary<System.Type, List<HIMEData>> cache = new Dictionary<System.Type, List<HIMEData>>();

    public void Push(HIMEData data)
    {
        onSearching = Search(data.GetType());
        if (onSearching != null)
        {
            data.index = onSearching.Count;
            onSearching.Add(data);
        }
    }
    private List<HIMEData> Search(System.Type type)
    {
        if (!cache.ContainsKey(type))
        {
            List<HIMEData> onSearching = new List<HIMEData>();
            cache.Add(type, onSearching);
        }
        return cache[type];
    }
    public void Get<T>(int Count, out List<T> datas) where T : HIMEData
    {
        datas = new List<T>();
        List<HIMEData> temp = Search(typeof(T));
        for (int i = 0; i < temp.Count; i++)
        {
            if (i < Count)
            {
                datas.Add((T)temp[i]);
            }
        }
    }
    /// <summary>
    /// 获取一个 T 的 元素数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Get<T>(out List<T> datas) where T : HIMEData
    {
        datas = new List<T>();
        List<HIMEData> temp = Search(typeof(T));
        for (int i = 0; i < temp.Count; i++)
        {
            datas.Add((T)temp[i]);
        }
    }

    public override void Online()
    {
        
    }

    public override void Offline()
    {
        
    }
}