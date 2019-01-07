using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour {
	public Transform target;
	float angle;
	public float rate;
	public float radius = 30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		angle += rate;
		transform.position = new Vector3(Mathf.Sin(angle)*radius, 0, Mathf.Cos(angle)*radius);
		transform.rotation = Quaternion.LookRotation(target.position - transform.position);

	}
}
