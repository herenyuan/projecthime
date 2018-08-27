using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Collections;

public enum EProtocolType
{
    TYPE_Register = 10100,
    TYPE_GuestLogin = 10101,
    TYPE_Login = 10102,
    TYPE_GetOnlineIpPort = 10106,
    TYPE_LoginOnline = 20100,
}

public class MsgHead
{
    public Int32 len = 0; //整个msg长度
    public Int16 cmdID = 0;
    public Int32 userID = 0;
    public Int32 seq = 0;
    public Int32 ret = 0;
    public MsgHead(byte[] data)
    {
        if (data != null)
        {
            len = BitConverter.ToInt32(data, 0);
            cmdID = BitConverter.ToInt16(data, 4);
            userID = BitConverter.ToInt32(data, 6);
            seq = BitConverter.ToInt32(data, 10);
            ret = BitConverter.ToInt32(data, 14);

            //len = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 0));
            //cmdID = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, 4));
            //userID = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 6));
            //seq = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 10));
            //ret = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 14));
        }
    }
}

public class Protocol
{
    public static int MAXMSGSIZE = 512 * 1024;
    public static int HeadSize = 18; //包头长度
    public MsgHead msghead;
    public byte[] bodyBuff;

    protected Int16 m_Type;

    public Protocol(MsgHead msghead, byte[] bodyBuff)
    {
        this.msghead = msghead;
        this.bodyBuff = bodyBuff;
        m_Type = msghead.cmdID;
    }

    public Int16 GetMsgType()
    {
        return this.m_Type;
    }

    public T Deserialize<T>()
    {
        if (bodyBuff.Length == 0) return default(T);
        MemoryStream source = new MemoryStream(bodyBuff);
        T pf = ProtoBuf.Serializer.Deserialize<T>(source);
        source.Close();
        source = null;
        return pf;
    }

    public byte[] Encode()
    {
        System.IO.MemoryStream ms1 = new System.IO.MemoryStream();
        byte[] lenByte = HostToNetworkOrder(msghead.len);
        byte[] pbType = HostToNetworkOrder((int)msghead.cmdID, 16); //协议号
        byte[] userID32 = HostToNetworkOrder(msghead.userID);
        byte[] seq32 = HostToNetworkOrder(msghead.seq);
        byte[] ret32 = HostToNetworkOrder(msghead.ret);
        ms1.Write(lenByte, 0, lenByte.Length);
        ms1.Write(pbType, 0, pbType.Length);
        ms1.Write(userID32, 0, userID32.Length);
        ms1.Write(seq32, 0, seq32.Length);
        ms1.Write(ret32, 0, ret32.Length);
        byte[] headBuff = ms1.ToArray();
        ms1.Close();
        ms1 = null;

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        ms.Write(headBuff, 0, headBuff.Length);
        ms.Write(bodyBuff, 0, bodyBuff.Length);
        byte[] resultBytes = ms.ToArray();
        ms.Close();
        ms = null;
        return resultBytes;
    }

    byte[] HostToNetworkOrder(int value, int intFlag = 32)
    {
        string s = value.ToString();
        if (intFlag == 16)
        {
            //return BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(Int16.Parse(s)));
            return BitConverter.GetBytes((Int16.Parse(s)));
        }
        else if (intFlag == 32)
        {
            //return BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(Int32.Parse(s)));
            return BitConverter.GetBytes((Int32.Parse(s)));
        }
        //return BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(value));
        return BitConverter.GetBytes((value));
    }
}
