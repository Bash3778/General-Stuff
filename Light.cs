﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float velocityX;
    public float velocityZ;
    public float velocityY;
    public float polarizationAngle;
    public float rads;
    public float bamp;
    public float amplitude;
    public float currentIndRef = 1;
    public int indexSurface;
    public MainController mainner;
    public int counter;
    public bool split;
    public GameObject middle;
    public Vector3 focal = new Vector3(0, 0, 0);
    public bool lens = false;
    public bool testing = false;
    public Vector3 centerDistAmpl;
    public refractingSurface testSurface;

    [SerializeField] bool onset = false;

    float time = 3;
    bool set = false;
    public float angleUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float zeroOut(float numb, float compare) {
        if (compare < 0)
        {
            return Mathf.Abs(numb) * -1;
        }
        else
        {
            return Mathf.Abs(numb);
        }
    } 
    // Update is called once per frame
    void Update()
    {
        if (!testing) {
            if (mainner.emit)
            {
                if (lens)
                {
                    if (!set)
                    {
                        float totalVelocity = Mathf.Sqrt(Mathf.Pow(velocityY, 2) + Mathf.Pow(velocityZ, 2) + Mathf.Pow(velocityX, 2));
                        if (Mathf.Abs(focal.x) != Mathf.Infinity)
                        {
                            angleUp = Mathf.Atan(centerDistAmpl.y / Mathf.Sqrt(Mathf.Pow(centerDistAmpl.x, 2) + Mathf.Pow(centerDistAmpl.z, 2)));
                            float angleSide = Mathf.Atan(centerDistAmpl.z / centerDistAmpl.x);
                            velocityY = Mathf.Sin(angleUp) * totalVelocity;
                            float velocityXZ = Mathf.Cos(angleUp) * totalVelocity;
                            velocityX = Mathf.Cos(angleSide) * velocityXZ;
                            velocityZ = Mathf.Sin(angleSide) * velocityXZ;
                        }
                        else {
                            velocityY = 0;
                            velocityX = Mathf.Cos(middle.transform.rotation.eulerAngles.y * Mathf.Deg2Rad) * totalVelocity;
                            velocityZ = Mathf.Sin(middle.transform.rotation.eulerAngles.y * Mathf.Deg2Rad) * totalVelocity;
                        }
                        velocityY = zeroOut(velocityY, centerDistAmpl.y);
                        velocityX = zeroOut(velocityX, centerDistAmpl.x);
                        velocityZ = zeroOut(velocityZ, centerDistAmpl.z);
                        set = true;
                        lens = false;
                    }
                }
                else
                {
                    set = false;
                }
                if (transform.position.y < 0)
                {
                    Destroy(this.gameObject);
                }
                if (Vector3.Distance(mainner.centralBase.transform.position, transform.position) > 10)
                {
                    Destroy(this.gameObject);
                }
                transform.Translate(new Vector3(velocityX * Time.deltaTime, velocityY * Time.deltaTime, velocityZ * Time.deltaTime));
                if (split)
                {
                    if (time <= 0)
                    {
                        split = false;
                        time = 4;
                    }
                    time -= Time.deltaTime;
                }
            }
        }
        else {
            transform.Translate(new Vector3(velocityX * Time.deltaTime, velocityY * Time.deltaTime, velocityZ * Time.deltaTime));
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (testing && other.gameObject.tag == "lens") {
            Vector3 normal = other.contacts[0].normal;
            float angleUp = Mathf.Atan(Mathf.Abs(normal.y) / (Mathf.Sqrt(Mathf.Pow(normal.x, 2) + Mathf.Pow(normal.z, 2))));
            float angleSide = Mathf.Atan(Mathf.Abs(normal.x) / Mathf.Abs(normal.z));
            float lightUp = Mathf.Atan(velocityY / (Mathf.Sqrt(Mathf.Pow(velocityX, 2) + Mathf.Pow(velocityZ, 2))));
            float lightSide = Mathf.Atan(velocityX / velocityZ);
            ItemManager otherManage = other.gameObject.GetComponent<SurfaceRefrance>().refrence;
            float otherAngle = 0f;
            if (otherManage.sections[4].text == "") {
                otherAngle = 1;
            } else {
                float.TryParse(otherManage.sections[4].text, out otherAngle);
            }
            float newAngle;
            float newSide;
            if ((currentIndRef / otherAngle) * Mathf.Sin(angleUp - lightUp) > 1) {
                newAngle = Mathf.PI / 2;
            } else if ((currentIndRef / otherAngle) * Mathf.Sin(angleUp - lightUp) < -1) {
                newAngle = -1 * (Mathf.PI / 2);
            } else {
                newAngle = Mathf.Asin((currentIndRef / otherAngle) * Mathf.Sin(angleUp - lightUp));
            }
            if ((currentIndRef / otherAngle) * Mathf.Sin(angleSide - lightSide) > 1) {
                newSide = Mathf.PI / 2;
            } else if ((currentIndRef / otherAngle) * Mathf.Sin(angleSide - lightSide) < -1) {
                newSide = -1 * (Mathf.PI / 2);
            } else {
                newSide = Mathf.Asin((currentIndRef / otherAngle) * Mathf.Sin(angleSide - lightSide));
            }
            if (velocityX < 0 && velocityZ >= 0) {
                lightSide += (180 * Mathf.Deg2Rad);
            } else if (velocityX >= 0 && velocityZ >= 0) {
                lightSide += (180 * Mathf.Deg2Rad);
            } else if (velocityX >= 0 &&  velocityZ < 0) {
                lightSide += (360 * Mathf.Deg2Rad);
            }
            float changeY = 1;
            if (0 <= lightUp && lightUp < 90 * Mathf.Deg2Rad) {
                newAngle -= angleUp;
                changeY = -1;
            } else {
                changeY = -1;
                newAngle -= angleUp;
            } 
            if (0 <= lightSide && lightSide < 90 * Mathf.Deg2Rad) {
                newSide -= angleSide;
                newSide -= Mathf.PI / 2;
            } else if (90 * Mathf.Deg2Rad <= lightSide && lightSide < 180 * Mathf.Deg2Rad) {
                if (90 * Mathf.Deg2Rad <= lightSide && lightSide <= 135 * Mathf.Deg2Rad) {
                    newSide += angleSide;
                    newSide = (180 * Mathf.Deg2Rad - (newSide - (90 * Mathf.Deg2Rad))); 
                } else {
                    newSide -= angleSide;
                    newSide += Mathf.PI/2;
                }
            } else if (180 * Mathf.Deg2Rad <= lightSide && lightSide < 270 * Mathf.Deg2Rad) {
                newSide -= angleSide;
                newSide += Mathf.PI/2;
            } else if (270 * Mathf.Deg2Rad <= lightSide && lightSide < 360 * Mathf.Deg2Rad) {
                if (270 * Mathf.Deg2Rad <= lightSide && lightSide < 315 * Mathf.Deg2Rad) {
                    newSide += angleSide;
                    newSide = (90 * Mathf.Deg2Rad) - newSide;
                } else {
                    newSide -= angleSide;
                    newSide -= (Mathf.PI / 2);
                }
            }
            float totalVel = Mathf.Sqrt(Mathf.Pow(velocityX, 2) + Mathf.Pow(velocityY, 2) + Mathf.Pow(velocityZ, 2));
            velocityY = Mathf.Sin(newAngle) * totalVel * changeY;
            //velocityY = 0;
            float velocityXZ = Mathf.Cos(newAngle) * totalVel;
            //velocityXZ = 0;
            velocityX = Mathf.Cos(newSide) * velocityXZ;
            velocityZ = Mathf.Sin(newSide) * velocityXZ;        
        }
    }
    private void OnCollisionExit(Collision other) {
        if (testing && other.gameObject.tag == "boundry") {
            testSurface.posX[indexSurface] = transform.position.x * -1;
            testSurface.posY[indexSurface] = transform.position.y;
            testSurface.posZ[indexSurface] = transform.position.z * -1;
            float xMinus = 1;
            float yMinus = 1;
            float zMinus = 1;
            if (velocityX < 0) {
                xMinus = -1;
            }
            if (velocityY < 0) {
                yMinus = -1;
            }
            if (velocityZ < 0) {
                zMinus = -1;
            }
            testSurface.velX[indexSurface] = (Mathf.Pow(velocityX, 2) / 25) * xMinus;
            testSurface.velY[indexSurface] = (Mathf.Pow(velocityY, 2) / 25) * yMinus;
            testSurface.velZ[indexSurface] = (Mathf.Pow(velocityZ, 2) / 25) * zMinus;
            this.gameObject.SetActive(false);
        }    
    }
}