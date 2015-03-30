using UnityEngine;
using System.Collections;

public class NetworkMenu : MonoBehaviour 
{
	public string connectionIP = "127.0.0.1";
	public int portNumber = 8632;
	
	private bool connected = false;
	
	private void OnConnectedToServer()
	{
		// A client has just connected
		connected = true;
	}
	
	private void OnServerInitialized()
	{
		// The server has initialized
		connected = true;
	}
	
	private void OnDisconnectedFromServer()
	{
		// The connection has been lost 
		connected = false;
	}
	
	private void OnGUI()
	{
		
		
		if (!connected) 
		{
			connectionIP = GUILayout.TextField (connectionIP);
			int.TryParse (GUILayout.TextField (portNumber.ToString ()), out portNumber);
			
			if (GUILayout.Button ("Connect"))
				Network.Connect (connectionIP, portNumber);
			
			if (GUILayout.Button ("Host"))
				Network.InitializeServer (4, portNumber, true);
		}
		else 
		{
			GUILayout.Label("Connections: " + Network.connections.Length.ToString());
		}
		
	}
}