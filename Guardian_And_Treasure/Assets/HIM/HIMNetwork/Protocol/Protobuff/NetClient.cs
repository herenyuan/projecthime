using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using ProtoBuf;

public enum NetworkState
{
    EUnknow,
    ESocketConnected,
    EClosed
}


public class NetClient
{
    public delegate void ProcessProtocolDelegate(Protocol p);
    private ProcessProtocolDelegate _ProcessDelegate;

    protected ushort m_ProcessMsgPerTime = 1000;
    private Socket _Socket = null;
    private int m_RecvSize = 0; //接收数据包长度
    private int m_SendBufPosition = 0; //插入发送buff的位置
    private int m_SendFlag = 0;
    private Queue<Protocol> m_SendQueue = new Queue<Protocol>();
    private Queue<Protocol> m_RecvQueue = new Queue<Protocol>();
    private byte[] m_SendBuf = new byte[524288];
    private byte[] m_RecvBuf = new byte[524288];

    public NetworkState State
    {
        set;
        get;
    }
    public ulong ProfilerSendBytes
    {
        get;
        set;
    }
    public ulong ProfilerReceiveBytes
    {
        get;
        set;
    }
    public NetClient(ProcessProtocolDelegate pDelegate)
    {
        this._ProcessDelegate = pDelegate;
    }
    public bool IsConnected()
    {
        if (null != this._Socket)
        {
            if (this._Socket.Connected)
            {
                return true;
            }
        }
        return false;
    }
    public bool ConnectToServer(string strHost, int nPort)
    {
        bool result;
        if (strHost == null || 0 == nPort)
        {
            Debug.Log("Invalid Parameter!");
            result = false;
        }
        else
        {
            if (this.IsConnected())
            {
                result = true;
            }
            else
            {
                try
                {
                    IPAddress address = null;
                    IPEndPoint remoteEP;
                    if (IPAddress.TryParse(strHost, out address))
                    {
                        remoteEP = new IPEndPoint(address, nPort);
                    }
                    else
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(strHost);
                        remoteEP = new IPEndPoint(hostEntry.AddressList[0], nPort);
                    }
                    this._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IAsyncResult asyncResult = this._Socket.BeginConnect(remoteEP, new AsyncCallback(this._ConnectCallback), this._Socket);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    result = false;
                    return result;
                }
                result = true;
            }
        }
        return result;
    }
    public void CloseSocket()
    {
        this.State = NetworkState.EClosed;
        if (this._Socket != null && this._Socket.Connected)
        {
            this._Socket.Close();
            this._Socket = null;
        }
        this.m_RecvQueue.Clear();
        this.m_SendQueue.Clear();
    }

    public void SendMsg(EProtocolType pType, global::ProtoBuf.IExtensible pbmsg)
    {
        System.IO.MemoryStream msBody = new System.IO.MemoryStream();
        ProtoBuf.Serializer.Serialize<global::ProtoBuf.IExtensible>(msBody, pbmsg);
        byte[] pbBuff = msBody.ToArray();
        msBody.Close();
        msBody = null;
        MsgHead mh = new MsgHead(null);
        mh.len = Protocol.HeadSize + pbBuff.Length;
        mh.cmdID = (Int16)pType;
        mh.userID = 1001;
        mh.seq = 0;
        mh.ret = 0;
        Protocol protocol = new Protocol(mh, pbBuff);

        if (this._Socket != null && this._Socket.Connected)
        {
            Queue<Protocol> sendQueue;
            Monitor.Enter(sendQueue = this.m_SendQueue);
            try
            {
                this.m_SendQueue.Enqueue(protocol);
            }
            finally
            {
                Monitor.Exit(sendQueue);
            }
            if (0 == Interlocked.CompareExchange(ref this.m_SendFlag, 1, 0))
            {
                this._SendData();
            }
        }
    }
    
    public void Update(float fElapsedTimeInMS)
    {
        this._ProcessReceiveMsg();
    }
    public int _ProcessReceiveMsg()
    {
        ushort num;
        int result;
        for (num = 0; num < this.m_ProcessMsgPerTime; num += 1)
        {
            Queue<Protocol> syncRoot;
            Monitor.Enter(syncRoot = this.m_RecvQueue);
            try
            {
                if (this.m_RecvQueue.Count <= 0)
                {
                    result = (int)num;
                    return result;
                }
                Protocol p = this.m_RecvQueue.Dequeue();
                if (this._ProcessDelegate != null)
                {
                    this._ProcessDelegate(p);
                }
            }
            finally
            {
                Monitor.Exit(syncRoot);
            }
        }
        result = (int)num;
        return result;
    }
    private void _ConnectCallback(IAsyncResult asyncConnect)
    {
        this.State = NetworkState.ESocketConnected;
        try
        {
            this._Socket.EndConnect(asyncConnect);
            if (!this._Socket.Connected)
            {
                Debug.Log("Connect Failed!");
            }
            else
            {
                this._ReadData();
                Interlocked.Exchange(ref this.m_SendFlag, 0);
                this._SendData();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    private void _ReadData()
    {
        if (this._Socket == null || !this._Socket.Connected)
        {
            return;
        }
       this._Socket.BeginReceive(this.m_RecvBuf, m_RecvSize, this.m_RecvBuf.Length, SocketFlags.None, new AsyncCallback(this._ReceiveCallback), this.m_RecvBuf);
    }
    private void _ReceiveCallback(IAsyncResult asyncReceive)
    {
        if (this._Socket == null || !this._Socket.Connected)
        {
            return;
        }
        else
        {
            try
            {
                SocketError socketError;
                int num = this._Socket.EndReceive(asyncReceive, out socketError);
                this.ProfilerReceiveBytes += (ulong)((long)num);  //Profiler
                if (num <= 0)
                {
                    this._ReadData();
                }
                else
                {
                    m_RecvSize += num;
                    while (m_RecvSize >= Protocol.HeadSize)
                    {
                        byte[] headBuff = new byte[Protocol.HeadSize];
                        Monitor.Enter(m_RecvBuf);
                        try
                        {
                            Array.Copy(m_RecvBuf, 0, headBuff, 0, headBuff.Length);
                        }
                        finally
                        {
                            Monitor.Exit(m_RecvBuf);
                        }
                        MsgHead msghead = new MsgHead(headBuff); //解析包头
                        int packetLen = msghead.len;
                        //接收长度不足packet长度
                        if (m_RecvSize < packetLen)
                        {
                            this._ReadData();
                            return;
                        }
                        else
                        {
                            byte[] bodyBuff = new byte[packetLen - Protocol.HeadSize]; //包体
                            Monitor.Enter(m_RecvBuf);
                            try
                            {
                                Array.Copy(m_RecvBuf, Protocol.HeadSize, bodyBuff, 0, bodyBuff.Length);
                            }
                            finally
                            {
                                Monitor.Exit(m_RecvBuf);
                            }
                            Protocol msg = new Protocol(msghead, bodyBuff); //解析msg
                            Monitor.Enter(m_RecvQueue);
                            try
                            {
                                this.m_RecvQueue.Enqueue(msg);
                            }
                            finally
                            {
                                Monitor.Exit(m_RecvQueue);
                            }

                            //move recvBuff posion
                            Monitor.Enter(m_RecvBuf);
                            try
                            {
                                m_RecvSize -= packetLen;
                                if (m_RecvSize > 0)
                                {
                                    Array.Copy(m_RecvBuf, packetLen, m_RecvBuf, 0, m_RecvSize);
                                }
                            }
                            finally
                            {
                                Monitor.Exit(m_RecvBuf);
                            }
                        }
                    }
                    this._ReadData();
                }
            }
            catch (Exception ex)
            {
                this.CloseSocket();
                Debug.Log(ex.ToString());
            }
        }
    }
    private void _SendCallback(IAsyncResult iar)
    {
        if (this._Socket == null || !this._Socket.Connected)
        {
            Interlocked.Exchange(ref this.m_SendFlag, 0);
        }
        else
        {
            int num = this._Socket.EndSend(iar);
            this.ProfilerSendBytes += (ulong)((long)num); //Profiler
            if (num > 0)
            {
                Monitor.Enter(this.m_SendBuf);
                try
                {
                    m_SendBufPosition -= num; 
                    if (m_SendBufPosition < 0) m_SendBufPosition = 0; //if necessary
                    Array.Copy(this.m_SendBuf, num, this.m_SendBuf, 0, num);
                }
                finally
                {
                    Monitor.Exit(this.m_SendBuf);
                }
            }
            this._SendData();
        }
    }
    private void _SendData()
    {
        if (this._Socket != null && this._Socket.Connected)
        {
            Protocol protocol = null;
            bool flag = false;
            Queue<Protocol> sendQueue;
            Monitor.Enter(sendQueue = this.m_SendQueue);
            try
            {
                if (this.m_SendQueue.Count > 0)
                {
                    protocol = this.m_SendQueue.Dequeue();
                }
                else
                {
                    flag = true;
                }
            }
            finally
            {
                Monitor.Exit(sendQueue);
            }
            if (flag)
            {
                Interlocked.Exchange(ref this.m_SendFlag, 0);
            }
            else
            {
                byte[] pbBuf = protocol.Encode();
                Array.Copy(pbBuf, 0, this.m_SendBuf, m_SendBufPosition, pbBuf.Length);
                m_SendBufPosition += pbBuf.Length;
                try
                {
                    this._Socket.BeginSend(this.m_SendBuf, 0, m_SendBufPosition, SocketFlags.None, new AsyncCallback(this._SendCallback), null);
                }
                catch (Exception ex)
                {
                    this.CloseSocket();
                    Debug.Log(ex.ToString());
                }
            }
        }
    }
}