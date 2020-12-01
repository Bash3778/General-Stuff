         //Contours
            List<Vector3> radialPositions = new List<Vector3>();
            for (int i = 0; i < charges.Count; i++)
            {
                for (int r = 1; r <= maxRDist; r++)
                {
                    for (float theta = 0; theta < 180; theta += angleInterval[r - 1])
                    {
                        for (float phi = 0; phi < 360; phi += angleInterval[r - 1]) {
                            float ypos = charges[i].gameObject.transform.position.y + Mathf.Sin(phi * Mathf.Deg2Rad) * r * radialInterval;
                            float xzdist = Mathf.Cos(phi * Mathf.Deg2Rad) * r * radialInterval;
                            float xpos = charges[i].gameObject.transform.position.x + Mathf.Sin(theta * Mathf.Deg2Rad) * xzdist;
                            float zpos = charges[i].gameObject.transform.position.z + Mathf.Cos(theta * Mathf.Deg2Rad) * xzdist;
                            if (xpos <= 9 && xpos >= -9 && zpos <= 9 && zpos >= -9 && ypos <= 9 && ypos >= -9)
                            {
                                radialPositions.Add(new Vector3(xpos, ypos, zpos));
                            }
                        }
                    }
                }
            }
            for (int j = 0; j < radialPositions.Count; j++)
            {
                float strength = 0;
                for (int i = 0; i < charges.Count; i++)
                {
                    strength += Mathf.Abs(charges[i].charge) / Vector3.Distance(charges[i].gameObject.transform.position, radialPositions[j]);
                }
                GameObject obj = Instantiate(sampleContourPoint);
                obj.transform.position = radialPositions[j];
                obj.transform.localScale = new Vector3(strength / contourScalar, strength / contourScalar, strength / contourScalar);
                contourPoints.Add(obj);
                Debug.Log("making");
                obj.SetActive(true);
            }