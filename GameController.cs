using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {
	[SerializeField] GameObject[] outerBases = new GameObject[8];
	[SerializeField] int[] pos;
	[SerializeField] GameObject camera;
	[SerializeField] GameObject buttoner;
	[SerializeField] int offset;
	[SerializeField] float yoffset;
	[SerializeField] float speed;
	[SerializeField] float seconds;
	[SerializeField] float divide;
	[SerializeField] Vector3 launchOffset;
	[SerializeField] Vector3 PmoverOffset;
	[SerializeField] bool launchDone = false;
	[SerializeField] Slider speeder;
	[SerializeField] Slider rotator;
	[SerializeField] float speedConst;
	[SerializeField] float rotateConst;
	[SerializeField] float speedDivisor;
	[SerializeField] float rotateDivisor;
	[SerializeField] float rotateSpeedPow;
	[SerializeField] GameObject treeObject;
	[SerializeField] GameObject bullet;
	[SerializeField] float bulletSpeed;
	[SerializeField] GameObject deadEffect;
	[SerializeField] GameObject cityEffect;
	[SerializeField] GameObject city;
	[SerializeField] GameObject outText;
	[SerializeField] GameObject outMost;
	[SerializeField] GameObject beacon;
	[SerializeField] GameObject firedText;
	public bool impact = false;
	public Material[] colors;
	public GameObject enemy;
	public Vector3 playerPosition;
	public bool dead = false;
	public bool won = false;
	public bool destroy = false;
	Vector3 spot;
	int side;
	int ier;
	public bool close = false;
	float hypotnuse;
	void beginner (int xoff, int zoff, int sider, int j) {
		Vector3 posit = new Vector3 (outerBases [j].transform.position.x + xoff, outerBases [j].transform.position.y + yoffset, outerBases [j].transform.position.z + zoff);
		camera.transform.position = posit;
		Vector3 rotat = new Vector3 (camera.transform.rotation.eulerAngles.x, pos [j] * 90, camera.transform.rotation.eulerAngles.z);
		camera.transform.rotation = Quaternion.Euler (rotat);
		side = sider;
	}
	void camController () {
		Camera cam = camera.GetComponent<Camera> ();
		for (int i = 0; i < 3; i++) {
			cam.farClipPlane = cam.farClipPlane * Mathf.Pow (10, i);
		}
	}
	void Start () {
		camController ();
		if (PlayerPrefs.GetInt ("treeSetting") == 1) {
			treeObject.SetActive (true);
		} else {
			treeObject.SetActive (false);
		}
		int rand = Random.Range (0, 8);
		for (int i = 0; i < outerBases.Length; i++) {
			Animator anim = outerBases [i].GetComponent<Animator> ();
			if (rand == i) {
				ier = i;
				spot = outerBases [i].transform.position;
				if (pos [i] == 0) {
					beginner (0, -1 * offset, pos [i], i);
					StartCoroutine (timer (0f, launchOffset.z));
				}
				if (pos [i] == 1) {
					beginner (-1 * offset, 0, pos [i], i);
					StartCoroutine (timer (launchOffset.x, 0f));
				}
				if (pos [i] == 2) {
					beginner (0, offset, pos [i], i);
					StartCoroutine (timer (0f, launchOffset.z * -1));
				}
				if (pos [i] == 3) {
					beginner (offset, 0, pos [i], i);
					StartCoroutine (timer (-1 * launchOffset.x, 0f));
				}
			} else {
				anim.enabled = false;
			}
		}
		//Debug.Log (ier);
	}
	void beaconController () {
		if (PlayerPrefs.GetInt ("beaconSetting") == 1 && !won && !destroy && !dead) {
			beacon.SetActive (true);
			beacon.transform.position = enemy.transform.position;
		} else {
			beacon.SetActive (false);
		}
	}
	void destroyer () {
		float dister;
		dister = PlayerPrefs.GetFloat ("bulletDistance");
		if (Vector3.Distance (outerBases [ier].transform.position, enemy.transform.position) < dister && !close) {
			impact = true;
			close = true;
		} 
		if (Vector3.Distance (outerBases [ier].transform.position, enemy.transform.position) > dister && !impact) {
			close = false;
		}
		if (impact) {
			firedText.SetActive (true);
		} else {
			firedText.SetActive (false);
		}
		if (!impact) {
			bullet.gameObject.SetActive (false);
			bullet.transform.position = outerBases [ier].transform.position;
		} 
		if (impact && close) {
			bullet.gameObject.SetActive (true);
			Vector3 displace = new Vector3 (Vector2.Distance (new Vector2 (bullet.transform.position.x, 0), new Vector2 (enemy.transform.position.x, 0)), Vector2.Distance (new Vector2 (bullet.transform.position.y, 0), new Vector2 (enemy.transform.position.y, 0)), Vector2.Distance (new Vector2 (bullet.transform.position.z, 0), new Vector2 (enemy.transform.position.z, 0)));
			float total = displace.x + displace.y + displace.z;
			float xsped = bulletSpeed * (displace.x / total);
			float ysped = bulletSpeed * (displace.y / total);
			float zsped = bulletSpeed * (displace.z / total);
			if (bullet.transform.position.x > enemy.transform.position.x) {
				xsped = xsped * -1;
			}
			if (bullet.transform.position.y > enemy.transform.position.y) {
				ysped = ysped * -1;
			}
			if (bullet.transform.position.z > enemy.transform.position.z) {
				zsped = zsped * -1;
			}
			bullet.transform.Translate (new Vector3 (xsped, ysped, zsped));
		}
	}
	IEnumerator timer (float offx, float offz) {
		Animator anim = outerBases [ier].GetComponent<Animator> ();
		Vector3 offseter = new Vector3 (offx, launchOffset.y, offz);
		PlayerController play = outerBases [ier].GetComponent<PlayerController> ();
		Vector3 rot = new Vector3 (outerBases [ier].transform.rotation.eulerAngles.x - 90, outerBases [ier].transform.rotation.eulerAngles.y +  90, outerBases [ier].transform.rotation.eulerAngles.z + 90);
		play.starting = rot;
		if (ier == 0 || ier == 1) {
			anim.Play ("Launch");
			anim.SetBool ("Launcher", false);
			yield return new WaitForSeconds (seconds);
			anim.enabled = false;
		} else {
			anim.Play ("Launch");
			anim.SetBool ("Launcher", false);
			yield return new WaitForSeconds (seconds);
			anim.enabled = false;
		}
		launchDone = true;
		outerBases [ier].transform.position = offseter + spot;
		outerBases [ier].transform.rotation = Quaternion.Euler (rot);
		outerBases [ier].transform.localScale = new Vector3 (200f, 200f, 200f);
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < outerBases.Length; i++) {
			if (i == ier) {
				PlayerController player = outerBases [i].GetComponent<PlayerController> ();
				HUDcontroller huder = buttoner.GetComponent<HUDcontroller> ();
				GameController controla = gameObject.GetComponent<GameController> ();
				player.inUse = true;
				player.reviser = huder.revised;
				player.director = huder.direction;
				player.speeder = speed;
				player.posits = ier;
				player.refrence = controla;
				player.divisor = divide;
				player.moverOffset = PmoverOffset;
				player.launcherDone = launchDone;
				player.speedDistance = Mathf.Pow (Vector2.Distance (speeder.fillRect.position, speeder.handleRect.position) / speedDivisor, rotateSpeedPow); 
				player.rotateDistance = Mathf.Pow (Vector2.Distance (rotator.fillRect.position, rotator.handleRect.position) / rotateDivisor, rotateSpeedPow);
				player.spedConst = speedConst;
				player.rotatConst = rotateConst;
				player.script = this.gameObject;
				playerPosition = outerBases [i].transform.position;
			}
		}
		beaconController ();
		if (Vector3.Distance (outerBases [ier].transform.position, city.transform.position) > Vector3.Distance (city.transform.position, outMost.transform.position)) {
			outText.SetActive (true);
		} else {
			outText.SetActive (false);
		}
		if (dead) {
			deadEffect.transform.position = outerBases [ier].transform.position;
			deadEffect.SetActive (true);
		}
		if (destroy) {
			cityEffect.transform.position = city.transform.position;
			cityEffect.SetActive (true);
		}
		if (won) {
			deadEffect.transform.position = enemy.transform.position;
			deadEffect.SetActive (true);
		}
	}

	void targeter (int xoff, int zoff) {
		Vector3 target;
		float h2d;
		float xpos;
		float zpos;
		float angle = (outerBases [ier].transform.rotation.eulerAngles.y % 90) * Mathf.Deg2Rad;
		if (launchDone) {
			//Horizontal Axis
			if (xoff != 0) {
				h2d = Mathf.Abs (xoff);
			} else {
				h2d = Mathf.Abs (zoff);
			}
			if (outerBases [ier].transform.rotation.eulerAngles.y >= 90 && outerBases [ier].transform.rotation.eulerAngles.y < 180) {
				xpos = Mathf.Cos (angle) * h2d;
				zpos = Mathf.Sin (angle) * h2d;
			} else if (outerBases [ier].transform.rotation.eulerAngles.y >= 270 && outerBases [ier].transform.rotation.eulerAngles.y < 360) {
				xpos = Mathf.Cos (angle) * h2d;
				zpos = Mathf.Sin (angle) * h2d;
			} else {
				xpos = Mathf.Sin (angle) * h2d;
				zpos = Mathf.Cos (angle) * h2d;
			}

			if (outerBases [ier].transform.rotation.eulerAngles.y > 90 && outerBases [ier].transform.rotation.eulerAngles.y < 270) {
				zpos = zpos * -1;
			}
			if (outerBases [ier].transform.rotation.eulerAngles.y > 0 && outerBases [ier].transform.rotation.eulerAngles.y < 180) {
				xpos = xpos * -1;
			}
			target = new Vector3 (outerBases [ier].transform.position.x + xpos , outerBases [ier].transform.position.y + yoffset, outerBases [ier].transform.position.z - zpos);
			camera.transform.rotation = Quaternion.Euler (new Vector3 (camera.transform.rotation.eulerAngles.x, outerBases [ier].transform.rotation.eulerAngles.y, camera.transform.rotation.eulerAngles.z));
		} else {
			target = new Vector3 (outerBases [ier].transform.position.x + xoff, outerBases [ier].transform.position.y + yoffset, outerBases [ier].transform.position.z + zoff);
		}
		camera.transform.position = Vector3.Lerp (camera.transform.position, target, 5f);
	}

	void FixedUpdate() {
		destroyer ();
		if (side == 0) {
			targeter (0, -1 * offset);
		}
		if (side == 1) {
			targeter (-1 * offset, 0);
		}
		if (side == 2) {
			targeter (0, offset);
		} 
		if (side == 3) {
			targeter (offset, 0);
		}
		//camera.transform.position = Vector3.Lerp (camera.transform.position, new Vector3 (enemy.transform.position.x, enemy.transform.position.y + 25f, enemy.transform.position.z - 25), 5f);
	}
}