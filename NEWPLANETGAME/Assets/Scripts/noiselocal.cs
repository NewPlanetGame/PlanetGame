using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class noiselocal : MonoBehaviour {

	Mesh mesh;

	float radius = 0.01f;
	public Gradient gradient;
	Quaternion wholerotation = Quaternion.Euler(0, 0, 0); 
	int yintervals = 450;
	float offsetx;
	float offsety;
	public Transform target;

	float offsetz;
	int growthrate = 4;
	float v1 = 36;
	float v2 = 2;
	float v3 = 0.6f;
	float v4 = 3;
	float multiplier = 1;
	/*
	public float minmul = 100;
	public float maxmul = 0;
	*/
	int levels;

	public int recursion = 4;

	Vector3[] vertices;
	Color[] colors;
	int[] triangles;


	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		Random.InitState(1);
		levels = 100;
		offsetx = Random.Range(-10000, 10000);
		offsety = Random.Range(-10000, 10000);
		offsetz = Random.Range(-10000, 10000);
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		CreateVertices();
		CreateTriangles();
		UpdateMesh();
		//GetComponent<MeshCollider>().sharedMesh = mesh;

	
	}
	void Update(){
		wholerotation = Quaternion.LookRotation((target.position - transform.position), Vector3.up);
		CreateVertices();
		UpdateMesh();
		if (Input.GetKeyDown("s")){
			offsetx = Random.Range(-10000, 10000);
			offsety = Random.Range(-10000, 10000);
			offsetz = Random.Range(-10000, 10000);
		}
	}

	float noisy(float x, float y, float z, float height, float zoom){
		return (height + Noise.Perlin3D(wholerotation * new Vector3(z, x, y)/zoom + new Vector3(offsetx, offsety, offsetz), 1))/height;
	}
	void CreateVertices () {
	
		vertices = new Vector3[1 + ((levels - 1) * levels * growthrate) / 2];
		colors = new Color[vertices.Length];
		int i = 0;
		int xintervals = 0;
		float roty = 0;
		float y = 1;
		float rotx = 0;
		float x;
		float z;
		for (int b = 0; b < levels; roty += Mathf.PI / yintervals, y = Mathf.Cos(roty), b++){
			rotx = 0;
			for (int c = 0; c < Mathf.Max(growthrate * b, 1); c++, rotx = 2 * Mathf.PI / Mathf.Max(xintervals, 1) * c){
				x = Mathf.Cos(rotx) * Mathf.Sqrt(1 - y * y);
				z = Mathf.Sin(rotx) * Mathf.Sqrt(1 - y * y);
				for (int s = 0; s < recursion; s++){
					multiplier *= noisy(x, y, z, v1 * Mathf.Pow(v2, s), v3 / Mathf.Pow(v4, s));
				}
				vertices[i] = wholerotation * new Vector3(z, x, y) * radius * multiplier;
				colors[i] = gradient.Evaluate(Mathf.InverseLerp(0.972f, 1.027f, multiplier));
				/*
				if (multiplier < minmul){
					minmul = multiplier;
				}
				if (multiplier > maxmul){
					maxmul = multiplier;
				}
				*/
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
		triangles = new int[3 * levels * levels * growthrate];
		int n = 0;
		int level = 1;
		int prevlev = 0;
		int startpos = 1;
		int prevstartpos = 0;
		for (int s = 0; s < levels - 1; s++){
			
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


	}
	void UpdateMesh (){
		
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.colors = colors;
		mesh.RecalculateNormals();
	
	}
	/* *
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
