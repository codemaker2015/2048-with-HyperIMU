using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPReceiver : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client;
    public int port;
    public string lastReceivedUDPPacket = "";
    public float rot;
    public string result;

    public static UDPReceiver instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private static void Main()
    {
        UDPReceiver receiveObj = new UDPReceiver();
        receiveObj.Init();
    }

    public void Start()
    {
        port = 5009;
        Init();
    }

    private void Init()
    {
        port = 5009;
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    public void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                Debug.Log("Received Data from IMU");
                string text = Encoding.UTF8.GetString(data);
                Debug.Log(text);
                lastReceivedUDPPacket = text;
                result = text;
                Debug.Log(result);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }


    void OnApplicationQuit()
    {
        stopThread();
    }

    private void stopThread()
    {
        if (receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        client.Close();
    }
}

