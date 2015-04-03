using UnityEngine;
using System.Collections;

public class ServerClientSelect : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject[] spawnerPrefabs;
	public void OnGUI() {
		if (GUI.Button(new Rect(100, 175, 150, 25), "Server")) {
			ServerNetwork net = this.gameObject.AddComponent("ServerNetwork") as ServerNetwork;
			net.prefabs = spawnerPrefabs;
			this.enabled=false;
		} else if (GUI.Button(new Rect(100, 250, 150, 25), "Client")) {
			ClientNetwork net = this.gameObject.AddComponent("ClientNetwork") as ClientNetwork ;
			net.playerPrefab = playerPrefab;
			this.enabled=false;
		}
	}
}
