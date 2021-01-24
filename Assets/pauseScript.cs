using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//pauseScript contains functions to pause and exit simulation.
public class pauseScript : MonoBehaviour {
	public bool paused = false;

	public void Pause () {      //pause simulation
		paused = !paused;
		if (paused) {
			Time.timeScale = 0;
		}
		else {
			Time.timeScale = 1;
		}
	}

	public void quitApp(){      //exit simulation
		Application.Quit();
	}
}
