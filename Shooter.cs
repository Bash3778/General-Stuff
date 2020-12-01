using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shooter : MonoBehaviour {
	// Use this for initialization
	[SerializeField] GameObject system;
	[SerializeField] GameObject contoll;
	[SerializeField] float forward = -10;
	[SerializeField] float down = -5;
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (0f, down * Time.deltaTime, forward * Time.deltaTime);
	}

	void OnTriggerEnter (Collider other) {
		ScoreController score = contoll.GetComponent<ScoreController> ();
		score.tscore+=2;
		GameObject obj = (GameObject)Instantiate (system);
		obj.transform.position = transform.position;
		ParticleSystem sys = obj.GetComponent<ParticleSystem> ();
		if (other.tag == "Enemy") {
			other.gameObject.SetActive (false);
		}
		gameObject.SetActive (false);
		sys.Play ();
	}
}
