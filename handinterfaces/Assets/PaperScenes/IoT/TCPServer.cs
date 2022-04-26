using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    public String Host = "192.168.0.106";
    public int Port = 32419;

    private TcpListener listener = null;
    private TcpClient client = null;
    private NetworkStream ns = null;
    string msg;

    // Start is called before the first frame update
    void Awake()
    {
        listener = new TcpListener(Dns.GetHostEntry(Host).AddressList[1], Port);
        listener.Start();
        Debug.Log("[Server] is listening");

        if (listener.Pending())
        {
            client = listener.AcceptTcpClient();
            Debug.Log("[Server] Connected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (client == null)
        {
            if (listener.Pending())
            {
                client = listener.AcceptTcpClient();
                Debug.Log("[Server] Connected");
            }
            else
            {
                return;
            }
        }

        ns = client.GetStream();

        if ((ns != null) && (ns.DataAvailable))
        {
            StreamReader reader = new StreamReader(ns);
            msg = reader.ReadToEnd();
            Debug.Log("[Server] received: "+msg);
        }
    }

    private void OnApplicationQuit()
    {
        if (listener != null)
            listener.Stop();
            Debug.Log("[Server] stops listening");
    }
}
