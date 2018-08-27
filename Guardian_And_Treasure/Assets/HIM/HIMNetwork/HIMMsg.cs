using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class Msg
{
    public int cmd;
    public byte[] content;
    public Msg(int _cmd, byte[] _content)
    {
        cmd = _cmd;
        content = _content;
    }

    public bool Valid
    {
        get
        {
            bool cmdValid = cmd >= 0;
            bool contentValid = content != null;
            bool contentLength = content.Length >= 0;
            bool final = cmdValid && contentValid && contentLength;
            return final;
        }
    }
}

public class MSGEncoding
{
    public static byte[] GetInt(int value)
    {
        int final = IPAddress.HostToNetworkOrder(value);
        byte[] bytes = BitConverter.GetBytes(final);
        return bytes;
    }

    public static byte[] GetShort(short value)
    {
        short final = IPAddress.HostToNetworkOrder(value);
        byte[] bytes = BitConverter.GetBytes(final);
        return bytes;
    }
    public static byte[] GetString(string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }
}
