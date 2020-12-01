using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointParts : MonoBehaviour
{
    public bool inOperation = true;
    public Vector3 position;
    public float[] positionArray;
    public int index;
    public Vector3 combine = Vector3.zero;
    public Vector3 backCombine = Vector3.zero;
    public Vector3 forwardCombine = Vector3.zero;
    public List<int> tempStuff = new List<int>();
    public List<Vector3> verts = new List<Vector3>();
    public float tempDist;
    public float extremeOffset = 3;
    public GameObject obj;
    public Mesh mesh = new Mesh();
    public List<int> triangle = new List<int>();
    public List<PointParts> partPoint = new List<PointParts>();
    public List<int> previous = new List<int>();
    public List<int> current = new List<int>();
    public List<int> future = new List<int>();
    public List<int> queue = new List<int>();
    public int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        tempStuff = new List<int>();
        tempDist = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
