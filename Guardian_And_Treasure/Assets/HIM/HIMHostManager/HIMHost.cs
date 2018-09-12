using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIMHost
{
    public string Name { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
    public string FullName { get; set; }
    public HIMHost(string _name, string _ip, int _port)
    {
        Name = _name;
        IP = _ip;
        Port = _port;
        FullName = string.Format("{0}:{1}", IP, Port);
    }
}
