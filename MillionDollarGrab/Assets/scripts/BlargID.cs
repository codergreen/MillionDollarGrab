using UnityEngine;
using System.Collections;

public class BlargID : MonoBehaviour {
	public static int id = 0;
	public int myid;
	// Use this for initialization
	void Start () {
		++id;
		myid = id;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
