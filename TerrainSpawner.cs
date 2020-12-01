using UnityEngine;
using System.Collections;

public class TerrainSpawner : MonoBehaviour {
	[SerializeField] GameObject prefab;
	[SerializeField] GameObject player;
	[SerializeField] float dis = 16;
	//[SerializeField]  float maxTerrain = 100;
	[SerializeField] float flo = 7f;
	float i;
	// Use this for initialization
	void Start () {
		transform.position = player.transform.position;
		i = player.transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		i += dis;
		GameObject obj = (GameObject)Instantiate (prefab);
		obj.transform.position = new Vector3 (transform.position.x, transform.position.y + flo, transform.position.z + i);
		obj.transform.rotation = transform.rotation;
	}
}