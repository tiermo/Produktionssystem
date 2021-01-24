using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//drillingArmScript contains functions to control the drilling head of the drilling machine.
public class drillingArmScript : MonoBehaviour {

	private Vector3 initialPosition;                                    //initial position of the drilling head
    private Transform tr;                                               //x,y,z location of the drilling head
    private Rigidbody arm;                                              //rigidbody attached to the drilling head/arm
    private Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);          //movement vector of the drilling head initialised to zero
    private bool machineOn = false;                                     //state of the drilling machine   
    private Vector3 zeroMovement = new Vector3(0.0f, 0.0f, 0.0f);       //zero movement vector
    private Vector3 downMovement = new Vector3(0.0f, -0.025f, 0.0f);    //down movement vector
    private Vector3 upMovement = new Vector3(0.0f, 0.025f, 0.0f);       //up movement vector
    private bool upperLimitReached = false;                             //upper limit reached indicator
	private bool lowerLimitFlag = false;                                //flag to control sending back acknowledgement on reaching lower limit
	private bool upperLimitFlag = false;                                //flag to control sending back acknowledgement on reaching upper limit
    private bool lowerLimitReached = false;                             //lower limit reached indicator
    AudioSource audio;                                                  //drilling audio

	void Start () {                                                     //called only at the beginning
		tr = GetComponent<Transform>();	
		initialPosition = tr.position;
		arm = GetComponent<Rigidbody> ();
        arm.freezeRotation = true;                                      //avoid rotation of the drilling head
        audio = GetComponent<AudioSource> ();
	}

	void Update () {                                                    //called once in every frame

        float armVerticalPosition = initialPosition.y - arm.position.y;

        if (armVerticalPosition > 3.5f) {                               //check if lower limit is reached
			lowerLimitReached = true;
			if (lowerLimitFlag) {
				GetComponent<tcpDrilling> ().LimitSwitchesReached ("limitD");   //send acknowledgement from tcpDrilling class
                lowerLimitFlag = false;
			}
            Debug.Log("lowerLimitReached");
		}

        if(armVerticalPosition > 3.7f)
        {
            Debug.Log("Extreme lowerLimitReached. Machine stopped movement");
            stopVerticalMovement();                                      //stop movement on reaching extreme limit
        }

		if (armVerticalPosition < -1.75f) {                              //check if upper limit is reached
			upperLimitReached = true;
			if (upperLimitFlag) {
				GetComponent<tcpDrilling> ().LimitSwitchesReached ("limitU");
				upperLimitFlag = false;
			}
            Debug.Log("upperLimitReached");
        }

        if (armVerticalPosition < -1.95f)
        {
            Debug.Log("Extreme upperLimitReached. Machine stopped movement");
            stopVerticalMovement();                                     //stop movement on reaching extreme limit
        }


        arm.position += movement;                                       //change arm position if there is a request
	}

	public void callLimitSensorUp(){                                    //activate upper limit switch
		upperLimitFlag = true;
	}

	public void callLimitSensorDown(){                                  //activate lower limit switch
		lowerLimitFlag = true;
	}

	public void turnOn(){                                               //function to turn drilling head
		audio.Play(); 
    }

	public void turnOff(){
		audio.Stop();
	}

	public bool getMachineStatus(){
		return machineOn;
	}

	public bool getUpperLimitStatus(){
		return upperLimitReached;
	}

	public bool getLowerLimitStatus(){
		return lowerLimitReached;
	}

	public void moveUp() {                                              //function to move drilling head up
		movement = upMovement;
		machineOn = true;
		lowerLimitReached = false;
		arm.position += movement;
	}

	public void moveDown() {                                            //function to move drilling head down
		movement = downMovement;
		machineOn = true;
		upperLimitReached = false;
		arm.position += movement;
	}

	public void stopMovement() {                                            //function to stop drilling head
		movement = zeroMovement;
		arm.velocity = zeroMovement;
		machineOn = false;
	}

    public void stopVerticalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 0.0f, 1.0f)); //multiply by vector (1,0,1) to stop vertical movement
        arm.velocity = zeroMovement;
    }

}
