using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldArrowControl : MonoBehaviour
{
    [SerializeField] Camera camera;
    public float contourStrength;
    public float arrowStrength;
    public float phi;
    public float theta;
    public Transform arrowCanvas;
    [SerializeField] Transform generalCanvas;
    public Material offMaterial;
    [SerializeField] Material onMaterial;
    public MainController mainner;
    public Text phiText;
    public Text thetaText;
    public Text contourText;
    public Text fieldText;
    public int arrowIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.position == transform.position && Input.GetMouseButtonDown(0))
        {
            phiText.text = phi.ToString();
            thetaText.text = theta.ToString();
            contourText.text = (contourStrength * 9 * Mathf.Pow(10, 9)).ToString();
            fieldText.text = (arrowStrength * 9*Mathf.Pow(10, 9)).ToString();
            mainner.arrowCanvasOn = true;
            arrowCanvas.gameObject.SetActive(true);
            generalCanvas.gameObject.SetActive(false);
            if (mainner.currentArrow != -1)
            {
                mainner.fieldArrows[mainner.currentArrow].gameObject.GetComponent<Renderer>().material = offMaterial;
            }
            mainner.currentArrow = arrowIndex;
            gameObject.GetComponent<Renderer>().material = onMaterial;
        }
    }
}
