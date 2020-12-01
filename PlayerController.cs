using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {
	[SerializeField] GameObject system;
	[SerializeField] Button left;
	[SerializeField] Button right;
	[SerializeField] Button CenterAttack;
	[SerializeField] Button RightAttack;
	[SerializeField] Button LeftAttack;
	[SerializeField] Animator anim;
	[SerializeField] GameObject shoot;
	[SerializeField] GameObject shoots;
	[SerializeField] GameObject FrontSpawn;
	[SerializeField] GameObject RightSpawn;
	[SerializeField] GameObject LeftSpawn;
	[SerializeField] GameObject control;
	[SerializeField] Color rcolor;
	[SerializeField] Color lcolor;
	[SerializeField] Color ccolor;
	[SerializeField] float speed = 1;
	[SerializeField] float offset = 2;
	[SerializeField] float soffset = 1;
	int pos = 0;
	[SerializeField] bool rightFire = false;
	[SerializeField] bool leftFire = false;
	[SerializeField] bool centerFire = false;
	// Use this for initialization
	void Start () {
		left.onClick.AddListener (leftb);
		right.onClick.AddListener (rightb);
		CenterAttack.onClick.AddListener (centerb);
		RightAttack.onClick.AddListener (rightab);
		LeftAttack.onClick.AddListener (leftab);
	}
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (0f, 0f, 5f * Time.deltaTime);
		buttonAnim (true);
		buttonAnim (false);
	}
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			system.transform.position = transform.position;
			ParticleSystem sys = system.GetComponent<ParticleSystem> ();
			sys.Play ();
			other.gameObject.SetActive (false);
			gameObject.SetActive (false);
			left.gameObject.SetActive (false);
			right.gameObject.SetActive (false);
			CenterAttack.gameObject.SetActive (false);
			LeftAttack.gameObject.SetActive (false);
			RightAttack.gameObject.SetActive (false);
			Pause pause = control.GetComponent<Pause> ();
			pause.dead = true;
		}
	}
	void leftb () {
		if (pos == 0 || pos == 1) {
			transform.Translate (-3f, 0f, 0f); 
			pos = pos - 1;
		}
	} 
	void rightb () {
		if (pos == -1|| pos == 0) {
			transform.Translate(3f, 0f, 0f);
			pos = pos + 1;
		}
	}
	void centerb () {
		shooting ("front");
	}
	void rightab () {
		shooting ("right");
	}
	void leftab () {
		shooting ("left");
	}
	void buttonAnim(bool side) {
		if (side == true) {
			LeftAttack.image.color = lcolor;
			RightAttack.image.color = rcolor;
			if (lcolor.a >= 0.9) {
				lcolor.a = 1;
				leftFire = true;
			} else {
				lcolor.a += speed * Time.deltaTime / soffset;
			}
			if (rcolor.a >= 0.9) {
				rcolor.a = 1;
				rightFire = true;
			} else {
				rcolor.a += speed * Time.deltaTime / soffset;
			}
		} else {
			CenterAttack.image.color = ccolor;
			if (ccolor.a >= 0.9) {
				ccolor.a = 1;
				centerFire = true;
			} else {
				ccolor.a += speed * Time.deltaTime / offset;
			}
		}
	}
	void shooting (string side) {
		if (side == "front" && centerFire == true) {
			anim.Play ("FrontAttack");
			GameObject obj = (GameObject)Instantiate (shoot);
			obj.transform.position = FrontSpawn.transform.position;
			obj.transform.rotation = FrontSpawn.transform.rotation;
			obj.SetActive (true);
			centerFire = false;
			ccolor.a = 0;
		} else if (side == "right" && rightFire == true) {
			anim.Play ("SideAttack");
			transform.Translate (-0.10f, 0f, 0f);
			GameObject obj = (GameObject)Instantiate (shoots);
			obj.transform.position = RightSpawn.transform.position;
			obj.transform.rotation = RightSpawn.transform.rotation;
			obj.SetActive (true);
			rightFire = false;
			rcolor.a = 0;
		} else if (side == "left" && leftFire == true) {
			anim.Play ("SideAttack");
			transform.Translate (0.10f, 0f, 0f);
			GameObject obj = (GameObject)Instantiate (shoots);
			obj.transform.position = LeftSpawn.transform.position;
			obj.transform.rotation = LeftSpawn.transform.rotation;
			obj.SetActive (true);
			leftFire = false;
			lcolor.a = 0;
		}
	}
}