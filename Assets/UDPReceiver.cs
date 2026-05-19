using UnityEngine;
using System;          // 关键：加上这行，解决IAsyncResult找不到的问题
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPReceiver : MonoBehaviour
{
    private UdpClient udp;
    private int port = 8888;

    void Start()
    {
        udp = new UdpClient(port);
        udp.BeginReceive(OnReceive, null);
        Debug.Log("✅ 数据接收器启动，监听端口：" + port);
    }

    void OnReceive(IAsyncResult ar)
    {
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
        byte[] bytes = udp.EndReceive(ar, ref ip);
        string json = Encoding.UTF8.GetString(bytes);

        Debug.Log("<color=green>📥 收到数据：</color>" + json);
        udp.BeginReceive(OnReceive, null);
    }

    void OnDestroy()
    {
        udp.Close();
    }
}