using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour {
	[SerializeField] Button pauseb;
	[SerializeField] Transform gameplay;
	[SerializeField] Transform pcanvas;
	[SerializeField] Transform rcanvas;
	[SerializeField] Button resume;
	[SerializeField] Button prestart;
	[SerializeField] Button rrestart;
	[SerializeField] Button rmenu;
	[SerializeField] Button pmenu;
	public bool dead = false;
	[SerializeField] string scene;
	[SerializeField] string menu;
	// Use this for initialization
	void Start () {
		pauseb.onClick.AddListener (pauser);
		resume.onClick.AddListener (resumer);
		prestart.onClick.AddListener (reloader);
		rrestart.onClick.AddListener (ReloadScene);
		rmenu.onClick.AddListener(main);
		pmenu.onClick.AddListener(main);
	}
	
	// Update is called once per frame
	void Update () {
		deader ();
	}
	void pauser () {
		pcanvas.gameObject.SetActive (true);
		gameplay.gameObject.SetActive (false);
		Time.timeScale = 0;
	}
	void resumer () {
		pcanvas.gameObject.SetActive (false);
		gameplay.gameObject.SetActive (true);
		Time.timeScale = 1;
	}
	void deader()
	{
		if (dead == true) {
			gameplay.gameObject.SetActive (false);
			rcanvas.gameObject.SetActive (true);
		}
	}
	void ReloadScene () {
		SceneManager.LoadScene (scene);
	}
	void reloader () {
		Time.timeScale = 1;
		SceneManager.LoadScene (scene);
	}
	void main () {
		SceneManager.LoadScene (menu);
		Time.timeScale = 1;
	}
}
