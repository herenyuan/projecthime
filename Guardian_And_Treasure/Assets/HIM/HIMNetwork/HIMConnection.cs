using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIMConnection
{
    public string Name { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string FullName { get; set; }
    public HIMConnection(string _name, string _host, int _port)
    {
        Name = _name;
        Host = _host;
        Port = _port;
        FullName = string.Format("{0}:{1}", Host, Port.ToString());
    }
}

public class HIMConnectionManager
{
    public static void Set(int index)
    {
        if (index < 0 || index >= Connections.Count) { Debug.LogError(string.Format("connection <color=#ff0000>[{0}]</color> on choose is not exist....", index)); }
        else
        {
            Current = Connections[index];
        }
    }
    public static HIMConnection Current { get; private set; }
    public static List<HIMConnection> Connections = new List<HIMConnection>()
    {
        #if UNITY_EDITOR
        new HIMConnection("内网测试", "192.168.1.7", 9000),
        new HIMConnection("肖格格内网测试", "192.168.1.14", 9000),
        #endif
        new HIMConnection("外网测试", "119.29.114.49", 9000),
    };
}