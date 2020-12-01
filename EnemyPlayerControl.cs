using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
So it seems the problems that I ahev seen from the start are as follows

Firstly, when the player hits someone with an object in their hand, and then trys to hit with another object, both are 
ontop of each other and then when thrown there are problems.  Player animation speed increasing with the speed the player is going at.

Concerning the enemy, he seems to be able to find objects pretty efficently, be he is has no way of stopping to throw an object nor 
does he have an animation hierachy

Future issues would be adding additinal player skins, setting up the team battle controls, spawning new weapons when there are not enough avialable, 
death and respond, and gneral UI and starting screen.  Way more of the app is finsihed then I thought with very little code, genrally speaking

*/
public class EnemyPlayerControl : MonoBehaviour
{
    public GameObject player;
    public GameController control;

    [SerializeField] Transform handPosition;

    NavMeshAgent meshnav;
    GameObject object1 = null;
    Transform goingPosition;
    GameObject currentWeapon;
    bool destSet = false;
    bool weaponGoing = false;
    bool stopAction;
    Animator anim;
    PersonCenter person;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        meshnav = this.gameObject.GetComponent<NavMeshAgent>();
        person = this.gameObject.GetComponent<PersonCenter>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void attacking()
    {
        Weapon objScript = object1.GetComponent<Weapon>();
        objScript.handPos = handPosition;
        objScript.moving = true;
        object1.SetActive(true);
        objScript.playcontrol = person;
        objScript.enemyYes = true;
        if (anim.GetBool("Stand") == true)
        {
            anim.SetTrigger("StandThrow");
        }
        else
        {
            anim.SetTrigger("CrouchThrow");
        }
    }
    void destinationSetter() {
        if (!destSet)
        {
            if (object1 == null)
            {
                float closestDist = 1000;
                weaponGoing = true;
                for (int i = 0; i < control.weaponsList.Length; i++)
                {
                    if (control.weaponsList[i] != null)
                    {
                        if (Vector3.Distance(transform.position, control.weaponsList[i].transform.position) < closestDist)
                        {
                            closestDist = Vector3.Distance(transform.position, control.weaponsList[i].transform.position);
                            goingPosition = control.signalSpawns[control.weaponsList[i].gameObject.GetComponent<Weapon>().index];
                            currentWeapon = control.weaponsList[i];
                        }
                    }
                }
            }
            else
            {
                goingPosition = player.transform;
            }
            destSet = true;
        }
        else {
            if (weaponGoing)
            {
                if (Vector3.Distance(currentWeapon.transform.position, transform.position) < 10)
                {
                    currentWeapon.SetActive(false);
                    object1 = currentWeapon;
                    currentWeapon = null;
                    weaponGoing = false;
                    destSet = false;
                }
            }
            else {
                if (Vector3.Distance(goingPosition.position, transform.position) < 20) {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.position - player.transform.position, out hit, Mathf.Infinity)) {
                        stopAction = true;
                        attacking();
                        object1 = null;
                        destSet = false;
                        Debug.Log("Killing");
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        destinationSetter();
        if (!stopAction)
        {
            meshnav.destination = goingPosition.position;
            meshnav.updateRotation = false;
            float xdist = meshnav.velocity.x;
            float zdist = meshnav.velocity.z;
            float angle = Mathf.Atan(xdist / zdist) * Mathf.Rad2Deg;
            if (zdist > 0)
            {
                angle += 180;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
        } else if (person.ready) {
            stopAction = false;
        }
    }
}
