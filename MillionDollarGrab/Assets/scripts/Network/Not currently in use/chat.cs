using UnityEngine;
using System.Collections.Generic;

public class chat : MonoBehaviour 
{
	public List<string> chatHistory = new List<string> ();
	public GameObject playerPrefab;
	private string currentMessage = string.Empty;
	public string connectionIP = "127.0.0.1";
	public int portNumber = 8632;
	private bool connected = false;

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
		GUILayout.BeginHorizontal (GUILayout.Width (250));
		currentMessage = GUILayout.TextField (currentMessage);
		if (GUILayout.Button ("send")) {
			if (!string.IsNullOrEmpty (currentMessage.Trim ())) {
				//n.RPC ("ChatMessage", RPCMode.AllBuffered, new object[] {currentMessage });
				currentMessage = string.Empty;
			}
		}
		GUILayout.EndHorizontal ();
		foreach (string c in chatHistory){
			GUILayout.Label(c);
		}
	}
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

	[RPC]
	public void ChatMessage(string message)
	{
		chatHistory.Add(message);
	}
}