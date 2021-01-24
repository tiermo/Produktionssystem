using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class millingArmScript : MonoBehaviour {

    private Vector3 initialPosition;                                    //initial position of the milling head
    private Transform tr;                                               //x,y,z location of the milling head
    private Rigidbody arm;                                              //rigidbody attached to the milling head/arm
    private Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);          //movement vector of the milling head initialised to zero
    private bool machineOn = false;                                     //state of the milling machine   
    private Vector3 zeroMovement = new Vector3(0.0f, 0.0f, 0.0f);       //zero movement vector
    private Vector3 downMovement = new Vector3(0.0f, -0.025f, 0.0f);    //down movement vector
    private Vector3 upMovement = new Vector3(0.0f, 0.025f, 0.0f);       //up movement vector
    private bool upperLimitReached = false;                             //upper limit reached indicator
    private bool lowerLimitFlag = false;                                //flag to control sending back acknowledgement on reaching lower limit
    private bool upperLimitFlag = false;                                //flag to control sending back acknowledgement on reaching upper limit
    private bool lowerLimitReached = false;                             //lower limit reached indicator
    AudioSource audio;                                                  //milling audio
    private Vector3 leftMovement = new Vector3(0f, 0.0f, +0.025f);
    private Vector3 rightMovement = new Vector3(0f, 0.0f, -0.025f);
	private bool middlePosition = false;
	private bool rightPosition = false;
	private bool leftPosition = false;

	void Start () {                                                     //called only at the beginning
		tr = GetComponent<Transform>();	
		initialPosition = tr.position;
		arm = GetComponent<Rigidbody> ();
        arm.freezeRotation = true;                                      //avoid rotation of the milling head
        audio = GetComponent<AudioSource> ();
	}

	void Update () {                                                    //called once in every frame

        float armVerticalPosition = initialPosition.y - arm.position.y;
        float armHorizontalPosition = initialPosition.z - arm.position.z;

        if (armVerticalPosition > 4.2f) {                               //check if lower limit is reached
			lowerLimitReached = true;
			if (lowerLimitFlag) {
				GetComponent<tcpMilling> ().LimitSwitchesReached ("limitD");   //send acknowledgement from tcpMilling class
                lowerLimitFlag = false;
			}
            Debug.Log("lowerLimitReached");
        }

        if (armVerticalPosition > 4.4f)
        {
            Debug.Log("Extreme lowerLimitReached. Machine stopped movement");
            stopVerticalMovement();                                            //stop movement on reaching extreme limit
        }

        if (armVerticalPosition < -0.75f) {                                    //check if upper limit is reached
			upperLimitReached = true;
			if (upperLimitFlag) {
				GetComponent<tcpMilling> ().LimitSwitchesReached ("limitU");
				upperLimitFlag = false;
			}
            Debug.Log("upperLimitReached");
        }

        if (armVerticalPosition < -0.95f)
        {
            Debug.Log("Extreme upperLimitReached. Machine stopped movement");
            stopVerticalMovement();                                             //stop movement on reaching extreme limit
        }

        if ((armHorizontalPosition > 2.5f) || (armHorizontalPosition < -2.5))   // limit reached along horizontal axis
        {
            stopHorizontalMovement();
        }

        if ((armHorizontalPosition > 2.0f) && rightPosition) {                  //move to right position if rightPosition is true
            stopHorizontalMovement();
            rightPosition = false;
		}

        if ((armHorizontalPosition < -2.0) && leftPosition) {                   //move to left position if leftPosition is true
            stopHorizontalMovement();
            leftPosition = false;
		}

		if ((armHorizontalPosition == 0f) && middlePosition) {                   //move to middle position if middlePosition is true
            stopHorizontalMovement();
            middlePosition = false;
		}

		arm.position += movement;
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

	public void moveLeft() {                                            //function to move milling head to left postion
		movement = leftMovement;
		arm.position += movement;
		machineOn = true;
		leftPosition = true;
	}

	public void moveRight() {                                           //function to move milling head to right postion                  
		movement = rightMovement;
		arm.position += movement;
		machineOn = true;
		rightPosition = true;
	}

	public void moveMiddle() {                                          //function to move milling head to middle postion
		if (initialPosition.z < arm.position.z) {                       //check if milling head is in the left postion, if yes then move to the right, else move to the left
			movement = rightMovement;
			arm.position += movement;
			machineOn = true;
		} else {
			movement = leftMovement;
			arm.position += movement;
			machineOn = true;
		}
		middlePosition = true;
	}

	public void moveUp() {                                              //function to move milling head up
		movement = upMovement;
		machineOn = true;
		lowerLimitReached = false;
		arm.position += movement;
	}

	public void moveDown() {                                             //function to move milling head down
		movement = downMovement;
		machineOn = true;
		upperLimitReached = false;
		arm.position += movement;
	}

	public void stopMovement() {                                         //function to stop milling head
		movement = zeroMovement;
		arm.velocity = zeroMovement;
        machineOn = false;
	}

    public void stopHorizontalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 1.0f, 0.0f));
        arm.velocity = zeroMovement;
    }

    public void stopVerticalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 0.0f, 1.0f));
        arm.velocity = zeroMovement;
    }

}
