using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
public class UnityDarkMatterController : MonoBehaviour {
	[SerializeField] GameObject matterPointObject;
	[SerializeField] GameObject scrptController;
	[SerializeField] InputField inputter;
	[SerializeField] InputField zoomOutField;
	[SerializeField] InputField speedInput;
	[SerializeField] GameObject cameraObject;
	[SerializeField] float galaxyScale; 
	[SerializeField] int matterPerGalaxy;
	[SerializeField] int galaxyNumber;
	[SerializeField] Transform central;
	string txt = System.IO.File.ReadAllText(@"/Users/benjaminash/Desktop/School/Summer Projects/Dark Matter Halo Project/Random.txt");
	Vector3[] positions;
	GameObject[] present;
	HUDcontroller joyStick;
	float speed = 0;
	// Use this for initialization
	void Awake () {
		joyStick = scrptController.GetComponent<HUDcontroller> ();
	}
	void Start () {
		present = new GameObject[matterPerGalaxy * galaxyNumber];
		positions = new Vector3[matterPerGalaxy * galaxyNumber];
		char[] chars = txt.ToCharArray ();
		string[] pointsString = new string[matterPerGalaxy * galaxyNumber * 3];
		int indexNumber = 0;
		for (int i = 0; i < chars.Length; i++) {
			if (chars [i].ToString() == " " && indexNumber < pointsString.Length - 1) {
				indexNumber++;
			} else {
				pointsString [indexNumber] = pointsString [indexNumber] + chars [i].ToString ();
			}
		}
		float[] pointsFloat = new float[matterPerGalaxy * galaxyNumber * 3]; 
		for (int i = 0; i < pointsString.Length; i++) {
			float.TryParse (pointsString [i], out pointsFloat [i]);
		}
		int indexcounter = 0;
		int index = -1;
		for (int i = 0; i < pointsFloat.Length; i++) {
			index++;
			if (indexcounter < positions.Length) {
				if (index == 0) {
					positions [indexcounter].x = pointsFloat [i];
				} else if (index == 1) {
					positions [indexcounter].y = pointsFloat [i];
				} else {
					positions [indexcounter].z = pointsFloat [i];
					indexcounter++;
					index = -1;
				}
			}
		}
		for (int i = 0; i < present.Length; i++) {
			present [i] = Instantiate (matterPointObject);
			present [i].transform.position = positions [i];
			present [i].transform.SetParent (central);
		}
	}
	// Update is called once per frame
	void Update () {
		float.TryParse (speedInput.text, out speed);
		float zoomOut;
		float.TryParse (zoomOutField.text, out zoomOut);
		if (zoomOut == 0) {
			zoomOut = 4;
		}
		if (speed == 0) {
			speed = 0.2f;
		}
		float rotatex = (joyStick.direction.y - joyStick.revised.y) * speed;
		float rotatey = (joyStick.direction.x - joyStick.revised.x) * speed;
		Vector3 rotate = new Vector3 (central.transform.rotation.eulerAngles.x + rotatex, central.transform.rotation.eulerAngles.y + rotatey, central.transform.rotation.eulerAngles.z);
		if (central.rotation.eulerAngles.x + rotatex < 90 || central.rotation.eulerAngles.x + rotatex > 270) {
			central.rotation = Quaternion.Euler (rotate);
		}
		cameraObject.transform.position = new Vector3 (0, 0, -1 * galaxyScale * zoomOut);
		if (inputter.text != "" || inputter.text != null) {
			float index;
			float.TryParse (inputter.text, out index);
			float bottom = matterPerGalaxy * index;
			float top = (matterPerGalaxy * (index + 1));
			for (int i = 0; i < present.Length; i++) {
				if (i >= bottom && i < top) {
					present [i].SetActive (true);
				} else {
					present [i].SetActive (false);
				}
			}
		}
	}
}