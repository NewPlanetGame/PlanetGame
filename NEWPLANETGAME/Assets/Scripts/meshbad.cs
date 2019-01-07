using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshbad : MonoBehaviour {

	Mesh mesh;
	public int thing1;
	public int thing2;
	public int thing3;
	int radius = 4; 

	Vector3[] vertices;
	int a = 0;
	int[] triangles;

	// Use this for initialization
	void Start () {
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
	
		int yintervals = 16;
		int xintervals = yintervals * 2;
		vertices = new Vector3[yintervals * yintervals * 2];
		int i = 0;
		for (float roty = 0, y = radius; Mathf.Round(roty / Mathf.PI * 1000) <= 1000; roty += Mathf.PI / yintervals, y = Mathf.Cos(roty) * radius){
			for (float rotx = 0, x, z; Mathf.Round(rotx / Mathf.PI * 1000) < 2000; rotx += 2 * Mathf.PI / xintervals){
				if ((radius * radius - y * y) * 100 != 0){
					x = Mathf.Cos(rotx) * Mathf.Sqrt(radius * radius - y * y);
					z = Mathf.Sin(rotx) * Mathf.Sqrt(radius * radius - y * y);
					vertices[i] = new Vector3(x, y, z);
					i++;
				}
			}

		}
		/* 
		triangles = new int[3 * 100];
		for (i = 0; i < growthrate + 1; i++){
			triangles[3 * i + 1] = 0;
			triangles[3 * i] = i + 1;
			triangles[3 * i+2] = (i + 1)%(growthrate + 1) + 1;
			Debug.Log(i);
		}
		for (i = 0; i < 5; i++){
			triangles[3 * growthrate + 6 + 3 * i] = i + 1;
			triangles[3 * growthrate + 6 + 3 * i + 1] = (i + 1)%(growthrate + 1) + 1;
			triangles[3 * growthrate + 6 + 3 * i + 2] = 7;
		}
		/*@ *
		triangles[3 * growthrate + 6] = 1;
		triangles[3 * growthrate + 7] = 2;
		triangles[3 * growthrate + 8] = 7;
		triangles[3 * growthrate + 9] = 2;
		triangles[3 * growthrate + 10] = 3;
		triangles[3 * growthrate + 11] = 9;
		triangles[3 * growthrate + 12] = 3;
		triangles[3 * growthrate + 13] = 4;
		triangles[3 * growthrate + 14] = 10;
		triangles[3 * growthrate + 15] = 4;
		triangles[3 * growthrate + 16] = 5;
		triangles[3 * growthrate + 17] = 12;
		triangles[3 * growthrate + 18] = 5;
		triangles[3 * growthrate + 19] = 1;
		triangles[3 * growthrate + 20] = 14;

		triangles[3 * growthrate + 21] = 7;
		triangles[3 * growthrate + 22] = 2;
		triangles[3 * growthrate + 23] = 8;
		triangles[3 * growthrate + 24] = 8;
		triangles[3 * growthrate + 25] = 2;
		triangles[3 * growthrate + 26] = 9;
	
		
		//SECOND ROW
		/* 
		for (b = gapsum - gap + times; b < gapsum + times; b++){
			triangles[l] = b;
			l++;
			triangles[l] = b + gap + 1;
			l++;
			triangles[l] = b + 1;
			l++;
		}
		for ( b = gapsum + times + 1; b < gapsum + gap + times; b++){
			triangles[l] = b;
			l++;
			triangles[l] = b + 1;
			l++;
			triangles[l] = b - gap;
			l++;		
		}
		*/

	}
	void UpdateMesh (){
		
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	
	}
 
	void OnDrawGizmos(){
		if (vertices == null){
			return;
		}
		for (int i = 0; i < vertices.Length; i++){
			Gizmos.DrawSphere(vertices[i], .1f);
		}
	}
	
	
}
