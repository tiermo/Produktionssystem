using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//particleSystemScript contains functions to control paint spray on/off.
public class particleSystemScript : MonoBehaviour {
	Material pink;      // pink color

	void Awake(){
		gameObject.GetComponent<ParticleSystem>().Stop ();  // stop on initialisation
		pink = Resources.Load("pink", typeof(Material)) as Material;
	}

	void OnParticleCollision(GameObject other) {
		if (string.Compare (other.name, "TestCube") == 0) {
			StartCoroutine (Delay());                       // change color of testCube to pink
		}
	}

	IEnumerator Delay()
	{
		GameObject.Find ("TestCube").GetComponent<Renderer> ().material = pink;
		yield return new WaitForSeconds(1);
	}
}
