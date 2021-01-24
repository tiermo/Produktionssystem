using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//painterPostionScript controls paint spray position
public class painterPostionScript : MonoBehaviour {

	private Vector3 Position;       //vector to hold postion of paint spray
    private Transform tr;           //postion of paint spray
    private Rigidbody r;
	private Vector3 movement;       //movement vector
    private GameObject pole;        //paint pole

	// Use this for initialization
	void Start () {
		r = GetComponent<Rigidbody> ();
        pole = GameObject.Find("pole");
    }

	// Update is called once per frame
	void Update () {
		movement = new Vector3 (0.0f, 0.0f, 0.0f);                          //reset movement vector
		movement.y = (pole.GetComponent<Transform> ().position.y) - 1.2f;   //keep distance of 1.2f from pole along y axis
		movement.x = r.position.x;                                          //keep same value along x and z axes
		movement.z = r.position.z;
		r.position = movement;
	}
}
