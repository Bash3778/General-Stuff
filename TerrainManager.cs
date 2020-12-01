using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {
	[SerializeField] GameObject largeTerrain;
	[SerializeField] Terrain[] terrains;
	[SerializeField] Transform[] tObject;
	[SerializeField] Terrain center;
	[SerializeField] Terrain left;
	[SerializeField] Terrain top;
	[SerializeField] Terrain right;
	[SerializeField] Terrain bottom;
	//Array with trees we are going to combine
	//The object that is going to hold the combined mesh
	public GameObject combinedObj;
	// Use this for initialization
	void Start () {
		Terrain[] children = largeTerrain.GetComponentsInChildren<Terrain> ();
		Transform[] child = largeTerrain.GetComponentsInChildren<Transform> ();
		tObject = child;
		terrains = children;
		for (int i = 0; i < terrains.Length; i++) {
			Terrain[] sides = new Terrain[4];
			int nullifier = 0;
			for (int j = 0; j < terrains.Length; j++) {
				if (terrains [i].transform.position.z + 500 == terrains [j].transform.position.z) {
					sides [1] = terrains [j];
				}
				if (terrains [i].transform.position.x - 500 == terrains [j].transform.position.x) {
					sides [0] = terrains [j];
				}
				if (terrains [i].transform.position.x + 500 == terrains [j].transform.position.x) {
					sides [2] = terrains [j];
				}
				if (terrains [i].transform.position.z - 500 == terrains [j].transform.position.z) {
					sides [3] = terrains [j];
				}
			}
			for (int j = 0; j < sides.Length; j++) {
				if (sides [j] == null) {
					nullifier += 1;
				}
			}
			if (nullifier == 0) {
				terrains [i].SetNeighbors (sides [0], sides [1], sides [2], sides [3]);
				terrains [i].Flush ();
			}
		}
		//CombineTrees ();
	}
		//Similar to Unity's reference, but with different materials
		//http://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
		void CombineTrees() 
		{
			//Lists that holds mesh data that belongs to each submesh
			List<CombineInstance> leafList = new List<CombineInstance>();

			//Loop through the array with trees
		for (int i = 0; i < tObject.Length; i++)
			{
			GameObject currentTree = tObject[i].gameObject;

				//Deactivate the tree 
				currentTree.SetActive(false);

				//Get all meshfilters from this tree, true to also find deactivated children
				MeshFilter[] meshFilters = currentTree.GetComponentsInChildren<MeshFilter>(true);

				//Loop through all children
				for (int j = 0; j < meshFilters.Length; j++)
				{
					MeshFilter meshFilter = meshFilters[j];

					CombineInstance combine = new CombineInstance();

					//Is it wood or leaf?
					MeshRenderer meshRender = meshFilter.GetComponent<MeshRenderer>();

					//Modify the material name, because Unity adds (Instance) to the end of the name
					string materialName = meshRender.material.name.Replace(" (Instance)", "");

					if (materialName == "Leaf")
					{
						combine.mesh = meshFilter.mesh;
						combine.transform = meshFilter.transform.localToWorldMatrix;

						//Add it to the list of leaf mesh data
						leafList.Add(combine);
					}
				}
			}


			//First we need to combine the wood into one mesh and then the leaf into one mesh
			Mesh combinedLeafMesh = new Mesh();
			combinedLeafMesh.CombineMeshes(leafList.ToArray());

			//Create the array that will form the combined mesh
			CombineInstance[] totalMesh = new CombineInstance[1];

			//Add the submeshes in the same order as the material is set in the combined mesh
			totalMesh[0].mesh = combinedLeafMesh;
			totalMesh[0].transform = combinedObj.transform.localToWorldMatrix;
			//Create the final combined mesh
			Mesh combinedAllMesh = new Mesh();

			//Make sure it's set to false to get 2 separate meshes
			combinedAllMesh.CombineMeshes(totalMesh, false);
			combinedObj.GetComponent<MeshFilter>().mesh = combinedAllMesh;
		}

	// Update is called once per frame
	void Update () {
		
	}
}