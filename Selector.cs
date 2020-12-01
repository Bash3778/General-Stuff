using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Selector : MonoBehaviour {
	[SerializeField] Button rock;
	[SerializeField] Button ice;
	[SerializeField] Button fire;
	// Use this for initialization
	void Start () {
		rock.onClick.AddListener (rocker);
		ice.onClick.AddListener (icer);
		fire.onClick.AddListener (firer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void rocker () {
		SceneManager.LoadScene ("Fire");
	}
	void icer () {
		SceneManager.LoadScene ("Rock");
	}
	void firer () {
		SceneManager.LoadScene ("Ice");
	}
}
