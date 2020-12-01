using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public string name;
    public float xpos = 0;
    public float ypos = 0;
    public float zpos = 0;
    public float charge = 0;
    public Camera camera;
    public MainController mainner;
    public bool touched = false;
    [SerializeField] Material redCharge;
    [SerializeField] Material blueCharge;
    [SerializeField] float oneSize;
    [SerializeField] float chargeRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(xpos, ypos, zpos);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.position == transform.position && Input.GetMouseButtonDown(0))
        {
            touched = true;
            mainner.changeCharge();
        }
        if (charge < 0)
        {
            this.gameObject.GetComponent<Renderer>().material = blueCharge;
        } else
        {
            this.gameObject.GetComponent<Renderer>().material = redCharge;
        }
        transform.localScale = new Vector3(oneSize + (Mathf.Abs(charge) - 1) * chargeRate, oneSize + (Mathf.Abs(charge) - 1) * chargeRate, oneSize + (Mathf.Abs(charge) - 1) * chargeRate);
    }
}
