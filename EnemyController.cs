using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	[SerializeField] GameObject managerGameobject;
	[SerializeField] Transform[] probes;
	[SerializeField] Transform distSetter;
	[SerializeField] Transform center;
	[SerializeField] float maximumy;
	[SerializeField] float adjust;
	[SerializeField] Transform city;
	[SerializeField] float smoothing;
	[SerializeField] float speed;
	[SerializeField] float rayDist = 100;
	[SerializeField] GameObject ball;
	[SerializeField] Transform outerEdge;
	[SerializeField] float maxSpeed;
	[SerializeField] float minSpeed;
	[SerializeField] GameObject hitEffect;
	public int hits;
	GameController manager;
	int placeHolder = 0;
	float dister;
	float maximum;
	static int probeNumber = 690;
	Detector[] scripts;
	Vector3[] pointsList;
	[SerializeField] Vector3[] direction;
	[SerializeField] Transform[] points;
	public int pointsPosition = -1;
	GameObject[] balls = new GameObject[probeNumber];
	Vector3[] transformer = new Vector3[probeNumber];
	GameObject[] effects = new GameObject[5];
	void StartingPosition () {
		maximum = Vector3.Distance (center.position, distSetter.position);
		int negPos = Random.Range (1, 3);
		if (negPos <= 1.5) {
			dister = -1 * maximum;
		} else {
			dister = maximum;
		}
		Vector3 spos; 
		Vector3 rotate = new Vector3 (0, 0, 0);
		int xz = Random.Range (1, 3);
		if (xz <= 1.5) {
			spos = new Vector3 (Random.Range (-1 * maximum, maximum), maximumy, dister);
			if (dister < 0) {
				rotate.y = 0f;
			} else {
				rotate.y = 180f;
			}

		} else {
			spos = new Vector3 (dister, maximumy, Random.Range (-1 * maximum, maximum));
			if (dister < 0) {
				rotate.y = 90f;
			} else {
				rotate.y = 270f;
			}
		}
		transform.position = spos;
		transform.rotation = Quaternion.Euler (rotate);
	}
	void probeStarter () {
		scripts = new Detector [probes.Length];
		direction = new Vector3 [probes.Length];
		for (int i = 0; i < scripts.Length; i++) {
			Detector script = probes [i].gameObject.GetComponent<Detector> ();
			Collider detect = probes [i].gameObject.GetComponent<Collider> ();
			scripts [i] = script;
			float distz = Vector2.Distance (new Vector2 (transform.position.z, 0f), new Vector2 (detect.bounds.center.z, 0f));
			float disty = Vector2.Distance (new Vector2 (transform.position.y, 0f), new Vector2 (detect.bounds.center.y, 0f));
			float distx = Vector2.Distance (new Vector2 (transform.position.x, 0f), new Vector2 (detect.bounds.center.x, 0f));
			direction [i] = new Vector3 (distx * adjust, disty * adjust, distz * adjust);
		}
	}
	void enemyMover () {
		float xdist = Vector2.Distance (new Vector2 (transform.position.x, 0), new Vector2 (pointsList [pointsPosition].x, 0));
		float ydist = Vector2.Distance (new Vector2 (transform.position.y, 0), new Vector2 (pointsList [pointsPosition].y, 0));
		float zdist = Vector2.Distance (new Vector2 (transform.position.z, 0), new Vector2 (pointsList [pointsPosition].z, 0));
		float xzdist = Vector2.Distance (new Vector2 (transform.position.x, transform.position.z), new Vector2 (pointsList [pointsPosition].x, pointsList [pointsPosition].z));
		Vector3[] destinations = new Vector3[probeNumber];
		float[] hits = new float[probeNumber];
		float[] angles = new float[probeNumber];
		float[] anglesx = new float[probeNumber];
		int[] works = new int[probeNumber];
		int index = 0;
		int index2 = 0;
		float lowest = 360;
		float lowestx = 360;
		float objectiveX = 0;
		float objectiveY = 0;
		for (int i = 0; i < probeNumber; i++) {
			hits [i] = 360;
			destinations [i] = Vector3.zero;
		}
		if (Vector3.Distance(transform.position, pointsList[pointsPosition]) < 100) {
			pointsPosition++;
		}
		if (pointsPosition == PlayerPrefs.GetInt("wayNumber")) {
			manager.destroy = true;
			gameObject.SetActive (false);
		} 
		if (pointsPosition < PlayerPrefs.GetInt("wayNumber")) {
			if (transform.position.x < pointsList [pointsPosition].x && transform.position.z < pointsList [pointsPosition].z) {
				objectiveY = Mathf.Atan2 (zdist, xdist) * Mathf.Rad2Deg; 
			}
			if (transform.position.x < pointsList [pointsPosition].x && transform.position.z > pointsList [pointsPosition].z) {
				objectiveY = Mathf.Atan2 (zdist, xdist) * Mathf.Rad2Deg + 90;
			}
			if (transform.position.x > pointsList [pointsPosition].x && transform.position.z > pointsList [pointsPosition].z) {
				objectiveY = Mathf.Atan2 (zdist, xdist) * Mathf.Rad2Deg + 180;
			}
			if (transform.position.x > pointsList [pointsPosition].x && transform.position.z < pointsList [pointsPosition].z) {
				objectiveY = Mathf.Atan2 (zdist, xdist) * Mathf.Rad2Deg + 270;
			} 
			if (transform.position.y < pointsList [pointsPosition].y) {
				objectiveX = Mathf.Atan2 (ydist, xzdist) * Mathf.Rad2Deg * -1;
			}
			if (transform.position.y >= pointsList [pointsPosition].y) {
				objectiveX = Mathf.Atan2 (ydist, xzdist) * Mathf.Rad2Deg;
			} 
		}
		for (int i = 0; i < 46; i++) {
			for (int j = 0; j < 15; j++) {
				Vector3 dot;
				float angle = 90 - (i * 4);
				float anglex = -90 + (j * 12);
				float overallA = transform.rotation.eulerAngles.y + angle;
				float angler = ((transform.rotation.eulerAngles.y + angle) % 90) * Mathf.Deg2Rad;
				float xoff = rayDist * Mathf.Sin (angler);
				float zoff = rayDist * Mathf.Cos (angler);
				float anglerx = (anglex % 90) * Mathf.Deg2Rad;
				float yoff = rayDist * Mathf.Sin (anglerx);
				float xzoff = rayDist * Mathf.Cos (anglerx);
				if (overallA > 360) {
					overallA = overallA - 360;
				}
				if (overallA > 90 && overallA <= 180) {
					xoff = rayDist * Mathf.Cos (angler);
					zoff = rayDist * Mathf.Sin (angler) * -1;
				}
				if (overallA > 180 && overallA <= 270) {
					xoff = rayDist * Mathf.Sin (angler) * -1;
					zoff = rayDist * Mathf.Cos (angler) * -1;
				}
				if (overallA > 270 && overallA < 360) {
					xoff = rayDist * Mathf.Cos (angler) * -1;
					zoff = rayDist * Mathf.Sin (angler);
				} 
				float ratio = xzoff / rayDist;
				xoff = xoff * Mathf.Abs(ratio);
				zoff = zoff * Mathf.Abs(ratio);
				dot = new Vector3 (transform.position.x + xoff, transform.position.y + yoff, transform.position.z + zoff);
				if (index < probeNumber) {
					destinations [index] = dot;
					transformer [index] = destinations [index]; 
					angles [index] = angle;
					anglesx [index] = anglex * -1;
					index++;
				}
			}
		}
		for (int i = 0; i < probeNumber; i++) {
			if (!Physics.Raycast (transform.position, destinations [i], rayDist)) {
				if (Mathf.Abs (angles [i]) <= Mathf.Abs (lowest) && Mathf.Abs (anglesx [i]) <= Mathf.Abs (lowestx)) {
					lowest = angles [i];
					lowestx = anglesx [i];
					if (Mathf.Abs(anglesx [i]) < 10) {
						works [index2] = i;
						index2++;
					}
				}
			}
		}
		if (lowestx == 360 && lowest == 360) {
			int indexing = Random.Range (0, index2);
			lowest = angles [indexing];
		} 
		Quaternion target = Quaternion.Euler (objectiveX + lowestx, objectiveY + lowest, transform.rotation.eulerAngles.z);;
		transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * smoothing);
		if (Vector3.Distance (outerEdge.transform.position, city.transform.position) < Vector3.Distance(transform.position, city.transform.position)) {
			speed = maxSpeed;
		} else {
			speed = minSpeed;
		}
	}
	void Start () {
		pointsList = new Vector3[PlayerPrefs.GetInt ("wayNumber")];
		/*
		for (int i = 0; i < balls.Length; i++) {
			balls[i] = (GameObject)Instantiate (ball);
			balls [i].SetActive (true);
		} */
		manager = managerGameobject.GetComponent<GameController> ();
		manager.enemy = this.gameObject;
		StartingPosition ();
		pointsList [PlayerPrefs.GetInt ("wayNumber") - 1] = city.transform.position; 
		int[] done = new int[PlayerPrefs.GetInt("wayNumber") - 1];
		int place = 0;
		for (int i = 0; i < PlayerPrefs.GetInt("wayNumber") - 1; i++) {
			int spot = Random.Range (0, points.Length - 1);
			pointsList [place] = points [spot].transform.position;
			done [place] = spot;
			place++;
		}
		for (int i = 0; i < done.Length - 1; i++) {
			if (done [i] == done [i + 1]) {
				if (done [i] > 20) {
					done [i + 1] = done [i + 1] + 1;
				} else {
					done [i + 1] = done [i + 1] - 1;
				}
				pointsList [i + 1] = points [done [i + 1]].transform.position;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	void FixedUpdate () {
		if (!manager.destroy && !manager.won && !manager.dead) {
			transform.Translate (new Vector3 (0f, 0f, speed));
			enemyMover ();
		}
		for (int i = 0; i < effects.Length; i++) {
			if (effects [i] != null) {
				effects [i].transform.position = transform.position;
			}
		}
		//Debug.Log (PlayerPrefs.GetInt ("BulletHits"));
		/*
		for (int i = 0; i < balls.Length; i++) {
			balls [i].transform.position = transformer [i];
		} */
	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			hits++;
			manager.impact = false;
			if (hits == PlayerPrefs.GetInt ("BulletHits")) {
				manager.won = true;
				this.gameObject.SetActive (false);
				other.gameObject.SetActive (false);
			} else {
				GameObject obj = Instantiate (hitEffect);
				obj.transform.position = transform.position;
				placeHolder++;
				obj.SetActive (true);
				effects [placeHolder] = obj;
			}
		}
	}
}