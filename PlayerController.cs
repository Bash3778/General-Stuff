using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public int floor = 1;
	[SerializeField] GameObject cameraObject;
	[SerializeField] GameObject HudButton;
    [SerializeField] GameObject controller;
	[SerializeField] float smooting = 5.0f;
	[SerializeField] float yoffset;
	[SerializeField] float h2d;
	[SerializeField] float speedMultiplier;
	[SerializeField] float rotateMultiplier;
    [SerializeField] float reorrient;
    [SerializeField] Button crouchButton;
    [SerializeField] Button throwButton;
    [SerializeField] Button hitButton;
    [SerializeField] Text crouchText;
    [SerializeField] Sprite[] spriteImages;
    [SerializeField] string[] stringTypes;
    [SerializeField] Transform handPosition;

    public Image image1;
    public Image image2;
    public string item1 = "";
    public bool up1 = false;
    public string item2 = "";
    public bool up2 = false;
    public GameObject obj1;
    public GameObject obj2;
    public bool ready = false;
    public float speedWeapon;
    public float yspeedWeapon;
    public float yaddFactor;

    HUDController joystick;
    Animator animate;
    GameController gamcontrol;
    PersonCenter person;
	// Use this for initialization
	void Start () {
        gamcontrol = controller.GetComponent<GameController>();
        animate = GetComponent<Animator>();
		joystick = HudButton.GetComponent<HUDController> ();
        throwButton.onClick.AddListener(throwController);
        hitButton.onClick.AddListener(hitController);
        crouchButton.onClick.AddListener(standChanger);
        person = this.gameObject.GetComponent<PersonCenter>();
	}
	void movement () {
		float angle = 0;
		float deltaX = joystick.direction.x - joystick.revised.x;
		float deltaY = joystick.direction.y - joystick.revised.y;
		angle = deltaX * rotateMultiplier; 
		if (deltaX != 0 || deltaY != 0) {
			transform.Rotate (new Vector3 (0, angle, 0));
		}
		float xpos;
		float zpos;
		float angleCam = (transform.rotation.eulerAngles.y % 90) * Mathf.Deg2Rad;
		Vector3 target = new Vector3 (0, 0, 0);
		if (transform.rotation.eulerAngles.y >= 0 && transform.rotation.eulerAngles.y <= 90) {
			xpos = Mathf.Sin (angleCam) * h2d ;
			zpos = Mathf.Cos (angleCam) * h2d;
		} else if (transform.rotation.eulerAngles.y > 90 && transform.rotation.eulerAngles.y <= 180) {
			xpos = Mathf.Cos (angleCam) * h2d ;
			zpos = Mathf.Sin (angleCam) * h2d * -1;
		} else if (transform.rotation.eulerAngles.y > 180 && transform.rotation.eulerAngles.y <= 270) {
			xpos = Mathf.Sin (angleCam) * h2d * -1;
			zpos = Mathf.Cos (angleCam) * h2d * -1;
		} else {
			xpos = Mathf.Cos (angleCam) * h2d * -1;
			zpos = Mathf.Sin (angleCam) * h2d;
		}
		float distX = (cameraObject.transform.position.x - transform.position.x) / 100;
		float distY = (cameraObject.transform.position.y - transform.position.y) / 100;
		float distZ = (cameraObject.transform.position.z - transform.position.z) / 100;
        float largeDist = 0.0f;
		Vector3 defaulter = new Vector3 (xpos + distX, yoffset + distY, zpos + distZ) + transform.position;
		for (int i = 100; i >= 0; i--) {
			RaycastHit hit;
			Vector3 probable = new Vector3 (defaulter.x - distX * i, defaulter.y - distY * i, defaulter.z - distZ * i);
			Vector3 direction = transform.position - probable;
            if (Physics.Raycast (probable, direction, out hit, Mathf.Infinity)) {
				if (hit.transform.position == transform.position && Vector3.Distance(probable, transform.position) > largeDist)
                {
                    target = probable - new Vector3 (distX * reorrient, distY * reorrient, distZ * reorrient);
                    largeDist = Vector3.Distance(probable, transform.position);
				}
			}
		}
        if (largeDist == 0.0f) {
            target = defaulter;
        }
		//target = new Vector3 (xpos, yoffset, zpos) + transform.position;
		cameraObject.transform.position = Vector3.Lerp (cameraObject.transform.position, target, smooting);
		Vector3 speed = new Vector3 (0, 0, Vector2.Distance (joystick.direction, joystick.revised) * speedMultiplier * -1);
        if (deltaY < 0)
        {
            speed *= -1;
        }
		transform.Translate (speed);
		cameraObject.transform.rotation = Quaternion.Euler (new Vector3 (cameraObject.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, cameraObject.transform.rotation.eulerAngles.z));
	}
    void throwController()
    {
        if (obj1) {
            Weapon objScript = obj1.GetComponent<Weapon>();
            objScript.handPos = handPosition;
            objScript.moving = true;
            obj1.SetActive(true);
            objScript.playcontrol = person;
            //objScript.playcontrol = this.gameObject.GetComponent<PlayerController>();
            obj1 = null;
            image1.sprite = null;
            item1 = null;
            gamcontrol.switchControll(true);
            if (animate.GetBool("Stand") == true)
            {
                animate.SetTrigger("StandThrow");
            }
            else
            {
                animate.SetTrigger("CrouchThrow");
            }
        }
    }
    void hitController() {
        Weapon objScript = obj1.GetComponent<Weapon>();
        objScript.handPos = handPosition;
        objScript.moving = true;
        obj1.SetActive(true);
        objScript.playcontrol = person;
        //objScript.playcontrol = this.gameObject.GetComponent<PlayerController>();
        if (animate.GetBool("Stand") == true)
        {
            animate.SetTrigger("StandHit");
        }
        else
        {
            animate.SetTrigger("CrouchHit");
        }
        obj1.SetActive(false);
        objScript.moving = false;
    }
    void standChanger() {
        if (animate.GetBool("Stand") == true)
        {
            animate.SetBool("Crouch", true);
            animate.SetBool("Stand", false);
            crouchText.text = "Stand";
        }
        else {
            animate.SetBool("Stand", true);
            animate.SetBool("Crouch", false);
            crouchText.text = "Crouch";
        }
    }
    void itemSelector() {
        for (int i = 0; i < stringTypes.Length; i++) {
            if (item1 == stringTypes[i] && up1) {
                image1.sprite = spriteImages[i];
                image1.gameObject.SetActive(true);
            }
            if (item2 == stringTypes[i] && up2) {
                image2.sprite = spriteImages[i];
                image2.gameObject.SetActive(true);
            }
        }
    }
	// Update is called once per frame
	void Update () {
        itemSelector();
		movement ();
        if (up1)
        {
            image1.gameObject.SetActive(true);
        }
        else {
            image1.gameObject.SetActive(false);
        }
        if (up2) {
            image2.gameObject.SetActive(true);
        }
        else
        {
            image2.gameObject.SetActive(false);
        }
    } 
    void OnCollisionEnter(Collision collider)
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.rotation = Quaternion.identity;
        
    }
    private void OnTriggerEnter(Collider other)
    {

    }
}
