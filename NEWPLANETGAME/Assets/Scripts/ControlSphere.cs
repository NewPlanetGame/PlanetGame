using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSphere : MonoBehaviour {
Rigidbody rb;
public float power = 100f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(-transform.position);
		rb.AddForce(transform.rotation * (Vector3.left * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical")) * power);
	}
}
