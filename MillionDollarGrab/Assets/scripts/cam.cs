using UnityEngine;
using System.Collections;

public class cam : MonoBehaviour {
	public GameObject player;
	public GameObject holder;
	Vector3 offset;
	// Use this for initialization
	void Start() {
		offset = new Vector3(0,0,1);
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<move> ().toggle) {
			offset = new Vector3 (0, 0, -1);
			float desiredAngle = player.transform.eulerAngles.y;
			Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
			transform.position = player.transform.position - (rotation * offset);
			transform.LookAt(player.transform);
		} else {
			this.transform.position = holder.transform.position;
			this.transform.rotation = holder.transform.rotation;

		}
	}
}
