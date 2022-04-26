using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;


public class TCPClient : MonoBehaviour
{
    public String Host = "192.168.0.243";
    public Int32 Port = 9999;

    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    private Thread clientSendThread;
    private Thread clientReceiveThread;
    [SerializeField]
    private joystick_i joystick_data;
    

    // Start is called before the first frame update
    void Start()
    {
        mySocket = new TcpClient();

        // if (SetupSocket())
        // {
        //     Debug.Log("[Client] socket is set up");
        // }
        // mySocket.Connect(Host, Port);
        clientReceiveThread = new Thread(new ThreadStart(ListenForData));
        clientReceiveThread.IsBackground = true;
        clientReceiveThread.Start();
        clientSendThread = new Thread(new ThreadStart(SetupSocket));
        clientSendThread.IsBackground = true;
        clientSendThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mySocket.Connected)
        {
            SetupSocket();
        }
        else 
        {
            // Debug.Log("send msg"+mySocket.Connected);
            if (Input.GetKeyDown("a"))
            {
                SendMessage();
                Debug.Log("[Client] A click");
            }
        }
    }
    public bool SendMessage()
    {
        try
        {
            // if (!mySocket.Connected)
            // {
            //    mySocket.Connect(Host, Port);
            // }
            // mySocket.Connect(Host, Port);
            // theStream = mySocket.GetStream();
            // theWriter = new StreamWriter(theStream);
            string clientMessage = "joystick motion:" + " x "+joystick_data.pan +" z" +joystick_data.tilt;            
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(clientMessage);
            theStream.Write(sendBytes, 0, sendBytes.Length);
            Debug.Log("[Client] msg is sent");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("[Client] Socket error: " + e);
            return false;
        }
    }

    public void SetupSocket()
    {
        try
        {
            Debug.Log("[Client] Client tries sending message");
            // // Get a stream object for writing. 			
            // if (theStream.CanWrite)
            // {
            //     // theStream = mySocket.GetStream();
            //     // string clientMessage = "joystick motion:" + " x "+joystick_data.servo1 +" z" +joystick_data.servo2;           
            //     // Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(clientMessage);
            //     // theStream.Write(sendBytes, 0, sendBytes.Length);
            //     // Debug.Log("[Client] socket is sent"+clientMessage);
            // //     // TODO use bitconverter to convert floats to bytes and stream directly
            // //     string clientMessage = "joystick motion:" + " x "+joystick_data.diff.x +" z" +joystick_data.diff.z;
            // //     byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
            // //     // Write byte array to socketConnection stream.
            // //     stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            // }
            theStream = mySocket.GetStream();
            string clientMessage = "test";           
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(clientMessage);
            theStream.Write(sendBytes, 0, sendBytes.Length);
            Debug.Log("[Client] socket is sent"+clientMessage);
        }
        catch (Exception e)
        {
            Debug.Log("[Client] Socket error: " + e);
        }
    }
    private void ListenForData()
    {
        try
        {
            Debug.Log("[Client] Before connecting to server");
            mySocket = new TcpClient(Host, Port); //make sure server is under CMU-Secure wifi
            Debug.Log("[Client] After connecting to server");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (theStream = mySocket.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = theStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("[Client] server message received as: " + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("[Client] Socket exception: " + socketException);
        }
    }

    private void OnApplicationQuit()
    {
        if (mySocket != null && mySocket.Connected)
            mySocket.Close();
    }
}
// public class TCPClient: MonoBehaviour
// {
//     #region private members 	
//     private TcpClient socketConnection;
//     private Thread clientReceiveThread;
//     private Thread clientSendThread;
//     public String Host = "localhost";
//     private int Port = 445;
//     #endregion
//     [SerializeField]
//     private joystick_i joystick_data;
//     // Use this for initialization 	
//     void Start()
//     {
//         ConnectToTcpServer();
//     }
//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             SendMessage();
//             Debug.Log("[Client] space bar click");
//         }
//     }
//     private void OnApplicationQuit()
//     {
//         socketConnection.Close();
//     }
//     /// <summary> 	
//     /// Setup socket connection. 	
//     /// </summary> 	
//     private void ConnectToTcpServer()
//     {
//         try
//         {
//             clientReceiveThread = new Thread(new ThreadStart(ListenForData));
//             clientReceiveThread.IsBackground = true;
//             clientReceiveThread.Start();
//             clientSendThread = new Thread(new ThreadStart(StreamData));
//             clientSendThread.IsBackground = true;
//             clientSendThread.Start();
//         }
//         catch (Exception e)
//         {
//             Debug.Log("[Client] On client connect exception " + e);
//         }
//     }
//     private void StreamData()
//     {
//         while (socketConnection == null)
//         {
//             Thread.Sleep(100);
//         }
//         while (true)
//         {
//             Thread.Sleep(1);
//             try
//             {
//                 Debug.Log("[Client] Client tries sending message");
//                 // Get a stream object for writing. 			
//                 NetworkStream stream = socketConnection.GetStream();
//                 if (stream.CanWrite)
//                 {
//                     // TODO use bitconverter to convert floats to bytes and stream directly
//                     string clientMessage = "joystick motion:" + " x "+joystick_data.diff.x +" z" +joystick_data.diff.z;
//                     byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
//                     // Write byte array to socketConnection stream.
//                     stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
//                 }
//             }
//             catch (SocketException socketException)
//             {
//                 Debug.Log("[Client] Socket exception: " + socketException);
//             }
//         }
//     }
//     /// <summary> 	
//     /// Runs in background clientReceiveThread; Listens for incomming data. 	
//     /// </summary>
//     private void ListenForData()
//     {
//         try
//         {
//             Debug.Log("[Client] Before connecting to server");
//             socketConnection = new TcpClient(Host, Port); //make sure server is under CMU-Secure wifi
//             Debug.Log("[Client] After connecting to server");
//             Byte[] bytes = new Byte[1024];
//             while (true)
//             {
//                 // Get a stream object for reading 				
//                 using (NetworkStream stream = socketConnection.GetStream())
//                 {
//                     int length;
//                     // Read incomming stream into byte arrary. 					
//                     while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
//                     {
//                         var incommingData = new byte[length];
//                         Array.Copy(bytes, 0, incommingData, 0, length);
//                         // Convert byte array to string message. 						
//                         string serverMessage = Encoding.ASCII.GetString(incommingData);
//                         Debug.Log("[Client] server message received as: " + serverMessage);
//                     }
//                 }
//             }
//         }
//         catch (SocketException socketException)
//         {
//             Debug.Log("[Client] Socket exception: " + socketException);
//         }
//     }
//     /// <summary> 	
//     /// Send message to server using socket connection. 	
//     /// </summary> 	
//     private void SendMessage()
//     {
//         if (socketConnection == null)
//         {
//             return;
//         }
//         try
//         {
//             Debug.Log("[Client] Client tries sending message");
//             // Get a stream object for writing. 			
//             NetworkStream stream = socketConnection.GetStream();
//             if (stream.CanWrite)
//             {
//                 string clientMessage = "test";
//                 // Convert string message to byte array.
//                 byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
//                 // Write byte array to socketConnection stream.
//                 stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
//                 Debug.Log("[Client] Client sent his message");
//             }
//         }
//         catch (SocketException socketException)
//         {
//             Debug.Log("[Client] Socket exception: " + socketException);
//         }
//     }
// }