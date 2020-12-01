using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class refractingSurface : MonoBehaviour
{
    public float[] posX;
    public float[] posY;
    public float[] posZ;
    public float[] velX;
    public float[] velY;
    public float[] velZ;
    public float[] angleI;
    public float[] angleJ;
    public float[] angleK;
    public float[] angleL;
    public ItemManager namer;
    public int index;
    public int type;
    public Lenscrafter mainer;
    public GameObject mainbut;
    public float[] placeHold; 
    void Start()
    {

    }
    void Update()
    {
        /*
        if (buttonStarter && mainer.mainer.surfaceButtons[type, index].gameObject.active) {
            mainer.mainer.surfaceButtons[type, index].GetComponent<Button>().onClick.AddListener(delegate{ mainer.mainer.currentPieceType[type] = index;});
            buttonStarter = false;
        }*/
    }
    public int[] narrowDown (float comparision, float[] angleCompare, int upperLimit, int lowerLimit) {
        float smallDist = 1000;
        int startIndex = 0;
        for (int i = lowerLimit; i <= upperLimit; i++) {
            if (Mathf.Abs(comparision - angleCompare[i]) < smallDist) {
                smallDist = Mathf.Abs(comparision - angleCompare[i]);
                startIndex = i;
            }
        }
        int endIndex = startIndex;
        while (Mathf.Abs(comparision - angleCompare[endIndex + 1]) == smallDist && endIndex + 2 < upperLimit) {
            endIndex++;
        }
        return new int[2] {startIndex, endIndex};
    }
    public void lightRefractionUpdate (PieceController idSetting, GameObject other, MainController scripter, GameObject center, int typer) {
        float rotation = idSetting.values[2];
        Light otherLight = other.gameObject.GetComponent<Light>();
        float xOff = center.transform.position.x - other.transform.position.x;
        float yOff = center.transform.position.y - other.transform.position.y;
        float zOff = center.transform.position.z - other.transform.position.z;
        float xzOff = Mathf.Sqrt(Mathf.Pow(xOff, 2) + Mathf.Pow(zOff, 2));
        float angleI = ((Mathf.Atan(yOff / xzOff) + rotation) % 360) * Mathf.Rad2Deg;
        float angleJ = Mathf.Atan(zOff / xOff) * Mathf.Rad2Deg;
        float angleK = Mathf.Atan(otherLight.velocityZ / Mathf.Sqrt(Mathf.Pow(otherLight.velocityX, 2) + Mathf.Pow(otherLight.velocityZ, 2))) * Mathf.Rad2Deg;
        float angleL = Mathf.Atan(otherLight.velocityZ / otherLight.velocityX) * Mathf.Rad2Deg;
        int[,] placeHold= new int[4, 2];
        refractingSurface ref1 = new refractingSurface();
        ref1.placeHold = scripter.surfaces[typer, scripter.currentPieceType[typer]].angleI;
        refractingSurface ref2 = new refractingSurface();
        ref2.placeHold = scripter.surfaces[typer, scripter.currentPieceType[typer]].angleJ;
        refractingSurface ref3 = new refractingSurface();
        ref3.placeHold = scripter.surfaces[typer, scripter.currentPieceType[typer]].angleK;
        refractingSurface ref4 = new refractingSurface();
        ref4.placeHold = scripter.surfaces[typer, scripter.currentPieceType[typer]].angleL;
        refractingSurface[] placeSurfaces = new refractingSurface[4] {ref1, ref2, ref3, ref4};
        refractingSurface currentSurface = scripter.surfaces[typer, scripter.currentPieceType[typer]];
        float[] placeFloat = new float[4] {angleI, angleJ, angleK, angleL};
        for (int i = 0; i < placeFloat.Length; i++) {
            if (placeFloat[i] < 0) {
                placeFloat[i] = Mathf.Abs(placeFloat[i]);
            }
        }
        for (int i = 0; i < 4; i++) {
            int[] place = new int[2];
            if (i == 0) {
                place = narrowDown(placeFloat[i], placeSurfaces[i].placeHold, scripter.surfaces[typer, scripter.currentPieceType[typer]].angleI.Length - 1, 0);
            } else {
                place = narrowDown(placeFloat[i], placeSurfaces[i].placeHold, placeHold[i - 1, 1], placeHold[i - 1, 0]);
            }
            placeHold[i, 0] = place[0];
            placeHold[i, 1] = place[1];
        }
        other.transform.position = new Vector3(currentSurface.posX[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.x, currentSurface.posY[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.y, currentSurface.posZ[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.z);
        otherLight.velocityX = currentSurface.velX[placeHold[3, 0]];
        otherLight.velocityY = currentSurface.velY[placeHold[3, 0]];
        otherLight.velocityZ = currentSurface.velZ[placeHold[3, 0]];
        //Debug.Log(currentSurface.posX[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.x + " posX " + currentSurface.posY[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.y + " posY " + currentSurface.posZ[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.z + " posZ " + currentSurface.velX[placeHold[3, 0]] + " velX " + currentSurface.velY[placeHold[3, 0]] + " posY " + currentSurface.velZ[placeHold[3, 0]] + " posZ " + angleI + " angleI " + angleJ + " angleJ " + angleK + " angleK " + angleL + " angleL");
    }
 }
