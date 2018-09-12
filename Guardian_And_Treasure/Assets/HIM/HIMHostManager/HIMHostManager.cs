using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 主机管理器模块
/// </summary>
public class HIMHostManager : SingleMono<HIMHostManager>
{
    private Dictionary<string, HIMHost> hosts = new Dictionary<string, HIMHost>()
    {
        {"服务器1", new HIMHost("内网测试", "192.168.1.7", 9000) },
        {"服务器2",  new HIMHost("肖格格内网测试", "192.168.1.14", 9000) },
        {"服务器3",  new HIMHost("外网测试", "119.29.114.49", 9000) },
    };

    public override void Online()
    {

    }
    
    public override void Offline()
    {

    }
    public void Load(string _Name, string _IP, int _Port)
    {

    }
}