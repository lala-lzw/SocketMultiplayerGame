using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using System.Threading;
using SocketGameProtocol;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;
    private Thread aucThread;
    private const string ip = "127.0.0.1";

    public ClientManager(GameFace face) : base(face){}

    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        InitSocket();
        
        InitUDP();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;
        CloseSocket();
        if (aucThread != null)
        {
            aucThread.Abort();
            aucThread = null;
        }
        
    }

    /// <summary>
    /// 初始化socket
    /// </summary>
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(ip, 6666);
            //连接成功
            StartReceive();
            face.ShowMessage("连接成功！");
        }
        catch(Exception e)
        {
            //连接出错
            Debug.LogWarning(e);
            face.ShowMessage("连接失败！");
        }
    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    private void CloseSocket()
    {
        if (socket.Connected&&socket!=null)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.Remsize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult iar)
    {
        try
        {
            if (socket == null || socket.Connected == false) return;
            int len = socket.EndReceive(iar);
            if (len == 0)
            {
                Debug.Log("数据为0");
                CloseSocket();
                return;
            }

            message.ReadBuffer(len, HandleResponse);
            StartReceive();
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    private void HandleResponse(MainPack pack)
    {
        face.HandleResponse(pack);
        //Debug.Log("client处理");
    }

    public void Send(MainPack pack)
    {
        if (socket.Connected == false || socket == null) return;
        socket.Send(Message.PackData(pack));
    }
    
    
    //UDP协议

    private Socket udpClient;
    private IPEndPoint ipEndPoint;
    private EndPoint EPoint;
    private Byte[] buffer=new Byte[1024];

    private void InitUDP()
    {
        udpClient=new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
        ipEndPoint=new IPEndPoint(IPAddress.Parse(ip),6667);
        EPoint =ipEndPoint;
        try
        {
            udpClient.Connect(EPoint);
        }
        catch
        {
            Debug.Log("UDP连接失败！");
            return;
        }

        Loom.RunAsync(() =>
            {
                aucThread=new Thread(ReceiveMsg);
                aucThread.Start();
            }
        );



    }

    private void ReceiveMsg()
    {
        Debug.Log("UDP开始接收");
        while (true)
        {
            int len= udpClient.ReceiveFrom(buffer, ref EPoint);
            MainPack pack = (MainPack) MainPack.Descriptor.Parser.ParseFrom(buffer,0,len);
            // if (pack.Actioncode == ActionCode.UpPos)
            // {
            //     if (pack.Playerpack[0].Playername == "1234")
            //     {
            //         face.isRec = true;
            //     }
            // }
            
            //Debug.Log(Encoding.UTF8.GetString(buffer,0,len));
            //Debug.Log("接收数据："+pack.Actioncode.ToString()+pack.User);
            Loom.QueueOnMainThread((param) =>
                {
                    HandleResponse(pack);
                },null);
            
            //face.isRec = true;
        }
    }
    
    

    public void SendTo(MainPack pack)
    {
        Byte[] sendBuff = Message.PackDataUDP(pack);
        udpClient.Send(sendBuff, sendBuff.Length, SocketFlags.None);
    }
    
    
    
    
    //None
    
    // public class UdpState
    // {
    //     public UdpClient udpClient = null;
    //     public IPEndPoint ipEndPoint = null;
    //     public byte[] buffer=new byte[1024];
    //     public int counter = 0;
    // }
    //
    // public class AsyncUdpClient
    // {
    //     public static bool messageSent = false;
    //     //端口
    //     private const int listenPort = 6668;
    //     private const int remotePort = 6667;
    //     //定义节点
    //     private IPEndPoint localEP = null;
    //     private IPEndPoint remoteEP = null;
    //     //发送和接收socket
    //     private UdpClient udpReceive = null;
    //     private UdpClient udpSend = null;
    //     private UdpState udpSendState = null;
    //     private UdpState udpReceiveState = null;
    //     private int counter = 0;
    //     //异步状态同步
    //     private ManualResetEvent sendDone=new ManualResetEvent(false);
    //     private ManualResetEvent receiveDone=new ManualResetEvent(false);
    //
    //     public AsyncUdpClient()
    //     {
    //         //本机节点
    //         localEP=new IPEndPoint(IPAddress.Any, listenPort);
    //         //远程节点
    //         remoteEP=new IPEndPoint(IPAddress.Parse("47.112.246.218"), remotePort);
    //         //实例化
    //         udpReceive=new UdpClient(localEP);
    //         udpSend=new UdpClient();
    //         //实例化udpState
    //         udpSendState = new UdpState();
    //         udpSendState.ipEndPoint = remoteEP;
    //         udpSendState.udpClient = udpSend;
    //
    //         udpReceiveState = new UdpState();
    //         udpReceiveState.ipEndPoint = remoteEP;
    //         udpReceiveState.udpClient = udpReceive;
    //     }
    //
    //     public void SendMsg()
    //     {
    //         udpSend.Connect(remoteEP);
    //
    //         Byte[] sendBytes;
    //         for (int i = 0; i < 10; i++)
    //         {
    //             sendBytes = Encoding.UTF8.GetBytes(i.ToString());
    //             udpSend.BeginSend(sendBytes, sendBytes.Length, SendCallback, udpSendState);
    //             sendDone.WaitOne();
    //         }
    //         
    //     }
    //
    //     private void SendCallback(IAsyncResult iar)
    //     {
    //         UdpState udpState = iar.AsyncState as UdpState;
    //         if (iar.IsCompleted)
    //         {
    //             sendDone.Set();
    //         }
    //     }
    //
    //     public void ReceiveMsg()
    //     {
    //         while (true)
    //         {
    //             lock (this)
    //             {
    //                 udpReceive.BeginReceive(ReceiveCallback, udpReceiveState);
    //                 receiveDone.WaitOne();
    //                 Thread.Sleep(100);
    //             }
    //         }
    //     }
    //
    //     private void ReceiveCallback(IAsyncResult iar)
    //     {
    //         UdpState udpState = iar.AsyncState as UdpState;
    //         if (iar.IsCompleted)
    //         {
    //             Byte[] receiveBytes = udpState.udpClient.EndReceive(iar,ref udpReceiveState.ipEndPoint);
    //             Debug.Log(Encoding.UTF8.GetString(receiveBytes));
    //             receiveDone.Set();
    //         }
    //     }
    //}
}
