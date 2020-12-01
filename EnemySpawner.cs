using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] GameObject prefab;
	GameObject[] enemy;
	int[] rands;
	public int maxEnemy;
	[SerializeField] GameObject player;
	[SerializeField] int like = 120;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < maxEnemy; i++) { 
			rands = new int[maxEnemy];
			rands [i] = Random.Range (0, like);
			if (rands [i] == 1) {
				enemy = new GameObject[maxEnemy];
				GameObject obj = (GameObject)Instantiate (prefab);
				obj.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
				enemy [i] = obj;
			} else {
				return;
			}
		}
	}
		

	void SpawnEnemy()
	{
		//Loop through the pool of enemies
		for (int i = 0; i < enemy.Length; i++)
		{
			//If the current enemy is available (not active)...
			if (!enemy[i].gameObject.activeSelf && rands[i] == 1)
			{
				//...orient it with the spawner...
				enemy[i].transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
				enemy[i].transform.rotation = transform.rotation;
				//...enable it...
				enemy[i].gameObject.SetActive(true);
				//...and leave this method so it doesn't accidently spawn more enemies
				return;
			}
		}
	}
}
