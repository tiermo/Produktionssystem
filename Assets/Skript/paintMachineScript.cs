using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//paintMachineScript controls painting machine
public class paintMachineScript : MonoBehaviour {

    private bool machineOn = false;                                     // state of the painting machine   
    AudioSource audio;                                                  // conveyor audio
    private Rigidbody pole;                                             // access to paint pole
	private Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);          // movement vector
    private Vector3 zeroMovement = new Vector3(0.0f, 0.0f, 0.0f);       // zero movement vector
    private Vector3 downMovement = new Vector3(0.0f, -0.025f, 0.0f);    // down movement vector
    private Vector3 upMovement = new Vector3(0.0f, 0.025f, 0.0f);       // up movement vector
    private bool upperLimitReached = false;                             // upper limit switch flag
    private bool lowerLimitReached = false;                             // lower limit switch flag
    private Vector3 initialPosition;

	void Start(){                                                       //called only at the beginning
		audio = GetComponent<AudioSource> ();
		initialPosition = transform.GetChild(2).GetComponent<Transform>().position;
		pole = transform.GetChild(2).GetComponent<Rigidbody> ();
	}

	void Update () {                                                     // Update is called once per frame

        float armPolePosition = initialPosition.y - pole.position.y;

        if (armPolePosition > 3.0f) {                                    // check if lower limit reached
			lowerLimitReached = true;
			movement = zeroMovement;                                     // stop movement
			machineOn = false;
		}

		if (armPolePosition < -0.2f) {                                   // check if upper limit reached
			upperLimitReached = true;
			movement = zeroMovement;                                     // stop movement
            machineOn = false;
		}

		pole.position += movement;                                       // move pole is movement != 0
	}

	public void paintOn () {                                             // turn painting on by enabling particle system
		GameObject.Find ("ParticleSystem1").GetComponent<ParticleSystem> ().Play ();
		GameObject.Find ("ParticleSystem2").GetComponent<ParticleSystem>().Play ();
		machineOn = true;
		audio.Play();
	}

	public void paintOff () {                                             // turn painting off by disabling particle system
		GameObject.Find ("ParticleSystem1").GetComponent<ParticleSystem> ().Stop ();
		GameObject.Find ("ParticleSystem2").GetComponent<ParticleSystem>().Stop ();
		machineOn = false;
		audio.Stop();
	}

	public void moveUp() {                                               // function to move pole up
		if (!upperLimitReached) {
			movement = upMovement;                                       // set to up movement vector
			machineOn = true;
			lowerLimitReached = false;
			pole.position += movement;                                   // move up
		}
	}

	public void moveDown() {                                             // function to move pole down
		if (!lowerLimitReached) {
			movement = downMovement;
			machineOn = true;
			upperLimitReached = false;
			pole.position += movement;
		}
	}

	public bool getMachineStatus(){
		return machineOn;
	}

}
