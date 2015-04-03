using UnityEngine;
using System.Collections;
public class ServerNetwork : MonoBehaviour {
	private int port = 25000;
	private int playerCount = 10;
	private string _messageLog = "";
	public GameObject[] prefabs;
	public void Awake() {
		if (Network.peerType == NetworkPeerType.Disconnected)
			Network.InitializeServer(playerCount, port, false);
	}
	public void Start(){
		foreach (GameObject prefab in prefabs)
			Instantiate (prefab, Vector3.zero, Quaternion.identity);
	}
	public void Update() {
	}
	public void OnGUI() {
		if (Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(100, 100, 150, 25), "Server");
			GUI.Label(new Rect(100, 125, 150, 25), "Clients attached: " + Network.connections.Length);
			if (GUI.Button(new Rect(100, 150, 150, 25), "Quit server")) {
				Network.Disconnect();
				Application.Quit();
			}
			if (GUI.Button(new Rect(100, 175, 150, 25), "Send hi to client"))
				SendInfoToClient();
		}
		GUI.TextArea(new Rect(275, 100, 300, 300), _messageLog);
	}
	void OnPlayerConnected(NetworkPlayer player) {
		AskClientForInfo(player);
	}
	void AskClientForInfo(NetworkPlayer player) {
		networkView.RPC("SetPlayerInfo", player, player);
	}
	[RPC]
	void ReceiveInfoFromClient(string someInfo) {
		_messageLog += someInfo + "\n";
		string[] strarry=someInfo.Split(' ');
		if (strarry [0] == "c") {
			networkView.RPC ("ReceiveInfoFromServer", RPCMode.Others, someInfo.Substring(2));
		}
	}
	[RPC]
	void SendInfoToClient() {
		string someInfo = "Server: hello client";
		networkView.RPC("ReceiveInfoFromServer", RPCMode.Others, someInfo);
	}
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	// fix RPC errors
	[RPC]
	void SendInfoToServer() { }
	[RPC]
	void SetPlayerInfo(NetworkPlayer player) { }
	[RPC]
	void ReceiveInfoFromServer(string someInfo) { }
}

