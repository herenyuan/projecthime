using proto.cmd;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
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
    private Dictionary<int, Action<Msg>> mCallBack = new Dictionary<int, Action<Msg>>();

    public Action<bool> onSendMsgCallBack;
    public Action onSendMsgOkCallBack;
    public Action<string> onExceptionCallBack;

    public Action onSendMessage;
    public Action onEmptyQueue;

    public override void Online()
    {
        Debug.Log("网络模块初始化");

    }
    public override void Offline()
    {
        mSocketDic.Clear();
        for (int i = 0; i < mSocketList.Count; i++)
        {
            mSocketList[i].Close();
        }
        mSocketList.Clear();
    }

    public void Create(HIMHost _Connection)
    {
        //创建连接对象
        if (!mSocketDic.ContainsKey(_Connection.FullName))
        {
            GameObject GO = new GameObject(string.Format("[{0}]", _Connection.FullName));
            GO.transform.SetParent(transform);
            HIMSocket socket = GO.AddComponent<HIMSocket>();
            socket.Set(_Connection.IP, _Connection.Port);
            socket.onResultCallBack = OnResultCallBack;
            socket.onLogCallBack = OnLogCallBack;
            socket.onReceiveMsgCallBack = OnReceiveMsgCallBack;
            mSocketDic.Add(_Connection.FullName, socket);
        }
    }
    void OnConnectCallBack(HIMSocket.Result result)
    {
        HIMDebug.Ins.Log(result.ToString());
    }
    void OnResultCallBack(HIMSocket.Result result)
    {
        switch (result.Code)
        {
            case SocketCode.error:
                if (onExceptionCallBack != null) { onExceptionCallBack.Invoke(result.ToString()); }
                break;
            default:
                HIMDebug.Ins.Log(result.ToString());
                break;
        }

    }
    void OnLogCallBack(string log)
    {
        HIMDebug.Ins.Log(log);
    }

    void OnReceiveMsgCallBack(List<Msg> msgs)
    {
        for (int i = 0; i < msgs.Count; i++)
        {
            Msg data = msgs[i];
            if (mCallBack.ContainsKey(data.cmd))
            {
                mCallBack[data.cmd].Invoke(data);
                if (onSendMsgOkCallBack != null) { onSendMsgOkCallBack.Invoke(); }
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

    public void Send<T>(HIMHost _Connection, EchoCmd cmd, T target, bool visible = true)
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
            if (onSendMsgCallBack != null) { onSendMsgCallBack.Invoke(visible); }
        }
        else
        {
            Debug.Log(string.Format("<color=#00ff00>{0}</color> connection not exist...", _Connection.IP));
        }
    }
    /// <summary>
    /// 无内容消息
    /// </summary>
    /// <param name="way"></param>
    /// <param name="cmd"></param>
    public void Send(HIMHost _Connection, EchoCmd cmd, bool visible = true)
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
            if (onSendMsgCallBack != null) { onSendMsgCallBack.Invoke(visible); }
        }
        else
        {
            Debug.Log(string.Format("<color=#00ff00>{0}</color> connection not exist...", _Connection.IP));
        }
    }
    public void Close()
    {
        mCallBack.Clear();
        for (int i = 0; i < mSocketList.Count; i++)
        {
            mSocketList[i].Close();
        }
        mSocketList.Clear();
        mSocketDic.Clear();

    }
}


