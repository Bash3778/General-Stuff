using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] stairPoints;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject objectSpawn;
    [SerializeField] GameObject signalSpawn;
    [SerializeField] GameObject[] weapons;
    [SerializeField] int numbPointSpawn;
    [SerializeField] float playDist;
    [SerializeField] GameObject pickUp;
    [SerializeField] Button pickButton;
    [SerializeField] GameObject switchObj;
    [SerializeField] GameObject enemy;

    public Transform[] signalSpawns;

    Transform[] objectSpawns;
    bool[] possibleSpawns;
    public GameObject[] weaponsList = new GameObject[1000];
    StairController[] stairs = new StairController[4];
    int onIndex = -1;
    int wepIndex = 0;
    bool pickUpOn = false;
    int numbPoints = 0;
    Button switcher;
    PlayerController playerScript;
    EnemyPlayerControl enemyScript;
    // Start is called before the first frame update
    void objectManager()
    {
        for (int i = 0; i < objectSpawns.Length; i++)
        {
            if (!possibleSpawns[i] && numbPoints < numbPointSpawn) {
                float prob = ((float)(numbPointSpawn - numbPoints) / (float)(objectSpawns.Length - i) * 100);
                float randNumb = Random.Range(0f, 100f);
                if (randNumb <= prob)
                {
                    possibleSpawns[i] = false;
                    numbPoints++;
                    int randIndex = Random.Range(0, weapons.Length);
                    GameObject obj = Instantiate(weapons[randIndex]);
                    obj.transform.position = objectSpawns[i].position;
                    obj.transform.rotation = Quaternion.Euler(-90f, Random.Range(0f, 360f), obj.transform.rotation.z);
                    Weapon wepScr = obj.GetComponent<Weapon>();
                    wepScr.placeIndex = i;
                    wepScr.index = i;
                    wepScr.player = player;
                    wepScr.playDist = playDist;
                    wepScr.pickUp = pickUp;
                    weaponsList[wepIndex] = obj;
                    wepIndex++;
                }
            }
        }
    }
    void Start()
    {
        enemyScript = enemy.GetComponent<EnemyPlayerControl>();
        enemyScript.player = player;
        enemyScript.control = this.gameObject.GetComponent<GameController>();
        switcher = switchObj.GetComponent<Button>();
        switcher.onClick.AddListener(delegate { switchControll(false); }) ;
        playerScript = player.GetComponent<PlayerController>();
        pickButton.onClick.AddListener(pickUpper);
        objectSpawns = objectSpawn.GetComponentsInChildren<Transform>();
        signalSpawns = signalSpawn.GetComponentsInChildren<Transform>();
        possibleSpawns = new bool[objectSpawns.Length];
        for (int i = 0; i < possibleSpawns.Length; i++) {
            possibleSpawns[i] = false;
        }
        for (int i = 0; i < stairPoints.Length; i++) {
            stairs[i] = stairPoints[i].GetComponent<StairController>();
        }
        objectManager();
    }
    void pickUpper()
    {
        if (!playerScript.up2)
        {
            Weapon ws = weaponsList[onIndex].GetComponent<Weapon>();
            possibleSpawns[ws.placeIndex] = false;
            numbPoints--;
            ws.objectOn = false;
            weaponsList[onIndex].GetComponent<Rigidbody>().isKinematic = true;
            weaponsList[onIndex].GetComponent<Collider>().isTrigger = true;
            ws.near = false;
            weaponsList[onIndex].SetActive(false);
            pickUpOn = false;
            if (playerScript.up1)
            {
                playerScript.item2 = ws.typeTag;
                playerScript.up2 = true;
                playerScript.obj2 = weaponsList[onIndex];
            }
            else
            {
                playerScript.item1 = ws.typeTag;
                playerScript.up1 = true;
                playerScript.obj1 = weaponsList[onIndex];
            }
        }
    }
    public void switchControll(bool possible) {
        if (possible)
        {
            playerScript.up1 = false;
        }
        if (playerScript.up2 && (playerScript.up1 || possible)) {
            Sprite interm = playerScript.image1.sprite;
            playerScript.image1.sprite = playerScript.image2.sprite;
            playerScript.image2.sprite = interm;
            string interms = playerScript.item1;
            playerScript.item1 = playerScript.item2;
            playerScript.item2 = interms;
            GameObject intermg = playerScript.obj1;
            playerScript.obj1 = playerScript.obj2;
            playerScript.obj2 = intermg;
            bool intermb = playerScript.up1;
            playerScript.up1 = playerScript.up2;
            playerScript.up2 = intermb;
        }
    }
    // Update is called once per frame
    void Update()
    {
        objectManager();
        if (playerScript.up1 && playerScript.up2)
        {
            switchObj.SetActive(true);
        }
        else {
            switchObj.SetActive(false);
        }
        for (int i = 0; i < weaponsList.Length; i++) {
            if (weaponsList[i] != null) {
                Weapon wep = weaponsList[i].GetComponent<Weapon>();
                wep.playDist = playDist;
                if (wep.near) {
                    pickUpOn = true;
                    onIndex = i;
                }
            }
        }
        if (!pickUpOn) {
            onIndex = -1;
        }
        if (pickUpOn && !playerScript.up2)
        {
            pickUp.SetActive(true);
        }
        else {
             pickUp.SetActive(false);
        }
      
      for (int i = 0; i < stairs.Length; i++)
        {
            if (stairs[i].hit) {
                player.transform.position = spawnPoints[stairs[i].end].transform.position;
                if (stairs[i].end > 1)
                {
                    player.GetComponent<PlayerController>().floor = 2;
                    player.transform.position = new Vector3(player.transform.position.x, -9.81f, player.transform.position.z);
                }
                else {
                    player.GetComponent<PlayerController>().floor = 1;
                    player.transform.position = new Vector3(player.transform.position.x, -8.91f, player.transform.position.z);
                }
                player.transform.rotation = spawnPoints[stairs[i].end].transform.rotation;
                stairs[i].hit = false;
            }
        }
        pickUpOn = false;
    }
}
