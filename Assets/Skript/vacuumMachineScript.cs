using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//vacuumMachineScript contains functions to control the vacuum machine.
public class vacuumMachineScript : MonoBehaviour {
	
	private bool machineOn = false; // machine status
	Material red;                   // red color
	Material black;                 // black color
	AudioSource audio;

	void Awake(){
		red = Resources.Load("red", typeof(Material)) as Material;
		black = Resources.Load("crate2", typeof(Material)) as Material;
	}

	void Start(){
		audio = GetComponent<AudioSource> ();
	}

	public void turnMachineOn() {   // turn on of vacuum machine is indicated by change in color to red and playing audio
        machineOn = true;
		audio.Play();
		GetComponent<Renderer> ().material = red;
	}

	public void turnMachineOff() {  // turn off of vacuum machine is indicated by change in color to black and stoping audio
		machineOn = false;
		audio.Stop();
		GetComponent<Renderer> ().material = black;
	}

	public bool getMachineStatus(){
		return machineOn;
	}
		
}
