using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;




/// <summary>
/// HIM 的 Socket 扩展
/// mono单例，
/// 为防止跨线程错误和同步，以及时间缩放，导致无法回调
/// 数据解析后分发在 LateUpdate / Update 中
/// </summary>
public class HIMSocket : MonoBehaviour
{
    public class Result
    {
        public string Host;
        public SocketCode Code = SocketCode.none;
        public string Message;
        public Result(string _Host, SocketCode _Code, string _Message)
        {
            Host = _Host;
            Code = _Code;
            Message = _Message;
        }
        public override string ToString()
        {
            return string.Format("<color=#00ff00>[{0}]</color> ----> {1}...", Host, Message);
        }
    }
    private string host = "127.0.0.1";
    private int port = 9000;
    public string Host { get { return host; } private set { host = value; } }
    public int Port { get { return port; } private set { port = value; } }
    public string Name { get { return string.Format("{0}:{1}", host, port); } }

    /// <summary>
    /// Socket 是否活动状态
    /// </summary>
    public bool Activition { get { return mSocket != null && mSocket.Connected; } }
    private Socket mSocket;

    public Action<Result> onResultCallBack;
    public Action<List<Msg>> onReceiveMsgCallBack;
    public Action<string> onLogCallBack;

    public Queue<Msg> SendQueue;
    public Queue<List<Msg>> ReceiveQueue;

    public Queue<Result> ResultQueue;
    public Queue<string> LogQueue;
    public Msg CurrentSendMsg;
    private Thread mWorkThread;

    public Msg MSGSending { get; set; }
    private List<byte> ReceivedBytes = new List<byte>();

    /// <summary>
    /// 消息头需要的字节数
    /// </summary>
    public const uint HEAD_SIZE = 4;
    public bool Decoding = false;
    public IPAddress mAddress;
    public IPEndPoint mEndPoint;
    public void Set(string _host, int _port)
    {
        //创建连接对象
        //判断 Host
        if (string.IsNullOrEmpty(_host)) { this.PushResult(new Result(host, SocketCode.error, "Host is not correct...")); }
        //判断 Port
        if (_port <= 0) { this.PushResult(new Result(host, SocketCode.error, "Port is not correct...")); }
        Host = _host;
        Port = _port;
        SendQueue = new Queue<Msg>();
        ReceiveQueue = new Queue<List<Msg>>();
        ResultQueue = new Queue<Result>();
        LogQueue = new Queue<string>();
        if (IPAddress.TryParse(host, out mAddress))
        {
            //IP地址格式
            mEndPoint = new IPEndPoint(mAddress, port);
        }
        else
        {
            //域名格式
            IPHostEntry hostEntry = Dns.GetHostEntry(host);
            mEndPoint = new IPEndPoint(hostEntry.AddressList[0], port);
        }
    }
    /// <summary>
    /// 不受时间缩放影响
    /// </summary>
    private void LateUpdate()
    {
        if (LogQueue.Count > 0)
        {
            if (onLogCallBack != null) { onLogCallBack.Invoke(LogQueue.Dequeue()); }
        }
        if (ResultQueue.Count > 0)
        {
            //发生异常
            Result result = ResultQueue.Dequeue();
            if (onResultCallBack != null) { onResultCallBack.Invoke(result); }
        }

        if (ReceiveQueue.Count > 0)
        {
            List<Msg> msgs = ReceiveQueue.Dequeue();
            if (onReceiveMsgCallBack != null) { onReceiveMsgCallBack.Invoke(msgs); }
        }

    }
    public void Connect()
    {
        //开始连接服务器
        if (mEndPoint != null && mAddress != null)
        {
            this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult asyncResult = this.mSocket.BeginConnect(mEndPoint, new AsyncCallback(this.ConnectAsync), this.mSocket);
        }
    }

    public void Reconnect()
    {
        if (mEndPoint != null && mAddress != null)
        {
            this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult asyncResult = this.mSocket.BeginConnect(mEndPoint, new AsyncCallback(this.ConnectAsync), this.mSocket);
        }
    }
    public void Set(Action<Result> _onConnectResultCallBack, Action<Result> _onExceptionCallBack)
    {

    }
    /// <summary>
    /// 异步连接开始
    /// </summary>
    /// <param name="result"></param>
    private void ConnectAsync(IAsyncResult result)
    {
        try
        {
            this.mSocket.EndConnect(result);
            if (this.mSocket != null && this.mSocket.Connected)
            {
                mWorkThread = new Thread(SocketWork);
                mWorkThread.Start(); //启动工作线程
                string.Format(string.Format("<color=#00ff00>[{0}]</color> connect complete...", Host));
                this.PushResult(new Result(host, SocketCode.success, "connect success..."));
            }
            else
            {
                this.PushResult(new Result(host, SocketCode.error, "connect faild..."));
            }
        }
        catch (Exception ex)
        {
            //连接后的异常
            this.PushResult(new Result(host, SocketCode.error, ex.Message));
        }
    }
    /// <summary>
    /// 每帧同步
    /// </summary>
    void SocketWork()
    {
        while (this.mSocket != null && this.mSocket.Connected)
        {
            this.SendProgress();
            this.ReceiveOrAnalysis();
            Thread.Sleep(100);
        }
        this.Close();
        this.PushResult(new Result(host, SocketCode.close, "connection closed"));
    }
    public void Send(Msg msg)
    {
        if (!Activition) { this.Connect(); }
        SendQueue.Enqueue(msg);
    }

    void SendProgress()
    {
        if (SendQueue.Count <= 0) { return; }
        try
        {
            if (CurrentSendMsg == null) { CurrentSendMsg = SendQueue.Dequeue(); }//发送完成才能继续下一条
            if (CurrentSendMsg != null && CurrentSendMsg.Valid)
            {
                this.PushLog(string.Format("send  <color=#ff0000>[{0}]</color> -----------------------> <color=#00ff00>[{1}]</color>", CurrentSendMsg.cmd, host));
                byte[] buffer = this.Encode(CurrentSendMsg);
                this.mSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);//阻塞发送
                CurrentSendMsg = null;
            }
        }
        catch (Exception ex)
        {
            //发送时出现异常
            this.PushResult(new Result(host, SocketCode.error, ex.Message));
        }
    }

    void ReceiveOrAnalysis()
    {
        if (this.mSocket != null && mSocket.Available > 0)
        {
            int AvailableSize = mSocket.Available;
            try
            {
                byte[] ReceiveBuff = new byte[AvailableSize];//每次接收全部的字节 int32 上限
                this.mSocket.Receive(ReceiveBuff, 0, ReceiveBuff.Length, SocketFlags.None);//阻塞接收，完成接收 Available = 0
                ReceivedBytes.AddRange(ReceiveBuff);//接收网络流，填充进接收流
                List<Msg> ReceivedDatas = new List<Msg>();
                //开始解析
                while (ReceivedBytes.Count >= 4)
                {
                    byte[] head = ReceivedBytes.GetRange(0, 4).ToArray();//获取第一个int
                    ReceivedBytes.RemoveRange(0, 4);//移除头字节
                    int MaxSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(head, 0));
                    if (ReceivedBytes.Count < MaxSize)
                    {
                        //剩余消息体不够完整等待中，需要进行分包和粘包
                        this.PushLog("-------------------------------------------->出现分包情况");
                    }
                    else
                    {
                        //CMD长度（short），CMD内容（string），PROTO内容（object）
                        byte[] neck = ReceivedBytes.GetRange(0, 2).ToArray();
                        ReceivedBytes.RemoveRange(0, 2);//移除这个消息的neck字节
                        int cmdSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(neck, 0));
                        //CMD内容（string），PROTO内容（object）
                        byte[] cmdBuff = ReceivedBytes.GetRange(0, cmdSize).ToArray();
                        ReceivedBytes.RemoveRange(0, cmdSize);
                        string cmdString = Encoding.UTF8.GetString(cmdBuff);
                        int cmd = Convert.ToInt32(cmdString);
                        //PROTO内容（object）
                        byte[] content = ReceivedBytes.GetRange(0, MaxSize - 2 - cmdSize).ToArray();
                        ReceivedBytes.RemoveRange(0, MaxSize - 2 - cmdSize);
                        Msg data = new Msg(cmd, content);
                        ReceivedDatas.Add(data);//将数据流解包到列表，推入接收队列
                    }
                }
                ReceiveQueue.Enqueue(ReceivedDatas);//推入接受队列
            }
            catch (Exception ex)
            {
                //接收时出现异常
                this.PushResult(new Result(host, SocketCode.error, ex.Message));
            }
        }
    }

    void PushResult(Result result)
    {
        ResultQueue.Enqueue(result);
    }
    void PushLog(string log)
    {
        LogQueue.Enqueue(log);
    }
    public void Close()
    {
        this.PushLog(string.Format("[{0}] close...", host));
        if (mSocket.Connected) { mSocket.Disconnect(true); }
        mSocket = null;
    }
    /// <summary>
    /// 拼装消息
    /// 消息体结构：
    /// 0~4 -> 总长度(不包括自生) int
    /// 5~6 -> 命令号长度 short
    /// 7~10 -> 消息命令号 int
    /// 11~n -> 消息内容 byte[]
    /// </summary>
    /// <returns></returns>
    public byte[] Encode(Msg msg)
    {
        List<byte> Buffer = new List<byte>();
        byte[] CmdBuff = MSGEncoding.GetString(msg.cmd.ToString());//命令号内容
        byte[] ContentBuff = msg.content;// Proto 的内容
        int MSGLength = 2 + CmdBuff.Length + ContentBuff.Length;//总长度，不包含头的 4 字节
        byte[] MSGSizeBuff = MSGEncoding.GetInt(MSGLength);//消息总尺寸
        byte[] CmdSizeBuff = MSGEncoding.GetShort((short)CmdBuff.Length);//命令号尺寸

        Buffer.AddRange(MSGSizeBuff);
        Buffer.AddRange(CmdSizeBuff);
        Buffer.AddRange(CmdBuff);
        Buffer.AddRange(ContentBuff);
        byte[] final = Buffer.ToArray();

        return final;
    }
}
