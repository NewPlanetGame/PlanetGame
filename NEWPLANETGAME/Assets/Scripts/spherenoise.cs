using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class spherenoise : MonoBehaviour {

	Mesh mesh;

	float radius = 10; 
	int yintervals = 254;
	float offsetx;
	float offsety;
	float offsetz;
	int xintervals = 0;
	int growthrate = 4;
	float v1 = 1000;
	float v2 = 2;
	float v3 = 0.6f;
	float v4 = 3;
	float multiplier = 1;
	int recursion = 10;

	Vector3[] vertices;
	int[] triangles;
	float t = 0;
	public float zoom;
	public float rate;


	// Use this for initialization
	void Start () {
		offsetx = Random.Range(-10000, 10000);
		offsety = Random.Range(-10000, 10000);
		offsetz = Random.Range(-10000, 10000);
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		CreateVertices();
		CreateTriangles();
		UpdateMesh();
	
	}
	void Update(){
		if (Input.GetKeyDown("a")){
			CreateVertices();
			UpdateMesh();
		}
		if (Input.GetKeyDown("s")){
			offsetx = Random.Range(-10000, 10000);
			offsety = Random.Range(-10000, 10000);
			offsetz = Random.Range(-10000, 10000);
		}
	}

	float noisy(float x, float y, float z, float height, float zoom){
		return (height + Noise.Perlin3D(new Vector3(x, y, z)/zoom + new Vector3(offsetx, offsety, offsetz), 1))/height;
	}
	void CreateVertices () {
	

		vertices = new Vector3[2 + (yintervals * yintervals * growthrate / 4)];
		int i = 0;
		int xintervals = 0;
		for (float roty = 0, y = 1; Mathf.Round(roty / Mathf.PI * 1000) <= 1000; roty += Mathf.PI / yintervals, y = Mathf.Cos(roty)){
			for (float rotx = 0, x, z; Mathf.Round(rotx / Mathf.PI * 1000) < 2000; rotx += 2 * Mathf.PI / Mathf.Max(xintervals, 1)){
				x = Mathf.Cos(rotx) * Mathf.Sqrt(1 - y * y);
				z = Mathf.Sin(rotx) * Mathf.Sqrt(1 - y * y);
				for (int s = 0; s < recursion; s++){
					multiplier *= noisy(x, y, z, v1 * Mathf.Pow(v2, s), v3 / Mathf.Pow(v4, s));
				}
				vertices[i] = new Vector3(x, y, z) * radius * multiplier;
				i++;
				multiplier = 1;

			}
			if (Mathf.RoundToInt(y * 100) != 0){
				xintervals += Mathf.RoundToInt(Mathf.Sign(y)) * growthrate;
			}else{
				xintervals -= growthrate;
			}
		}
	}
	void CreateTriangles(){
		triangles = new int[6 * yintervals * yintervals * growthrate / 4];
		int n = 0;
		int level = 1;
		int prevlev = 0;
		int startpos = 1;
		int prevstartpos = 0;
		for (int s = 0; s < yintervals / 2; s++){
			
			for (int i = 0; i < growthrate * level; i++){
				if (prevlev != 0) {
					triangles[n + 1] = ((i + 1) * prevlev / level) % (growthrate * prevlev) + prevstartpos;
				}else{
					triangles[n + 1] = 0;
				}
				triangles[n] = i + startpos;
				triangles[n + 2] = (i + 1)%(growthrate * level) + startpos;
				n += 3;
			}
			
			for (int i = 0; i < growthrate * prevlev; i++){
				triangles[n] = prevstartpos + growthrate * prevlev + ((level * i / prevlev + 1) % (level * growthrate));
				triangles[n + 1] = i + prevstartpos;
				triangles[n + 2] = (i + 1)%(growthrate * prevlev) + prevstartpos;
				n += 3;
			}
			
			prevlev = level;
			prevstartpos = startpos;
			startpos += growthrate * level;
			level ++;

		}
		level = 1;
		prevlev = 0;
		startpos = 1;
		prevstartpos = 0;
		for (int s = 0; s < yintervals / 2; s++){
			
			for (int i = 0; i < growthrate * level; i++){
				if (prevlev != 0) {
					triangles[n + 1] = vertices.Length - 1 - (((i + 1) * prevlev / level) % (growthrate * prevlev) + prevstartpos);
				}else{
					triangles[n + 1] = vertices.Length - 1 - (0);
				}
				triangles[n] = vertices.Length - 1 - (i + startpos);
				triangles[n + 2] = vertices.Length - 1 - ((i + 1)%(growthrate * level) + startpos);
				n += 3;
			}
			
			for (int i = 0; i < growthrate * prevlev; i++){
				triangles[n] = vertices.Length - 1- (prevstartpos + growthrate * prevlev + ((level * i / prevlev + 1) % (level * growthrate)));
				triangles[n + 1] = vertices.Length - 1 - (i + prevstartpos);
				triangles[n + 2] = vertices.Length - 1 - ((i + 1)%(growthrate * prevlev) + prevstartpos);
				n += 3;
			}
			
			prevlev = level;
			prevstartpos = startpos;
			startpos += growthrate * level;
			level ++;

		}

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
