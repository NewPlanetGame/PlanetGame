using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MESH : MonoBehaviour {

	Mesh mesh;
	int radius = 2; //square of radius

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
	
		int intervals = 8;
		vertices = new Vector3[(intervals + 1) * (intervals + 2) * 4]; // * 4 for all, /2 for 1/8
		int i = 0;
		int yintervals = intervals + 1;
		float xitr = 1;
		float yitr;
		for (float x = 0; xitr <= intervals + 1.5f; x = Mathf.Sin(xitr * 0.5f / intervals * Mathf.PI) * radius, xitr++){
			yitr = 1;
			yintervals--;
			for (float y = 0; yitr <= yintervals + 1.5f; y = Mathf.Sin(yitr * 0.5f / yintervals * Mathf.PI) * Mathf.Sqrt(Mathf.Max(radius * radius - x * x, 0)), yitr ++){
				float z = Mathf.Sqrt(Mathf.Max(radius * radius - x * x - y * y, 0));
				if (true){
					vertices[i] = new Vector3(x, y, z);
					vertices[i + (intervals + 2) * (intervals + 1) / 2] = new Vector3(-x, z, y);
					vertices[i + (intervals + 2) * (intervals + 1)] = new Vector3(-x, -z, -y);
					vertices[i + (intervals + 2) * (intervals + 1) / 2 * 3] = new Vector3(x, -z, y);
					vertices[i + (intervals + 2) * (intervals + 1) * 2] = new Vector3(x, z, -y);
					vertices[i + (intervals + 2) * (intervals + 1) / 2 * 5] = new Vector3(-x, -y, z);
					vertices[i + (intervals + 2) * (intervals + 1) * 3] = new Vector3(-x, y, -z);
					vertices[i + (intervals + 2) * (intervals + 1) / 2 * 7] = new Vector3(x, -y, -z);
					i++;

					
				}
			}
		}
		triangles = new int[intervals * intervals * 3 * 24];
		int b;
		int l;
		//FIRST ROW
		for (int d = 0; d < 8; d++){
			l = d * intervals * intervals * 3 * 3;
			Debug.Log(l);
			int gap = intervals;
			int gapsum = gap;
			for (int c = 0; c < intervals; c++){
				for (b = gapsum - gap + c + d * (intervals + 1) * (intervals + 2) / 2; b < gapsum + c + d * (intervals + 1) * (intervals + 2) / 2; b++){
					triangles[l] = b;
					l++;
					triangles[l] = b + gap + 1;
					l++;
					triangles[l] = b + 1;
					l++;
				}
				for (b = gapsum + c + 1 + d * (intervals + 1) * (intervals + 2) / 2; b < gap + gapsum + c + d * (intervals + 1) * (intervals + 2) / 2; b++){
					triangles[l] = b;
					l++;
					triangles[l] = b + 1;
					l++;
					triangles[l] = b - gap;
					l++;
				}
				gap--;
				gapsum += gap;
			}
			Debug.Log(l);		
		}
		
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
/* 
	void OnDrawGizmos(){
		if (vertices == null){
			return;
		}
		for (int i = 0; i < vertices.Length; i++){
			Gizmos.DrawSphere(vertices[i], .1f);
		}
	}
	*/
	
}
