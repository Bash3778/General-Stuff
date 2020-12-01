using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IndivMenu : MonoBehaviour {
	[SerializeField] Transform main;
	[SerializeField] Transform how;
	[SerializeField] Transform select;
	[SerializeField] Button play;
	[SerializeField] Button bhow;
	[SerializeField] Button back;
	// Use this for initialization
	void Start () {
		play.onClick.AddListener (player);
		bhow.onClick.AddListener (hower);
		back.onClick.AddListener (backer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void player () {
		main.gameObject.SetActive (false);
		select.gameObject.SetActive (true);
	}
	void backer () {
		how.gameObject.SetActive (false);
		main.gameObject.SetActive (true);
	}
	void hower () {
		main.gameObject.SetActive (false);
		how.gameObject.SetActive (true);
	}
}
