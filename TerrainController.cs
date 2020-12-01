using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {
	[SerializeField] GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(player.transform.position.z - transform.position.z) > 1000) {
		} else {
			gameObject.SetActive (true);
		}
		if (Mathf.Abs(player.transform.position.z - transform.position.z) < 1000) {
		} else {
			gameObject.SetActive (false);
		}
	}
}
