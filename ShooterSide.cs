using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShooterSide : MonoBehaviour {
	// Use this for initialization
	[SerializeField] GameObject system;
	[SerializeField] GameObject contoll;
	[SerializeField] float side = -8;
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (2f * Time.deltaTime, -5f * Time.deltaTime, side * Time.deltaTime);
	}

	void OnTriggerEnter (Collider other) {
		ScoreController score = contoll.GetComponent<ScoreController> ();
		score.tscore++;
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
