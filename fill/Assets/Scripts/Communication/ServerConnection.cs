using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour {
	NetworkClient myClient;

	// Create a client and connect to the server port
	public void SetupClient()
	{
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);     
		myClient.Connect("127.0.0.1", 8080);
	}

	// client function
	public void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");

		// validate the server
	}

	public void SendGameInfo(){
		
	}

	public ScoreSet ReceiveScoreInfo(){
		return null;
	}
}