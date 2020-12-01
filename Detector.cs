using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {
	public bool hit = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "Player") {
			hit = true;
		}
	}
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag != "Player") {
			hit = false;
		}
	}
}
