using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景数据结构
/// </summary>
public class Scene : HIMEData
{
    public List<Character> characters { get; set; }
    public List<EventTrigger> triggers { get; set; }
}