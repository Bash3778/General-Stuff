using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	[SerializeField] Text txt;
	[SerializeField] Text score;
	public int tscore = 0;
	// Use this for initialization
	void Start () {
		txt.text = "Score: " + tscore;
	}
	hello
	// Update is called once per frame
	void Update () {
		txt.text = "Score: " + tscore;
		score.text = "Score: " + tscore;
	}
}
