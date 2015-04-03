﻿using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {
	public float timer;
	public GameObject cc;
	// Use this for initialization
	void Start () {
		timer += Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup > timer) {
			Collider[] cs = Physics.OverlapSphere (this.transform.position, 1.5f);
			foreach(Collider hit in cs){
				if (hit.rigidbody){
					if((hit.transform.position-this.transform.position).magnitude>=0.01||(hit.transform.position-this.transform.position).magnitude<=-0.01)
						hit.rigidbody.AddForce(1000*(hit.transform.position-this.transform.position)/(hit.transform.position-this.transform.position).magnitude);
				}
				else{
					ImpactReceiver i = hit.gameObject.GetComponent<ImpactReceiver>();
					if(i!=null){
						if((hit.transform.position-this.transform.position).magnitude>=0.01||(hit.transform.position-this.transform.position).magnitude<=-0.01)
							i.AddImpact(100*(hit.transform.position-this.transform.position)/(hit.transform.position-this.transform.position).magnitude);
					}
				}
			}
			if(Network.isServer)
				Network.Destroy(cc);
		}
	}
}