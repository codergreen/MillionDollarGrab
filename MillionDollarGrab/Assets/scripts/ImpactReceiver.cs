﻿using UnityEngine;

using System.Collections;

public class ImpactReceiver : MonoBehaviour {
	Vector3 impact = Vector3.zero;
	private CharacterController character;
	// Use this for initialization
	void Start () {
		character = GetComponent<CharacterController>();
	}
	// Update is called once per frame
	void Update () {
		// apply the impact force:
		if (impact.magnitude > 0.02F) character.Move(impact * Time.deltaTime);
		// consumes the impact energy each cycle:
		impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
	}
	// call this function to add an impact force:
	public void AddImpact(Vector3 dir){
		if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
		impact += dir;
	}
}