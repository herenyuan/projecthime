using proto.cmd;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// mono单利对象
/// 有待优化
/// HIMSocket 管理器
/// </summary>
public class HIMNetwork : SingleMono<HIMNetwork>
{
    private Dictionary<string, HIMSocket> mSocketDic = new Dictionary<string, HIMSocket>();
    private List<HIMSocket> mSocketList = new List<HIMSocket>();
    /// <summary>
    /// 连接结果回调
    /// </summary>
    public Dictionary<string, Action<HIMSocket.Result>> ResultCallBack = new Dictionary<string, Action<HIMSocket.Result>>();
    /// <summary>
    /// 连接错误回调
    /// </summary>
    public Dictionary<string, Action<HIMSocket.Result>> ErrorCallBack = new Dictionary<string, Action<HIMSocket.Result>>();
    private Dictionary<int, Action<Msg>> mCallBack = new Dictionary<int, Action<Msg>>();


    public Action<string> onExceptionCallBack;

    public void Online()
    {
        Debug.Log("网络模块初始化");
    }
    public void Offline()
    {
        mSocketDic.Clear();
        for (int i = 0; i < mSocketList.Count; i++)
        {
            mSocketList[i].Close();
        }
        mSocketList.Clear();
    }

    public void Create(HIMConnection _Connection)
    {
        //创建连接对象
        if (!mSocketDic.ContainsKey(_Connection.FullName))
        {
            GameObject GO = new GameObject(string.Format("[{0}]", _Connection.FullName));
            GO.transform.SetParent(transform);
            HIMSocket socket = GO.AddComponent<HIMSocket>();
            socket.Set(_Connection.Host, _Connection.Port);
            socket.onConnectCallBack = OnConnectCallBack;
            socket.onExceptionCallBack = OnExceptionCallBack;
            socket.onReceiveMsgCallBack = OnReceiveMsgCallBack;
            socket.onCloseCallBack = null;
            mSocketDic.Add(_Connection.FullName, socket);
        }
    }
    void OnConnectCallBack(HIMSocket.Result result)
    {
        Debug.Log(string.Format("<color=#00ff00>[{0}]</color> is {1}...", result.Host, result.Message));
    }
    void OnExceptionCallBack(HIMSocket.Result result)
    {
        Debug.Log(string.Format("<color=#00ff00>[{0}]</color> is {1}...", result.Host, result.Message));
    }
    void OnCloseCallBack(HIMSocket.Result result)
    {
        Debug.Log(string.Format("<color=#00ff00>[{0}]</color> is {1}...", result.Host, result.Message));
    }
    void OnReceiveMsgCallBack(List<Msg> msgs)
    {
        for (int i = 0; i < msgs.Count; i++)
        {
            Msg data = msgs[i];
            if (mCallBack.ContainsKey(data.cmd))
            {
                mCallBack[data.cmd].Invoke(data);
            }
            else
            {
                Debug.Log(string.Format("unknow cmd <color=#ff0000>[{0}]</color>", data.cmd));
            }
        }
    }

    public void Add(int _Cmd, Action<Msg> _CallBack)
    {
        if (mCallBack.ContainsKey(_Cmd))
        {
            mCallBack[_Cmd] = _CallBack;//同名替换
        }
        else
        {
            mCallBack.Add(_Cmd, _CallBack);
        }
    }

    public HIMSocket Search(string _Host)
    {
        if (mSocketDic.ContainsKey(_Host))
        {
            return mSocketDic[_Host];
        }
        return null;
    }

    public T GetData<T>(Msg msg) where T : IExtensible
    {
        MemoryStream stream = new MemoryStream(msg.content);
        return Serializer.Deserialize<T>(stream);
    }

    public void Send<T>(HIMConnection _Connection, EchoCmd cmd, T target)
    {
        if (_Connection == null)
        {
            if (onExceptionCallBack != null) { onExceptionCallBack.Invoke("connection is null or error, please check..."); }
            return;
        }
        this.Create(_Connection);
        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<T>(stream, target);
        HIMSocket ins = this.Search(_Connection.FullName);
        if (ins != null)
        {
            ins.Send(new Msg((int)cmd, stream.ToArray()));
        }
        else
        {
            Debug.Log(string.Format("<color=#00ff00>{0}</color> connection not exist...", _Connection.Host));
        }
    }
    /// <summary>
    /// 无内容消息
    /// </summary>
    /// <param name="way"></param>
    /// <param name="cmd"></param>
    public void Send(HIMConnection _Connection, EchoCmd cmd)
    {
        if (_Connection == null)
        {
            if (onExceptionCallBack != null) { onExceptionCallBack.Invoke("connection is null or error, please check..."); }
            return;
        }
        this.Create(_Connection);
        HIMSocket ins = this.Search(_Connection.FullName);
        if (ins != null)
        {
            ins.Send(new Msg((int)cmd, new byte[0]));
        }
        else
        {
            Debug.Log(string.Format("<color=#00ff00>{0}</color> connection not exist...", _Connection.Host));
        }
    }
    public void Close()
    {

    }

}

