using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class newTCPClient : MonoBehaviour {  	
	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread; 	
	#endregion  	
	[SerializeField]
	private lightswitch_i lightswitch_data;
	[SerializeField]
	private hand_pointer pointer_data;
	[SerializeField]
    private colorpalette_i colorpalette_data;
	[SerializeField]
    private GameObject light_1;
	[SerializeField]
    private GameObject light_2;
	[SerializeField]
    private GameObject light_3;
	private int pre_light_No=-1;
	private bool pre_isLightOn = false;
	private string pre_hue = "null";
	// Use this for initialization 	
	void Start () {
		ConnectToTcpServer();
	}  	
	// Update is called once per frame
	void Update () {         
		if (Input.GetKeyDown(KeyCode.Space)) {             
			SendMessage();         
		}
		if (pointer_data.light_No!=pre_light_No){
			SendMessage();   
			pre_light_No = pointer_data.light_No;
		}
		if (lightswitch_data.isLightOn!=pre_isLightOn){
			SendMessage();   
			pre_isLightOn = lightswitch_data.isLightOn;
		}
		if (colorpalette_data.hue!=pre_hue){
			SendMessage();   
			pre_hue = colorpalette_data.hue;
		}
           
	}  	
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();  		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}  	
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 		
		try { 			
			socketConnection = new TcpClient("192.168.0.122", 9999);  			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 						
						Debug.Log("[Client2] server message received as: " + serverMessage); 					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("[Client2] Socket exception: " + socketException);         
		}     
	}  	
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	private void SendMessage() {         
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
				string clientMessage = pointer_data.light_No+"$"+lightswitch_data.isLightOn+"$"+colorpalette_data.hue+"#"; 				
				//string clientMessage = 1+"$"+lightswitch_data.isLightOn+"$"+colorpalette_data.hue+"#"; 				
				
				//string clientMessage = "2$False$white#"; 				
				
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
				// Debug.Log("[Client2] Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("[Client2] Socket exception: " + socketException);         
		}     
	} 
}