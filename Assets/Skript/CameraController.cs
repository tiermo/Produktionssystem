using UnityEngine;
using System.Collections;

//Author: Sagar Nayak
//Date: 26.10.2017

//CameraController controls camera movement in game engine
public class CameraController : MonoBehaviour {

	public GameObject testCube;                                      //testCube gameObject
	private Vector3 offset;                                          //store offset distance

	void Start ()
	{
		offset = transform.position - testCube.transform.position;   //find distance between testCube and camera
	}

	void LateUpdate ()
	{
		transform.position = testCube.transform.position + offset;   //update camera postion with testCube postion
    }
}