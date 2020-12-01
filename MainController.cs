using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    public Lenscrafter crafter;
    [SerializeField] GameObject[] pieces;
    [SerializeField] Button[] piecePlacer;
    [SerializeField] float[] yPieceDisplacemnent;
    public GameObject[] canvases;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject onPhysical;
    [SerializeField] Camera camera;
    [SerializeField] GameObject baseLayer;
    [SerializeField] GameObject axisLayer;
    [SerializeField] GameObject generalExperimentContainer;
    public Transform centralBase;
    [SerializeField] int sideBaseLength;
    [SerializeField] float addLength;
    [SerializeField] float specialAdd;
    [SerializeField] float baseW;
    [SerializeField] float baseA;
    [SerializeField] float timerIntergral;
    [SerializeField] float waveSpeed;
    [SerializeField] GameObject wheelButton;
    [SerializeField] float cameraMult;
    [SerializeField] float cameraMax = 85;
    [SerializeField] float cameraMin = 5;
    [SerializeField] GameObject pivitCenter;
    [SerializeField] Button up;
    [SerializeField] Button down;
    [SerializeField] Button right;
    [SerializeField] Button left;
    [SerializeField] Button zoomIn;
    [SerializeField] Button zoomOut;
    [SerializeField] Button reset;
    [SerializeField] Button pauseButton;
    [SerializeField] Button clearButton;
    [SerializeField] float moveFactorCamera;
    [SerializeField] float minYMovement;
    [SerializeField] float maxYMovement;
    [SerializeField] float outerMost;
    [SerializeField] float zoomFactor;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] float surfaceVerticalOffset;
    [SerializeField] float surfaceHorizontalOffset;
    [SerializeField] float baseCanvasWidth;
    [SerializeField] float baseCanvasHeight;
    public float circleAmp = 5;
    [SerializeField] Button switchModeBut;
    [SerializeField] Button constructBut;
    [SerializeField] Button backToTable;
    [SerializeField] Button surfaceSelector;
    [SerializeField] Text switchModeText; 
    public GameObject physicalObjects;
    [SerializeField] GameObject geometricObjects;
    [SerializeField] Transform geometricMainCanvas;
    [SerializeField] Transform helpActiveCanvas;
    [SerializeField] Button helpActiveButton;
    [SerializeField] Button backHelpButton;
    HUDController hud;
    public bool placer = false;
    public bool emit = true;
    public int mode = 0;
    public int surfaceSecondCount = 7;
    public refractingSurface[,] surfaces;
    public Transform[] surfaceCanvas = new Transform[3];
    public RectTransform[] surfaceCanvasStartingPosition = new RectTransform[3];
    public GameObject[,] surfaceButtons;
    [SerializeField] Button[] deleteType = new Button[3];
    public int[] surfaceCounter = new int[3];
    public int[] currentPieceType = new int[3] {-1, -1, -1};
    public float dialationSize = 0.1f; 
    int smallTimer = 0;
    int placeIndex = 0;
    int currentIndex = 0;
    float dist;
    float savedDist;
    Vector3 savedStart;
    Quaternion savedRotation;
    GameObject[] optics = new GameObject[100];
    List<GameObject> piecesHolder = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        onPhysical.gameObject.SetActive(false);
        onPhysical.gameObject.SetActive(true);
        clearButton.onClick.AddListener(clearFunc);
        helpActiveButton.onClick.AddListener(delegate { helpActiveCanvas.gameObject.SetActive(true); mainCanvas.gameObject.SetActive(false); });
        backHelpButton.onClick.AddListener(delegate { helpActiveCanvas.gameObject.SetActive(false); mainCanvas.gameObject.SetActive(true); });
        surfaces = new refractingSurface[3,surfaceSecondCount];
        surfaceButtons = new GameObject[3,surfaceSecondCount];
        deleteType[0].onClick.AddListener(delegate{deletePieceType(0);});
        deleteType[1].onClick.AddListener(delegate{deletePieceType(1);});
        deleteType[2].onClick.AddListener(delegate{deletePieceType(2);});
        dist = Vector3.Distance(pivitCenter.transform.position, camera.transform.position);
        savedDist = dist;
        savedStart = camera.transform.position;
        savedRotation = camera.transform.rotation;
        hud = wheelButton.GetComponent<HUDController>();
        piecePlacer[0].onClick.AddListener(delegate { placement(0); });
        piecePlacer[1].onClick.AddListener(delegate { placement(1); });
        piecePlacer[2].onClick.AddListener(delegate { placement(2); });
        piecePlacer[3].onClick.AddListener(delegate { placement(3); });
        piecePlacer[4].onClick.AddListener(delegate { placement(4); });
        piecePlacer[5].onClick.AddListener(delegate { placement(5); });
        piecePlacer[6].onClick.AddListener(delegate { placement(6); });
        piecePlacer[7].onClick.AddListener(delegate { placement(7); });
        pauseButton.onClick.AddListener(pauseFunc);
        up.onClick.AddListener(upFunc);
        down.onClick.AddListener(downFunc);
        right.onClick.AddListener(rightFunc);
        left.onClick.AddListener(leftFunc);
        reset.onClick.AddListener(resetFunc);
        zoomIn.onClick.AddListener(zoomInFunc);
        zoomOut.onClick.AddListener(zoomOutFunc);
        switchModeBut.onClick.AddListener(switchMode);
        backToTable.onClick.AddListener(delegate{constructing(true);});
        constructBut.onClick.AddListener(delegate{constructing(false);});
        for (float width = -1 * sideBaseLength; width <= sideBaseLength; width+= addLength)
        {
            for (float length = -1 * sideBaseLength; length <= sideBaseLength; length+= addLength) {
                GameObject obj = Instantiate(baseLayer);
                obj.transform.position = centralBase.position + new Vector3(centralBase.position.x + width, centralBase.position.y, centralBase.position.z + length);
                obj.transform.SetParent(physicalObjects.transform);
                obj.SetActive(true);
            }
        }
        for (float width = -1 * sideBaseLength; width <= sideBaseLength; width += specialAdd)
        {
            if (width != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float[] offsets = { 0f, 0f, 0f };
                    offsets[i] = width;
                    GameObject obj = Instantiate(axisLayer);
                    obj.transform.position = centralBase.position + new Vector3(centralBase.position.x + offsets[0], centralBase.position.y + offsets[1], centralBase.position.z + offsets[2]);
                    obj.transform.SetParent(geometricObjects.transform);
                    obj.SetActive(true);
                }
            }
        }
    }
    void placement(int index)
    {
        placer = true;
        placeIndex = index;
        smallTimer = 0;
    }
    void place()
    {
        if (placer)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonUp(0) && smallTimer >= 1)
            {
                GameObject obj = Instantiate(pieces[placeIndex]);
                piecesHolder.Add(obj);
                obj.transform.SetParent(generalExperimentContainer.transform);
                PieceController idsetting = obj.GetComponent<PieceController>();
                idsetting.id = currentIndex;
                idsetting.baseWavelength = baseW;
                idsetting.baseAmplitude = baseA;
                idsetting.timerInt = timerIntergral;
                idsetting.waveSpeed = waveSpeed;
                optics[currentIndex] = obj;
                currentIndex++;
                obj.transform.position = hit.transform.position + new Vector3(0, yPieceDisplacemnent[placeIndex], 0);
                obj.SetActive(true);
                placer = false;
                placeIndex = 0;
            }
            smallTimer++;
        }
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
    void pauseFunc() {
        if (emit)
        {
            emit = false;
        }
        else {
            emit = true;
        }
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
    void clearFunc () {
        for (int i = 0; i < piecesHolder.Count; i++) {
            Destroy(piecesHolder[i]);
        }
        piecesHolder.Clear();
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
    void switchMode() {
        if (mode == 0)
        {
            mode = 1;
            switchModeText.text = "Current Mode: Geometric";
        }
        else {
            mode = 0;
            switchModeText.text = "Current Mode: Physical";
        }
    }
    void constructing(bool direction) {
        if (!direction) {
            for (int i = 0; i < canvases.Length; i++) {
                canvases[i].SetActive(direction);
            } 
        }
        mainCanvas.SetActive(direction);
        if (helpActiveCanvas.gameObject.active) {
            helpActiveCanvas.gameObject.SetActive(direction);
        }
        physicalObjects.SetActive(direction);
        geometricObjects.SetActive(!direction);
        geometricMainCanvas.gameObject.SetActive(!direction);
        onPhysical.SetActive(direction);
        emit = true;
    }
    // Update is called once per frame
    void Update()
    {
        cameraControl();
        place();
    }
    public void surfaceCanvasUpdate (int surfaceType) {
        for (int i = 0; i < surfaceSecondCount; i++) {
            if (surfaceButtons[surfaceType, i] != null) {
                GameObject obj3 = Instantiate(crafter.surfaceTestButton);
                obj3.transform.SetParent(surfaceCanvas[surfaceType]);
                obj3.SetActive(true);
                int constant = i;
                obj3.GetComponent<Button>().onClick.AddListener(delegate{currentPieceType[surfaceType] = constant;});
                obj3.GetComponentInChildren<Text>().text = surfaceButtons[surfaceType, i].GetComponentInChildren<Text>().text;
                Destroy(surfaceButtons[surfaceType, i]);
                surfaceButtons[surfaceType, i] =  obj3;
                RectTransform recter = surfaceButtons[surfaceType, i].GetComponent<RectTransform>();
                RectTransform genRect = surfaceCanvas[surfaceType].GetComponent<RectTransform>();
                if (i > surfaceSecondCount / 2 -1) {
                    recter.localPosition = new Vector3(surfaceCanvasStartingPosition[surfaceType].GetComponent<RectTransform>().localPosition.x + surfaceHorizontalOffset, -1 * (i - 2) * surfaceVerticalOffset + surfaceCanvasStartingPosition[surfaceType].GetComponent<RectTransform>().localPosition.y);
                } else {
                    recter.localPosition = new Vector3(surfaceCanvasStartingPosition[surfaceType].GetComponent<RectTransform>().localPosition.x, -1 * i * surfaceVerticalOffset + surfaceCanvasStartingPosition[surfaceType].GetComponent<RectTransform>().localPosition.y);
                }
                Vector3 storage = recter.localPosition;
                recter.anchorMax = new Vector2(0.5f, 0.5f);
                recter.anchorMin = new Vector2(0.5f, 0.5f);
                recter.localPosition = storage;
                bool tempBool = false;
                if (surfaceCanvas[surfaceType].gameObject.active) {
                    tempBool = true;
                }
                surfaceCanvas[surfaceType].gameObject.SetActive(true);
                recter.localScale = new Vector3(genRect.rect.width / baseCanvasWidth, genRect.rect.height / baseCanvasHeight, 1);
                recter.anchorMin = new Vector2((recter.localPosition.x - recter.rect.width / 2 + genRect.rect.width / 2) / genRect.rect.width, (recter.localPosition.y - recter.rect.height / 2 + genRect.rect.height / 2) / genRect.rect.height);
                recter.anchorMax = new Vector2((recter.localPosition.x + recter.rect.width / 2 + genRect.rect.width / 2) / genRect.rect.width, (recter.localPosition.y + recter.rect.height / 2 + genRect.rect.height / 2) / genRect.rect.height);
                if (!tempBool) {
                    surfaceCanvas[surfaceType].gameObject.SetActive(false);
                }
                recter.offsetMin = Vector2.zero;
                recter.offsetMax = Vector2.zero;
            }
        }
    }
    void deletePieceType (int type) {
        if (currentPieceType[type] >= 0) {
            Destroy(surfaceButtons[type, currentPieceType[type]]);
            surfaceButtons[type, currentPieceType[type]] = null;
            surfaces[type, currentPieceType[type]] = null;
            for (int i = currentPieceType[type]; i < surfaceSecondCount; i++) {
                if (i + 1 < surfaceSecondCount) {
                    GameObject temp = surfaceButtons[type, i+1];
                    surfaceButtons[type, i + 1] = surfaceButtons[type, i];
                    surfaceButtons[type, i] = temp;
                    refractingSurface tempSurf = surfaces[type, i+1];
                    surfaces[type, i + 1] = surfaces[type, i];
                    surfaces[type, i] = tempSurf;
                    for (int j = 0; j < 2; j++) {
                        if (surfaces[type, i + j] != null) {
                            surfaces[type, i + j].index = i + j;
                        }
                    }
                }
            }
            currentPieceType[type] = -1;
            surfaceCounter[type]--;
            surfaceCanvasUpdate(type);
        }
    }
}