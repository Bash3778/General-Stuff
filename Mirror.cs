using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mirror : MonoBehaviour
{
    [SerializeField] MainController scripter;
    [SerializeField] int type = 0;
    [SerializeField] Collider sphere;
    [SerializeField] Collider plane;
    PieceController idSetting;
    //debug
    bool trying = true;
    // Start is called before the first frame update
    void Start()                           
    {
        idSetting = this.GetComponent<PieceController>();

    }
    //the below function is perfect
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Light")
        {
            if (scripter.mode == 0) {
                sphere.enabled = false;
                plane.enabled = true;
                float rotation = idSetting.values[2];
                Light otherScript = other.gameObject.GetComponent<Light>();
                GameObject center = this.transform.GetChild(0).gameObject;
                float totalVelocity = Mathf.Sqrt(Mathf.Pow(otherScript.velocityX, 2) + Mathf.Pow(otherScript.velocityZ, 2));
                float zvel = Mathf.Sin(rotation * Mathf.Deg2Rad * 2) * totalVelocity;
                float xvel = Mathf.Cos(rotation * Mathf.Deg2Rad * 2) * totalVelocity;
                if (otherScript.velocityX < 0)
                {
                    xvel = xvel * -1;
                }
                if (otherScript.velocityZ < 0)
                {
                    zvel = zvel * -1;
                }
                float otherAngle = Mathf.Atan2(otherScript.velocityZ, otherScript.velocityX) * Mathf.Rad2Deg;
                otherScript.velocityZ = zvel;
                otherScript.velocityX = xvel;
                float netAngle = rotation - otherAngle;
                float xdist = other.transform.position.x - center.transform.position.x;
                float zdist = other.transform.position.z - center.transform.position.z;
                float xzdist = Mathf.Sqrt(Mathf.Pow(xdist, 2) + Mathf.Pow(zdist, 2));
                float xpos = Mathf.Cos((rotation + netAngle) * Mathf.Deg2Rad) * xzdist;
                float zpos = Mathf.Sin((rotation + netAngle) * Mathf.Deg2Rad) * xzdist;
                if (zdist < 0)
                {
                    xpos = xpos * -1;
                }
                if (xdist < 0) {
                    zpos = zpos * -1;
                }
                other.gameObject.transform.position = new Vector3(center.transform.position.x + xpos, other.transform.position.y, center.transform.position.z + zpos);
            } else {
                sphere.enabled = true;
                plane.enabled = false;
                if (scripter.currentPieceType[type] > -1) {
                    refractingSurface user = new refractingSurface();
                    GameObject center = this.transform.GetChild(0).gameObject;
                    user.lightRefractionUpdate(idSetting, other.gameObject, scripter, center, type);
                    /*
                    float rotation = idSetting.values[2];
                    //look for angle plus rotation nad then look at what the refracting surface says
                    Light otherLight = other.gameObject.GetComponent<Light>();
                    float xOff = center.transform.position.x - other.transform.position.x;
                    float yOff = center.transform.position.y - other.transform.position.y;
                    float zOff = center.transform.position.z - other.transform.position.z;
                    float xzOff = Mathf.Sqrt(Mathf.Pow(xOff, 2) + Mathf.Pow(zOff, 2));
                    float angleI = (Mathf.Atan(yOff / xzOff) + rotation) * Mathf.Rad2Deg;
                    float angleJ = Mathf.Atan(zOff / xOff) * Mathf.Rad2Deg;
                    float angleK = Mathf.Atan(otherLight.velocityZ / Mathf.Sqrt(Mathf.Pow(otherLight.velocityX, 2) + Mathf.Pow(otherLight.velocityZ, 2))) * Mathf.Rad2Deg;
                    float angleL = Mathf.Atan(otherLight.velocityZ / otherLight.velocityX) * Mathf.Rad2Deg;
                    int[,] placeHold= new int[4, 2];
                    refractingSurface ref1 = new refractingSurface();
                    ref1.placeHold = scripter.surfaces[type, scripter.currentPieceType[type]].angleI;
                    refractingSurface ref2 = new refractingSurface();
                    ref2.placeHold = scripter.surfaces[type, scripter.currentPieceType[type]].angleJ;
                    refractingSurface ref3 = new refractingSurface();
                    ref3.placeHold = scripter.surfaces[type, scripter.currentPieceType[type]].angleK;
                    refractingSurface ref4 = new refractingSurface();
                    ref4.placeHold = scripter.surfaces[type, scripter.currentPieceType[type]].angleL;
                    refractingSurface[] placeSurfaces = new refractingSurface[4] {ref1, ref2, ref3, ref4};
                    refractingSurface currentSurface = scripter.surfaces[type, scripter.currentPieceType[type]];
                    float[] placeFloat = new float[4] {angleI, angleJ, angleK, angleL};
                    for (int i = 0; i < placeFloat.Length; i++) {
                        if (placeFloat[i] < 0) {
                            placeFloat[i] = Mathf.Abs(placeFloat[i]);
                        }
                    }
                    for (int i = 0; i < 4; i++) {
                        int[] place = new int[2];
                        if (i == 0) {
                            place = narrowDown(placeFloat[i], placeSurfaces[i].placeHold, scripter.surfaces[type, scripter.currentPieceType[type]].angleI.Length - 1, 0);
                        } else {
                            place = narrowDown(placeFloat[i], placeSurfaces[i].placeHold, placeHold[i - 1, 1], placeHold[i - 1, 0]);
                        }
                        placeHold[i, 0] = place[0];
                        placeHold[i, 1] = place[1];
                    }
                    //new type of colliders, so get rid of box colliders and get spherical colliders
                    other.transform.position = new Vector3(currentSurface.posX[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.x, currentSurface.posY[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.y, currentSurface.posZ[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.z);
                    otherLight.velocityX = currentSurface.velX[placeHold[3, 0]];
                    otherLight.velocityY = currentSurface.velY[placeHold[3, 0]];
                    otherLight.velocityZ = currentSurface.velZ[placeHold[3, 0]];*/
                    //Debug.Log(currentSurface.posX[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.x + " posX " + currentSurface.posY[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.y + " posY " + currentSurface.posZ[placeHold[3, 0]] * scripter.dialationSize + center.transform.position.z + " posZ " + currentSurface.velX[placeHold[3, 0]] + " velX " + currentSurface.velY[placeHold[3, 0]] + " posY " + currentSurface.velZ[placeHold[3, 0]] + " posZ " + angleI + " angleI " + angleJ + " angleJ " + angleK + " angleK " + angleL + " angleL");
                }
            }
        }
    }
    // Update is called once per frame
}