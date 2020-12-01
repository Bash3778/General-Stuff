using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour {
	[SerializeField] Image playerIcon;
	[SerializeField] Image enemyIcon;
	[SerializeField] Image starter;
	[SerializeField] GameObject controll;
	[SerializeField] GameObject enemy;
	[SerializeField] GameObject origon;
	[SerializeField] Image orginI;
	[SerializeField] GameObject end;
	[SerializeField] Image endI;
	[SerializeField] GameObject playerg;
	[SerializeField] GameObject enemyg;
	[SerializeField] Button map;
	[SerializeField] Button exit;
	[SerializeField] Button pauser;
	[SerializeField] Button winStart;
	[SerializeField] Button deadStart;
	[SerializeField] Button cityStart;
	[SerializeField] Button resumeButton;
	[SerializeField] Image playerIcon2;
	[SerializeField] Image enemyIcon2;
	[SerializeField] Image end2;
	[SerializeField] Image end12;
	[SerializeField] GameObject playerg2;
	[SerializeField] GameObject enemyg2;
	[SerializeField] Transform HUD;
	[SerializeField] Transform mapping;
	[SerializeField] Transform pause;
	[SerializeField] Transform win;
	[SerializeField] Transform city;
	[SerializeField] Transform destroyP;
	[SerializeField] Button mainMenWin;
	[SerializeField] Button mainMenDest;
	[SerializeField] Button mainMenL;
	[SerializeField] Button mainMenPause;
	[SerializeField] Button pauseRestart;
	[SerializeField] Text waypointNumber;
	[SerializeField] Text bulletNumb;
	[SerializeField] GameObject loadingScreener;
	[SerializeField] Slider loadingSlider; 
	AsyncOperation async;
	float ratio;
	float smallDist;
	Vector2 starterPos;
	Vector2 playerPos;
	Vector2 enemyPos;
	GameController gamer;
	EnemyController enem;
	bool status = false;
	// Use this for initialization
	void Start () {
		enem = enemy.GetComponent<EnemyController> ();
		gamer = controll.GetComponent<GameController> ();
		map.onClick.AddListener (mapper);
		exit.onClick.AddListener (exitter);
		pauser.onClick.AddListener (pauseButton);
		resumeButton.onClick.AddListener (resume);
		winStart.onClick.AddListener (startLoadingScreen);
		cityStart.onClick.AddListener (startLoadingScreen);
		deadStart.onClick.AddListener (startLoadingScreen);
		mainMenDest.onClick.AddListener (mainMen);
		mainMenWin.onClick.AddListener (mainMen);
		mainMenL.onClick.AddListener (mainMen);
		mainMenPause.onClick.AddListener (mainMen);
		pauseRestart.onClick.AddListener (startLoadingScreen);
	}
	void canvasSwitcher () {
		if (gamer.dead) {
			HUD.gameObject.SetActive (false);
			mapping.gameObject.SetActive (false);
			city.gameObject.SetActive (false);
			destroyP.gameObject.SetActive (true);
			pause.gameObject.SetActive (false);
			win.gameObject.SetActive (false);
		}
		if (gamer.won) {
			HUD.gameObject.SetActive (false);
			mapping.gameObject.SetActive (false);
			city.gameObject.SetActive (false);
			destroyP.gameObject.SetActive (false);
			pause.gameObject.SetActive (false);
			win.gameObject.SetActive (true);
		}
		if (gamer.destroy) {
			HUD.gameObject.SetActive (false);
			mapping.gameObject.SetActive (false);
			city.gameObject.SetActive (true);
			destroyP.gameObject.SetActive (false);
			pause.gameObject.SetActive (false);
			win.gameObject.SetActive (false);
		}
	}

	public void startLoadingScreen () {
		StartCoroutine (loadingScreen ());
	}
	IEnumerator loadingScreen () {
		loadingScreener.gameObject.SetActive (true);
		pause.gameObject.SetActive (false);
		destroyP.gameObject.SetActive (false);
		city.gameObject.SetActive (false);
		win.gameObject.SetActive (false);
		Time.timeScale = 0f;
		async = SceneManager.LoadSceneAsync ("Game");
		async.allowSceneActivation = false;
		while (async.isDone == false) {
			loadingSlider.value = async.progress;
			if (async.progress == 0.9f) {
				loadingSlider.value = 1f;
				async.allowSceneActivation = true;
				Time.timeScale = 1f;
			}
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {
		Image oI;
		Image eI;
		Image pL;
		Image eL;
		GameObject pG;
		GameObject eG;
		if (!status) {
			oI = orginI;
			eI = endI;
			pL = playerIcon;
			eL = enemyIcon;
			pG = playerg;
			eG = enemyg;
		} else {
			oI = end2;
			eI = end12;
			pL = playerIcon2;
			eL = enemyIcon2;
			pG = playerg2;
			eG = enemyg2;
		}
		smallDist = Vector2.Distance (oI.transform.position, eI.transform.position);
		starterPos = oI.transform.position;
		ratio = Vector2.Distance (oI.transform.position, eI.transform.position) / Vector3.Distance (origon.transform.position, end.transform.position);
		playerPos = new Vector2 (Vector2.Distance (new Vector2 (gamer.playerPosition.x, 0), new Vector2 (origon.transform.position.x, 0)) * ratio, Vector2.Distance (new Vector2 (gamer.playerPosition.z, 0), new Vector2 (origon.transform.position.z, 0)) * ratio);
		enemyPos = new Vector2 (Vector2.Distance (new Vector2 (enemy.transform.position.x, 0), new Vector2 (origon.transform.position.x, 0)) * ratio, Vector2.Distance (new Vector2 (enemy.transform.position.z, 0), new Vector2 (origon.transform.position.z, 0)) * ratio);
		if (playerPos.x < 0 || playerPos.y < 0 || playerPos.x > smallDist || playerPos.y > smallDist) {
			pG.SetActive (false);
		} else {
			pG.SetActive (true);
			pL.transform.position = playerPos + starterPos;
		}
		if (enemyPos.x < 0 || enemyPos.y < 0 || enemyPos.x > smallDist || enemyPos.y > smallDist) {
			eG.SetActive (false);
		} else {
			eG.SetActive (true);
			eL.transform.position = enemyPos + starterPos;
		}
		bulletNumb.text = "Bullet Hits: " + enem.hits.ToString();
		waypointNumber.text = "Waypoints left: " + (PlayerPrefs.GetInt ("wayNumber") - enem.pointsPosition).ToString ();
		canvasSwitcher ();
	}
	void mapper () {
		status = true;
		HUD.gameObject.SetActive (false);
		mapping.gameObject.SetActive (true);
	}
	void exitter () {
		status = false;
		HUD.gameObject.SetActive (true);
		mapping.gameObject.SetActive (false);
	}
	void pauseButton () {
		HUD.gameObject.SetActive (false);
		mapping.gameObject.SetActive (false);
		city.gameObject.SetActive (false);
		destroyP.gameObject.SetActive (false);
		pause.gameObject.SetActive (true);
		win.gameObject.SetActive (false);
		Time.timeScale = 0f;
	}
	void resume () {
		HUD.gameObject.SetActive (true);
		mapping.gameObject.SetActive (false);
		city.gameObject.SetActive (false);
		destroyP.gameObject.SetActive (false);
		pause.gameObject.SetActive (false);
		win.gameObject.SetActive (false);
		Time.timeScale = 1f;
	}
	void mainMen () {
		SceneManager.LoadScene ("MainScreen");
		Time.timeScale = 1f;
	}
	void startOverAction () {
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Game");
	}
}
