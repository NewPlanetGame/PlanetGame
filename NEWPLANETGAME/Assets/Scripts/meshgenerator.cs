using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshgenerator : MonoBehaviour {

	Mesh mesh;
	int xsize = 50;
	int zsize = 50;
	int l;

	Vector3[] vertices;
	int[] triangles;

	// Use this for initialization
	void Start () {
		l = (zsize + 1) * xsize - 1;
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		CreateShape();
		UpdateMesh();
	
	}
/* 
	void Update(){
		l = Mathf.RoundToInt(Time.time);
		Debug.Log (l);
		CreateShape();
		UpdateMesh();
	}
*/
	
	// Update is called once per frame
	void CreateShape () {
		vertices = new Vector3[((xsize + 1) * (zsize + 1))];
		for (int i = 0, z = 0; z <= zsize; z++){
			for (int x = 0; x <= xsize; x++){
				vertices[i] = new Vector3(x, (x-10)*(x-40)*(z-10)*(z-40)/10000f, z);
				//Debug.Log(vertices[i]);
				i++;
			}
		}
		triangles = new int[6 * l];
		for (int i = 0; i < l; i++){
			if (i % (xsize + 1) != xsize){
			triangles[6 * i] = i;
			triangles[6 * i + 1] = zsize + i + 1;
			triangles[6 * i + 2] = i + 1;
			triangles[6 * i + 3] = i + 1;
			triangles[6 * i + 4] = zsize + i + 1;
			triangles[6 * i + 5] = zsize + 2 + i;
			}
		}
	
	}
	void UpdateMesh (){
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}
