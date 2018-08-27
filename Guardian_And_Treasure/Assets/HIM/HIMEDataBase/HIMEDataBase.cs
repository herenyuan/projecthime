using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// E数据
/// </summary>
public abstract class EData
{
    public int index;//索引
    public bool Available = false;
    public bool Modify = false;
    public bool Remove = false;
    public System.Type type;
    /// <summary>
    /// 初始化后将自己存入管理器
    /// </summary>
    public EData()
    {
        HIMEDataBase.Push(this);
    }
}
/// <summary>
/// E数据库
/// 键 -> 列表结构存储
/// </summary>
public sealed class HIMEDataBase
{
    private static List<EData> onSearching;
    private static Dictionary<System.Type, List<EData>> cache = new Dictionary<System.Type, List<EData>>();
    public static void Push(EData data)
    {
        onSearching = Search(data.GetType());
        if (onSearching != null)
        {
            data.index = onSearching.Count;
            onSearching.Add(data);
        }
    }
    private static List<EData> Search(System.Type type)
    {
        if (!cache.ContainsKey(type))
        {
            List<EData> onSearching = new List<EData>();
            cache.Add(type, onSearching);
        }
        return cache[type];
    }
    public static void Get<T>(int Count, out List<T> datas) where T : EData
    {
        datas = new List<T>();
        List<EData> temp = Search(typeof(T));
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
    public static List<T> Get<T>() where T : EData
    {
        return null;
    }
}

//-----------------------------------------测试-----------------------------------------
/// <summary>
/// 场景数据结构
/// </summary>
public class Scene : EData
{
    public List<Character> characters { get; set; }
    public List<EventTrigger> triggers { get; set; }
}
public class Character : EData
{
    public int only { get; set; }
    public int hp { get; set; }
    public int maxHp { get; set; }
    public int power { get; set; }
    public int maxPower { get; set; }
    public int level { get; set; }
    public int maxLevel { get; set; }
}
public class EventTrigger : EData
{
    public int only { get; set; }
    public int hp { get; set; }
    public int maxHp { get; set; }
    public int power { get; set; }
    public int maxPower { get; set; }
    public int level { get; set; }
    public int maxLevel { get; set; }
}

//-----------------------------------------