using UnityEngine;
using System.Collections;
public class ClientNetwork : MonoBehaviour {
	public string serverIP = "127.0.0.1";
	public int port = 25000;
	public GameObject playerPrefab;
	private string _messageLog = "";
	private string mymessage = "";
	string someInfo = "";
	private NetworkPlayer _myNetworkPlayer;
	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if (GUI.Button(new Rect(300, 100, 150, 25), "Connect")) {
				Network.Connect(mymessage, port);
				mymessage="";
			}
		} else {
			if (Network.peerType == NetworkPeerType.Client) {
				if (GUI.Button(new Rect(150, 425, 150, 25), "Logout")){
					Network.Disconnect();
					Application.LoadLevel("myscene");
				}
				if (GUI.Button(new Rect(0, 425, 150, 25), "Send Message"))
					SendInfoToServer();
			}
		}
		GUI.TextArea(new Rect(0, 0, 300, 400), _messageLog);
		mymessage = GUI.TextField (new Rect(0, 400, 300, 25), mymessage);
	}
	[RPC]
	void SendInfoToServer(){
		someInfo = "c " + _myNetworkPlayer.guid + ": "+mymessage;
		networkView.RPC("ReceiveInfoFromClient", RPCMode.Server, someInfo);
	}
	[RPC]
	void SetPlayerInfo(NetworkPlayer player) {
		_myNetworkPlayer = player;
		someInfo = "Player setted";
		networkView.RPC("ReceiveInfoFromClient", RPCMode.Server, someInfo);
	}
	[RPC]
	void ReceiveInfoFromServer(string someInfo) {
		_messageLog += someInfo + "\n";
	}
	void OnConnectedToServer() {
		GameObject go = Network.Instantiate (playerPrefab, Vector3.zero, Quaternion.identity, 0) as GameObject;
		cam me = this.gameObject.AddComponent("cam") as cam;
		me.player = go;
		me.holder = go.transform.Find("cameraholder").gameObject;
	}
	void OnDisconnectedToServer() {
		_messageLog += "Disco from server" + "\n";
	}
	// fix RPC errors
	[RPC]
	void ReceiveInfoFromClient(string someInfo) { }
	[RPC]
	void SendInfoToClient(string someInfo) { }
}
