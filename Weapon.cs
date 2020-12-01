using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int index;
    public bool inUse = false;
    public GameObject player;
    public float playDist;
    public GameObject pickUp;
    public bool near = false;
    public bool objectOn = true;
    public string typeTag;
    public bool moving = false;
    public int placeIndex;
    public Transform handPos;
    public PersonCenter playcontrol;
    public bool enemyYes = false;


    bool going = false;
    float expo = 1.0f;
    bool found = false;
    float xvel;
    float zvel;
    Rigidbody rig;
    Collider collide;

    // Start is called before the first frame update
    void Start()
    {
        rig = this.gameObject.GetComponent<Rigidbody>();
        collide = this.gameObject.GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -30) {
            this.gameObject.SetActive(false);
        }
        if (moving) {
            if (playcontrol.ready)
            {
                collide.isTrigger = false;
                going = true;
            }
            else {
                if (!going)
                {
                    transform.position = handPos.position;
                    transform.rotation = handPos.rotation;
                }
            }
            if (!going)
            {
                collide.isTrigger = true;
            }
            if (going)
            {
                if (Vector3.Distance(handPos.transform.position, transform.position) < 0.5)
                {
                    collide.isTrigger = true;
                }
                else {
                    collide.isTrigger = false;
                    rig.isKinematic = false;
                }
                if (!found)
                {
                    float angle;
                    Debug.Log(player.transform.rotation.eulerAngles.y);
                    if (player.transform.rotation.eulerAngles.y > 0)
                    {
                        angle = player.transform.rotation.eulerAngles.y % 360;
                    }
                    else
                    {
                        angle = 360 + (player.transform.rotation.eulerAngles.y % 360);
                    }
                    if (enemyYes)
                    {
                        angle += 180;
                    }
                    xvel = Mathf.Cos(angle * Mathf.Deg2Rad) * playcontrol.speedWeapon;
                    zvel = Mathf.Sin(angle * Mathf.Deg2Rad) * playcontrol.speedWeapon;
                    xvel = xvel * -1;
                    zvel = zvel * -1;
                    found = true;
                }
                transform.Translate(new Vector3(zvel * Time.deltaTime, -1 * Mathf.Pow(playcontrol.yspeedWeapon, expo) * Time.deltaTime, xvel * Time.deltaTime), Space.World);
                expo += playcontrol.yaddFactor;
                rig.constraints = RigidbodyConstraints.None;
            }
        }
        if (Vector3.Distance(player.transform.position, transform.position) < playDist && objectOn)
        {
            near = true;
        }
        else {
            near = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (going && collision.other.tag != "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnCollisionExit(Collision collision)
    {

    }
    private void OnTriggerExit(Collider other)
    {

    }
}
