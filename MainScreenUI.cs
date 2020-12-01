using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainScreenUI : MonoBehaviour {
	[SerializeField] Button BackButtonM;
	[SerializeField] Button BackButtonS;
	[SerializeField] Text missileColor;
	[SerializeField] Text treeSettingText;
	[SerializeField] Text waypointText;
	[SerializeField] Button treeButton;
	[SerializeField] Button waypointButton;
	[SerializeField] Button[] colorButtons;
	[SerializeField] Button playButton;
	[SerializeField] Button howtoplayButton;
	[SerializeField] Button settingsButton;
	[SerializeField] Transform howtoplayScreen;
	[SerializeField] Transform settingsScreen;
	[SerializeField] Transform mainScreen;
	[SerializeField] bool treeSetting = false;
	[SerializeField] int waypointSetting = 7;
	[SerializeField] int colorSetting = 0;
	[SerializeField] int bulletSetting = 1;
	[SerializeField] string[] colorNames;
	[SerializeField] InputField bulDist;
	[SerializeField] Button bulNumber;
	[SerializeField] Text bulHitText;
	[SerializeField] Button beacon;
	[SerializeField] Text beaconText;
	[SerializeField] bool beaconSetting = false;
	[SerializeField] GameObject loadingScreener;
	[SerializeField] Slider loadingSlider;
	AsyncOperation async;
	// Use this for initialization 
	void Start () {
		beacon.onClick.AddListener (beaconer);
		bulNumber.onClick.AddListener (bulSet);
		BackButtonM.onClick.AddListener (backButton);
		BackButtonS.onClick.AddListener (backButton);
		howtoplayButton.onClick.AddListener (howtoplay);
		settingsButton.onClick.AddListener (settingsBut);
		playButton.onClick.AddListener (startLoadingScreen);
		treeButton.onClick.AddListener (treeSetter);
		waypointButton.onClick.AddListener (waypointSetter);
		colorButtons [0].onClick.AddListener (redBut);
		colorButtons [1].onClick.AddListener (blueBut);
		colorButtons [2].onClick.AddListener (greenBut);
		colorButtons [3].onClick.AddListener (orangeBut);
		colorButtons [4].onClick.AddListener (yellowBut);
		colorButtons [5].onClick.AddListener (purpleBut);
		colorButtons [6].onClick.AddListener (pinkBut);
		colorButtons [7].onClick.AddListener (brownBut);
		colorButtons [8].onClick.AddListener (silverBut);
		colorButtons [9].onClick.AddListener (turquiseBut);
	}
	
	// Update is called once per frame
	void Update () {
		if (treeSetting) {
			PlayerPrefs.SetInt ("treeSetting", 1);
		} else {
			PlayerPrefs.SetInt ("treeSetting", 0);
		}
		if (beaconSetting) {
			PlayerPrefs.SetInt ("beaconSetting", 1);
		} else {
			PlayerPrefs.SetInt ("beaconSetting", 0);
		}
		PlayerPrefs.SetInt ("BulletHits", bulletSetting);
		PlayerPrefs.SetInt ("wayNumber", waypointSetting);
		PlayerPrefs.SetInt ("missileColor", colorSetting);
		treeSettingText.text = "Tree Setting: " + treeSetting;
		waypointText.text = "Waypoint Number: " + (waypointSetting - 1);
		missileColor.text = "Current Color: " + colorNames[colorSetting];
		bulHitText.text = "Bullet Hit Number: " + bulletSetting;
		beaconText.text = "Beacon Setting: " + beaconSetting;
		string texter = bulDist.text.ToString ();
		float dist;
		if (bulDist.text.ToString () == "") {
			dist = 200f;
		} else {
			float.TryParse (texter, out dist);
		}
		PlayerPrefs.SetFloat ("bulletDistance", dist);
	}
	public void startLoadingScreen () {
		StartCoroutine (loadingScreen ());
	}
	IEnumerator loadingScreen () {
		loadingScreener.gameObject.SetActive (true);
		Time.timeScale = 0f;
		mainScreen.gameObject.SetActive (false);
		async = SceneManager.LoadSceneAsync ("Game");
		async.allowSceneActivation = false;
		while (async.isDone == false) {
			loadingSlider.value = async.progress;
			if (async.progress == 0.9f) {
				Time.timeScale = 1f;
				loadingSlider.value = 1f;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}
	void beaconer () {
		if (beaconSetting) {
			beaconSetting = false;
		} else {
			beaconSetting = true;
		}
	}
	void playBut () {
		SceneManager.LoadScene ("Game");
	}
	void howtoplay () {
		howtoplayScreen.gameObject.SetActive (true);
		settingsScreen.gameObject.SetActive (false);
		mainScreen.gameObject.SetActive (false);
	}
	void settingsBut () {
		howtoplayScreen.gameObject.SetActive (false);
		settingsScreen.gameObject.SetActive (true);
		mainScreen.gameObject.SetActive (false);
	}
	void backButton () {
		mainScreen.gameObject.SetActive (true);
		howtoplayScreen.gameObject.SetActive (false);
		settingsScreen.gameObject.SetActive (false);
	}
	void treeSetter () {
		if (treeSetting) {
			treeSetting = false;
		} else {
			treeSetting = true;
		}
	}
	void waypointSetter () {
		if (waypointSetting > 10) {
			waypointSetting = 1;
		} else {
			waypointSetting++;
		}
	}
	void bulSet () {
		if (bulletSetting > 4) {
			bulletSetting = 1;
		} else {
			bulletSetting++;
		}
	}
	void redBut () {
		colorSetting = 0;
	}
	void blueBut () {
		colorSetting = 1;
	}
	void greenBut () {
		colorSetting = 2;
	}
	void orangeBut () {
		colorSetting = 3;
	}
	void yellowBut () {
		colorSetting = 4;
	}
	void purpleBut () {
		colorSetting = 5;
	}
	void pinkBut () {
		colorSetting = 6;
	}
	void brownBut () {
		colorSetting = 7;
	}
	void silverBut () {
		colorSetting = 8;
	}
	void turquiseBut () {
		colorSetting = 9;
	}
}
