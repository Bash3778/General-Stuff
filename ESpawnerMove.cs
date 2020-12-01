using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESpawnerMove : MonoBehaviour {
	[SerializeField] GameObject player;
	[SerializeField] float offset;
	[SerializeField] float upper = 0;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (player.transform.position.x + offset, player.transform.position.y + upper, player.transform.position.z + 20);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0f, 0f, 5f * Time.deltaTime);
	}
}
