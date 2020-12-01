using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCenter : MonoBehaviour
{
public int floor = 1;
    [SerializeField] GameObject controller;
	[SerializeField] float smooting = 5.0f;
    [SerializeField] Transform handPosition;
    [SerializeField] bool playerType;
    public bool ready = false;
    public float speedWeapon;
    public float yspeedWeapon;
    public float yaddFactor;
    GameController gamcontrol;
    PlayerController playerControl;
    EnemyPlayerControl enemy;
	// Use this for initialization
	void Start () {
        gamcontrol = controller.GetComponent<GameController>();
        if (playerType) {
            playerControl = this.gameObject.GetComponent<PlayerController>();
        } else {
            enemy = this.gameObject.GetComponent<EnemyPlayerControl>();
        }
	}
	// Update is called once per frame
	void Update () {

    } 
    void OnCollisionEnter(Collision collider)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

    }
}
