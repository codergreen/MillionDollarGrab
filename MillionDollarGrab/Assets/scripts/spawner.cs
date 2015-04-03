using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {
	public GameObject prefab;
	public float droprate;
	float droptime;
	// Use this for initialization
	void Start () {
		droptime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup > droptime) {
			Network.Instantiate(prefab,new Vector3(Random.Range(0.0f,10.0f),10.0f,Random.Range(0.0f,10.0f)),Quaternion.identity,0);
			droptime=Time.realtimeSinceStartup+droprate;
		}
	}
}
