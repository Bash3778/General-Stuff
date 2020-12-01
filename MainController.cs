using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] GameObject wheelButton;
    [SerializeField] GameObject sampleCharge;
    [SerializeField] GameObject sampleFieldArrow;
    [SerializeField] GameObject buildObject;
    [SerializeField] float cameraMult;
    [SerializeField] float cameraMax = 85;
    [SerializeField] float cameraMin = 5;
    [SerializeField] float numbOfCharge = 10;
    [SerializeField] GameObject pivitCenter;
    [SerializeField] Button up;
    [SerializeField] Button down;
    [SerializeField] Button right;
    [SerializeField] Button left;
    [SerializeField] Button zoomIn;
    [SerializeField] Button zoomOut;
    [SerializeField] Button reset;
    [SerializeField] Button newChargeButton;
    [SerializeField] Button deleteCurrentCharge;
    [SerializeField] Button compileFieldsButton;
    [SerializeField] Button hideShowFields;
    [SerializeField] Button hideShowContours;
    [SerializeField] Button helpButton;
    [SerializeField] Button helpBackButton;
    [SerializeField] Button arrowBackButton;
    [SerializeField] float moveFactorCamera;
    [SerializeField] float minYMovement;
    [SerializeField] float maxYMovement;
    [SerializeField] float outerMost;
    [SerializeField] float zoomFactor;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] InputField nameField;
    [SerializeField] InputField xposField;
    [SerializeField] InputField yposField;
    [SerializeField] InputField zposField;
    [SerializeField] InputField chargeField;
    [SerializeField] float lowestExtreme;
    [SerializeField] float highestExtreme;
    [SerializeField] float extremeCountingInterval;
    [SerializeField] float strengthScalar = 1f;
    [SerializeField] float maxArrowSize = 25;
    [SerializeField] float contourScalar = 1f;
    [SerializeField] float radialInterval;
    [SerializeField] int numberContourLayers = 4;
    [SerializeField] float[] angleInterval;
    [SerializeField] int maxRDist = 6;
    [SerializeField] bool stengthSquared = false;
    [SerializeField] Transform generalCanvas;
    [SerializeField] Transform helpCanvas;
    [SerializeField] Transform arrowCanvas;
    [SerializeField] float[] layers;
    HUDController hud;

    public List<Charge> charges;
    public List<GameObject> fieldArrows;
    public List<GameObject> contourPoints;
    public bool arrowCanvasOn = false;

    float dist;
    float savedDist;
    Vector3 savedStart;
    Quaternion savedRotation;
    bool fieldsOn = false;
    bool arrowsOn = true;
    bool contourOn = true;
    public int currentCharge = -1;
    public int currentArrow = -1;
    // Start is called before the first frame update
    void Start()
    {
        arrowBackButton.onClick.AddListener(arrowBackFunction);
        helpBackButton.onClick.AddListener(delegate
        {
            helpGeneral(true);
        });
        helpButton.onClick.AddListener(delegate
        {
            helpGeneral(false);
        });
        hideShowFields.onClick.AddListener(delegate {
            hideShow(fieldArrows, arrowsOn);
            arrowsOn = !arrowsOn;
	    });
        hideShowContours.onClick.AddListener(delegate
        {
            hideShow(contourPoints, contourOn);
            contourOn = !contourOn;
        });
        compileFieldsButton.onClick.AddListener(produceFields);
        deleteCurrentCharge.onClick.AddListener(deleteCharge);
        newChargeButton.onClick.AddListener(newCharge);
        dist = Vector3.Distance(pivitCenter.transform.position, camera.transform.position);
        savedDist = dist;
        savedStart = camera.transform.position;
        savedRotation = camera.transform.rotation;
        hud = wheelButton.GetComponent<HUDController>();
        up.onClick.AddListener(upFunc);
        down.onClick.AddListener(downFunc);
        right.onClick.AddListener(rightFunc);
        left.onClick.AddListener(leftFunc);
        reset.onClick.AddListener(resetFunc);
        zoomIn.onClick.AddListener(zoomInFunc);
        zoomOut.onClick.AddListener(zoomOutFunc);
    }
    List<int> triangleAdd(List<int> existing, List<int> previous, List<int> current, List<int> future, List<PointParts> partPoint, List<Vector3> verts)
    {
        List<int> triangle = existing;
        if (current.Count > 0)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < current.Count; j++)
            {
                temp.Add(current[j]);
            }
            int currentIndex = 0;
            int fun = 0;
            while (temp.Count > 0 && fun < current.Count + 1)
            {
                int closeIndex = 0;
                int removeIndex = 0;
                float smallerDist = 1000;
                for (int k = 0; k < temp.Count; k++)
                {
                    if (temp[k] != current[currentIndex])
                    {
                        if (Vector3.Distance(verts[current[currentIndex]], verts[temp[k]]) < smallerDist)
                        {
                            closeIndex = k;
                            smallerDist = Vector3.Distance(verts[current[currentIndex]], verts[temp[k]]);
                        }
                    }
                    else
                    {
                        removeIndex = k;
                    }
                }
                if (smallerDist < 900)
                {
                    if (previous.Count > 0)
                    {
                        triangle.Add(current[currentIndex]);
                        triangle.Add(temp[closeIndex]);
                        float smallDist = 1000;
                        int smallIndex = 0;
                        for (int s = 0; s < previous.Count; s++)
                        {
                            Vector3 average = (verts[current[currentIndex]] + verts[temp[closeIndex]]) / 2;
                            if (Vector3.Distance(average, verts[previous[s]]) < smallDist && partPoint[previous[s]].backCombine != average)
                            {
                                smallDist = Vector3.Distance(average, verts[previous[s]]);
                                smallIndex = s;
                                partPoint[previous[s]].backCombine = average;
                            }
                        }
                        triangle.Add(previous[smallIndex]);
                        triangle.Add(previous[smallIndex]);
                        triangle.Add(temp[closeIndex]);
                        triangle.Add(current[currentIndex]);
                    }
                    if (future.Count > 0)
                    {
                        triangle.Add(current[currentIndex]);
                        triangle.Add(temp[closeIndex]);
                        float smallDist2 = 1000;
                        int smallIndex2 = 0;
                        for (int s = future.Count - 1; s >= 0; s--)
                        {
                            Vector3 average = (verts[current[currentIndex]] + verts[temp[closeIndex]]) / 2;
                            if (Vector3.Distance(average, verts[future[s]]) < smallDist2 && partPoint[future[s]].forwardCombine != average)
                            {
                                smallDist2 = Vector3.Distance(average, verts[future[s]]);
                                smallIndex2 = s;
                                partPoint[future[s]].forwardCombine = average;
                            }
                        }
                        triangle.Add(future[smallIndex2]);
                        triangle.Add(future[smallIndex2]);
                        triangle.Add(temp[closeIndex]);
                        triangle.Add(current[currentIndex]);
                    }
                }
                int midde = temp[closeIndex];
                temp.RemoveAt(removeIndex);
                if (fun == 1)
                {
                    temp.Add(current[0]);
                }
                for (int j = 0; j < current.Count; j++)
                {
                    if (current[j] == midde)
                    {
                        currentIndex = j;
                    }
                }
                fun++;
            }
        }
        return triangle;
    }
    Vector3 averageList(List<int> range, List<Vector3> verts)
    {
        Vector3 sum = new Vector3(0, 0, 0);
        for (int i = 0; i < range.Count; i++)
        {
            sum += verts[range[i]];
        }
        sum /= range.Count;
        return sum;
    }
    void cameraControl() {
        float deltaX = hud.direction.x - hud.revised.x;
        float deltaY = hud.direction.y - hud.revised.y;
        float angle = deltaX * cameraMult;
        if (deltaX != 0)
        {
            camera.transform.Rotate(new Vector3(0f, angle, 0f));
        }
        if (deltaY != 0 && camera.transform.rotation.eulerAngles.x + (deltaY * cameraMult) < cameraMax && camera.transform.rotation.eulerAngles.x + (deltaY * cameraMult) > cameraMin) {
            camera.transform.Rotate(new Vector3(deltaY * cameraMult, 0f, 0f));
        }
        float xpos;
        float zpos;
        float angleCam = (camera.transform.rotation.eulerAngles.y % 90) * Mathf.Deg2Rad;
        float camY = (camera.transform.rotation.eulerAngles.x % 90) * Mathf.Deg2Rad;
        float xzdist = Mathf.Cos(camY) * dist;
        float ypos = Mathf.Sin(camY) * dist;
        if (camera.transform.rotation.eulerAngles.y >= 0 && camera.transform.rotation.eulerAngles.y <= 90)
        {
            xpos = Mathf.Sin(angleCam) * xzdist * -1;
            zpos = Mathf.Cos(angleCam) * xzdist * -1;
        }
        else if (camera.transform.rotation.eulerAngles.y > 90 && camera.transform.rotation.eulerAngles.y <= 180)
        {
            xpos = Mathf.Cos(angleCam) * xzdist * -1;
            zpos = Mathf.Sin(angleCam) * xzdist;
        }
        else if (camera.transform.rotation.eulerAngles.y > 180 && camera.transform.rotation.eulerAngles.y <= 270)
        {
            xpos = Mathf.Sin(angleCam) * xzdist;
            zpos = Mathf.Cos(angleCam) * xzdist;
        }
        else
        {
            xpos = Mathf.Cos(angleCam) * xzdist;
            zpos = Mathf.Sin(angleCam) * xzdist * -1;
        }
        camera.transform.position = new Vector3(xpos + pivitCenter.transform.position.x, ypos + pivitCenter.transform.position.y, zpos + pivitCenter.transform.position.z);
        camera.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y, 0f);
    }
    void upFunc() {
        if (pivitCenter.transform.position.y + moveFactorCamera < maxYMovement)
        {
            pivitCenter.transform.position = new Vector3(pivitCenter.transform.position.x, pivitCenter.transform.position.y + moveFactorCamera, pivitCenter.transform.position.z);
        }
    }
    void downFunc() {
        if (pivitCenter.transform.position.y - moveFactorCamera > minYMovement)
        {
            pivitCenter.transform.position = new Vector3(pivitCenter.transform.position.x, pivitCenter.transform.position.y - moveFactorCamera, pivitCenter.transform.position.z);
        }
    }
    void rightFunc() {
        float xpos = Mathf.Sin((camera.transform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad) * moveFactorCamera;
        float zpos = Mathf.Cos((camera.transform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad) * moveFactorCamera;
        if (pivitCenter.transform.position.x + xpos < outerMost && pivitCenter.transform.position.x + xpos > -1 * outerMost && pivitCenter.transform.position.z + zpos < outerMost && pivitCenter.transform.position.z + zpos > -1 * outerMost)
        {
            pivitCenter.transform.position = new Vector3(pivitCenter.transform.position.x + xpos, pivitCenter.transform.position.y, pivitCenter.transform.position.z + zpos);
        }
    }
    void leftFunc()
    {
        float xpos = Mathf.Sin((camera.transform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad) * moveFactorCamera;
        float zpos = Mathf.Cos((camera.transform.rotation.eulerAngles.y + 90)* Mathf.Deg2Rad) * moveFactorCamera;
        if (pivitCenter.transform.position.x - xpos < outerMost && pivitCenter.transform.position.x - xpos > -1 * outerMost && pivitCenter.transform.position.z - zpos < outerMost && pivitCenter.transform.position.z - zpos > -1 * outerMost)
        {
            pivitCenter.transform.position = new Vector3(pivitCenter.transform.position.x - xpos, pivitCenter.transform.position.y, pivitCenter.transform.position.z - zpos);
        }
    }
    void zoomInFunc() {
        if (dist - zoomFactor > minZoom)
        {
            dist -= zoomFactor;
        }
    }
    void zoomOutFunc() {
        if (dist + zoomFactor < maxZoom)
        {
            dist += zoomFactor;
        }
    }
    void resetFunc() {
        camera.transform.position = savedStart;
        camera.transform.rotation = savedRotation;
        dist = savedDist;
        pivitCenter.transform.position = new Vector3(0, 0, 0);
    }
    void Update()
    {
        cameraControl();
        if (currentCharge != -1)
        {
            float.TryParse(xposField.text, out charges[currentCharge].xpos);
            float.TryParse(yposField.text, out charges[currentCharge].ypos);
            float.TryParse(zposField.text, out charges[currentCharge].zpos);
            float.TryParse(chargeField.text, out charges[currentCharge].charge);
            charges[currentCharge].name = nameField.text;
        }
    }
    void setFieldsNew (Charge chargeScript)
    {
        chargeField.text = chargeScript.charge.ToString();
        nameField.text = chargeScript.name.ToString();
        xposField.text = chargeScript.xpos.ToString();
        yposField.text = chargeScript.ypos.ToString();
        zposField.text = chargeScript.zpos.ToString();
    }
    void newCharge ()
    {
        GameObject obj = Instantiate(sampleCharge);
        Charge chargeScript = obj.GetComponent<Charge>();
        chargeScript.name = "charge " + (charges.Count + 1).ToString();
        chargeScript.camera = camera;
        chargeScript.mainner = this;
        setFieldsNew(chargeScript);
        currentCharge = charges.Count;
        charges.Add(chargeScript);
        obj.SetActive(true);
        generalCanvas.gameObject.SetActive(true);
        arrowCanvas.gameObject.SetActive(false);
    }
    public void changeCharge ()
    {
        int onCharge = -1;
        generalCanvas.gameObject.SetActive(true);
        arrowCanvas.gameObject.SetActive(false);
        for(int i = 0; i < charges.Count; i++)
        {
            if (charges[i].touched)
            {
                onCharge = i;
                charges[i].touched = false;
            }
        }
        currentCharge = onCharge;
        setFieldsNew(charges[onCharge]);
    }
    void deleteCharge ()
    {
        if (currentCharge != -1)
        {
            generalCanvas.gameObject.SetActive(true);
            arrowCanvas.gameObject.SetActive(false);
            charges[currentCharge].gameObject.SetActive(false);
            charges.RemoveAt(currentCharge);
            currentCharge = -1;
        }
    }
    PointParts pointer(int index, Vector3 coordinates)
    {
        PointParts tempPoint = new PointParts();
        tempPoint.index = index;
        tempPoint.position = coordinates;
        tempPoint.positionArray = new float[] { coordinates.x, coordinates.y, coordinates.z };
        return tempPoint;
    }
    void produceFields()
    {
        if (!fieldsOn)
        {
            fieldsOn = true;
            //This varible is used but where it is used is commented out
            int countering = 0;
            PointParts[] contourLayers = new PointParts[numberContourLayers];
            float[] contourLayerLimits = layers;
            //field arrows
            for (float x = lowestExtreme; x <= highestExtreme; x += extremeCountingInterval)
            {
                PointParts[] queues = new PointParts[contourLayers.Length];
                for (int k = 0; k < queues.Length; k++)
                {
                    queues[k] = new PointParts();
                }
                for (float y = lowestExtreme; y <= highestExtreme; y += extremeCountingInterval)
                {
                    for (float z = lowestExtreme; z <= highestExtreme; z += extremeCountingInterval)
                    {
                        float xMag = 0;
                        float yMag = 0;
                        float zMag = 0;
                        float contour = 0;
                        for (int i = 0; i < charges.Count; i++)
                        {
                            if (!(charges[i].gameObject.transform.position.x == x && charges[i].gameObject.transform.position.y == y && charges[i].gameObject.transform.position.z == z))
                            {
                                float strength = 0;
                                if (stengthSquared)
                                {
                                    strength = Mathf.Abs(charges[i].charge) / Mathf.Pow(Vector3.Distance(charges[i].gameObject.transform.position, new Vector3(x, y, z)), 2);
                                }
                                else
                                {
                                    strength = Mathf.Abs(charges[i].charge) / Vector3.Distance(charges[i].gameObject.transform.position, new Vector3(x, y, z));
                                }
                                contour += charges[i].charge / Vector3.Distance(charges[i].gameObject.transform.position, new Vector3(x, y, z));
                                float phi1 = Mathf.Asin((charges[i].gameObject.transform.position.y - y) / Vector3.Distance(charges[i].gameObject.transform.position, new Vector3(x, y, z)));
                                if (z < charges[i].gameObject.transform.position.z)
                                {
                                    phi1 += Mathf.PI;
                                }
                                if (charges[i].charge < 0)
                                {
                                    phi1 += Mathf.PI;
                                    phi1 *= -1;
                                }
                                float theta1 = 0;
                                if (charges[i].gameObject.transform.position.z == z)
                                {
                                    if (charges[i].gameObject.transform.position.x <= x)
                                    {
                                        theta1 = -Mathf.PI / 2;
                                    }
                                    else
                                    {
                                        theta1 = Mathf.PI / 2;
                                    }
                                    phi1 += Mathf.PI;
                                    phi1 *= -1;
                                }
                                else
                                {
                                    theta1 = Mathf.Atan((charges[i].gameObject.transform.position.x - x) / (charges[i].gameObject.transform.position.z - z));
                                }
                                xMag += Mathf.Cos(phi1) * strength * Mathf.Sin(theta1);
                                zMag += Mathf.Cos(phi1) * strength * Mathf.Cos(theta1);
                                if (charges[i].gameObject.transform.position.x == x && charges[i].gameObject.transform.position.z == z)
                                {
                                    if (charges[i].charge < 0)
                                    {
                                        yMag += -1 * Mathf.Abs(Mathf.Sin(phi1) * strength);
                                    }
                                    else
                                    {
                                        yMag += Mathf.Abs(Mathf.Sin(phi1) * strength);
                                    }
                                }
                                else
                                {
                                    yMag += Mathf.Sin(phi1) * strength;
                                }
                            }
                        }

                        if (!(xMag == 0 && yMag == 0 && zMag == 0))
                        {

                            GameObject obj = Instantiate(sampleFieldArrow);
                            FieldArrowControl arrow = obj.GetComponent<FieldArrowControl>();
                            obj.transform.position = new Vector3(x, y, z);
                            float phi = Mathf.Asin(yMag / Mathf.Sqrt(Mathf.Pow(xMag, 2) + Mathf.Pow(yMag, 2) + Mathf.Pow(zMag, 2))) * Mathf.Rad2Deg;
                            if (zMag <= 0)
                            {
                                phi += 180;
                            }
                            float theta = Mathf.Atan(xMag / zMag) * Mathf.Rad2Deg;
                            arrow.contourStrength = contour;
                            arrow.phi = phi;
                            arrow.theta = theta;
                            arrow.arrowStrength = Mathf.Sqrt(Mathf.Pow(xMag, 2) + Mathf.Pow(yMag, 2) + Mathf.Pow(zMag, 2));
                            arrow.arrowIndex = countering;
                            countering++;
                            if (Mathf.Abs(zMag) < 0.0001 && Mathf.Abs(xMag) < 0.0001)
                            {
                                //phi *= -1;
                            }
                            obj.transform.rotation = Quaternion.Euler(phi, theta, 0f);
                            float localScaleAdd = Mathf.Sqrt(Mathf.Pow(xMag, 2) + Mathf.Pow(yMag, 2) + Mathf.Pow(zMag, 2)) / strengthScalar;
                            if (localScaleAdd > maxArrowSize)
                            {
                                localScaleAdd = maxArrowSize;
                            }
                            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, localScaleAdd);
                            obj.SetActive(true);
                            fieldArrows.Add(obj);
                        }
                    }
                }
            }
        }
        else
        {
            fieldsOn = false;
            for (int i = 0; i < fieldArrows.Count; i++)
            {
                Destroy(fieldArrows[i]);
            }
            for (int i = 0; i < contourPoints.Count; i++)
            {
                Destroy(contourPoints[i]);
            }
            fieldArrows.Clear();
            contourPoints.Clear();
            arrowsOn = true;
            contourOn = true;
        }
    }
    void hideShow (List<GameObject> types, bool onOff)
    {
        if (fieldsOn)
        {
            for (int i = 0; i < types.Count; i++)
            {
                types[i].gameObject.SetActive(!onOff);
            }
        }
    } 
    void helpGeneral(bool onOff)
    {
        generalCanvas.gameObject.SetActive(onOff);
        helpCanvas.gameObject.SetActive(!onOff);
    }
    void arrowBackFunction ()
    {
        arrowCanvas.gameObject.SetActive(false);
        generalCanvas.gameObject.SetActive(true);
        fieldArrows[currentArrow].gameObject.GetComponent<Renderer>().material = fieldArrows[currentArrow].gameObject.GetComponent<FieldArrowControl>().offMaterial;
    }
}