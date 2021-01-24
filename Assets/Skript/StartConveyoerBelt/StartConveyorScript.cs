using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

//Author: Sagar Nayak
//Date: 26.10.2017
//Author: Xiang, Yu
//Date: 26.10.2018

//ConveyorScript controls conveyor belt
public class StartConveyorScript : MonoBehaviour {

    public bool conveyorOn;                                 // turn conveyor on or off                                   //mapping: conveyorOn
	public float conveyorDriveSpeed;                        // give the conveyor a fixed speed to match when turned on   //mapping: conveyorDriveSpeed

	private float conveyorSpeed;                            // real conveyor belt speed                                  //mapping: conveyorSpeed
	private float previousConveyorSpeed;                    // store previous conveyor speed
	private Vector3 conveyorDirectionVector;                // vector to indicate direction of travel
	private Vector3 conveyorVelocityVector;                 // vector to indicate the conveyor velocity

    private float conveyorBeltMaterialOffset;               // offset for conveyor belt texture                          //mapping: Offset
	//public float conveyorBeltMaterialOffsetConstant;        // offest for objects on conveyor 

	private List<Rigidbody> listOfRigidbodiesOnConveyor;    // the list of objects on the conveyor belt.
	private NavMeshAgent agent;                             // used for navigation
	private Rigidbody r;                                    // access to collision objects

    private NavMeshSurface surface;

	private bool isObjectOnConveyor = false;                // dont use anymore
    //AudioSource audio;                                      // conveyor audio

	// for initialization
	void Start () {
		listOfRigidbodiesOnConveyor = new List<Rigidbody>();
		previousConveyorSpeed = conveyorSpeed;
		conveyorDirectionVector = transform.rotation * Vector3.down;
		//audio = GetComponent<AudioSource> ();

        surface = transform.Find("NavMeshBaker").gameObject.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
	}
	
	// called once per frame
	void Update () {
		if(conveyorOn){
			conveyorSpeed = conveyorDriveSpeed;
		}else{
			conveyorSpeed = 0f;
		}

		//Shift the belt texture to show that belt is moving
		//conveyorBeltMaterialOffset += conveyorBeltMaterialOffsetConstant * conveyorSpeed * Time.deltaTime;
        conveyorBeltMaterialOffset = conveyorSpeed * Time.time;
        GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2( 0f ,conveyorBeltMaterialOffset));
		
        //adjust the speed of the objects on the conveyor
		if (conveyorSpeed != previousConveyorSpeed) {
            if (listOfRigidbodiesOnConveyor.Count > 0) {
				// Remove the velocity component previously added.
				foreach (Rigidbody rigidbody in listOfRigidbodiesOnConveyor) {
                    r.velocity -= conveyorVelocityVector;
				}
			}
			//Adjust the velocity component
			conveyorVelocityVector = conveyorDirectionVector * conveyorSpeed;
			if (listOfRigidbodiesOnConveyor.Count > 0) {
				//Add the new velocity component 
				foreach (Rigidbody rigidbody in listOfRigidbodiesOnConveyor) {
					r.velocity += conveyorVelocityVector;
				}
			}
			previousConveyorSpeed = conveyorSpeed;
		}
	}

	void OnCollisionEnter(Collision collision) {                //if object collides with conveyor, add it to listOfRigidbodiesOnConveyor
		Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
		agent = collision.gameObject.GetComponent<NavMeshAgent>();
		listOfRigidbodiesOnConveyor.Add (rigidbody);
        Debug.Log("enter number" + listOfRigidbodiesOnConveyor.Count);
        r = rigidbody;
        StartCoroutine(Delay());                                //delay to move object from omni-conveyor to conveyor using nav-mesh agent
    }

	void OnCollisionExit(Collision collision) {                 //if object is not on conveyor anymore, remove it to listOfRigidbodiesOnConveyor
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
		r = rigidbody;
        StartCoroutine(Delay2());                               //delay to move object from conveyor to  omni-conveyor
        Debug.Log("exit number" + listOfRigidbodiesOnConveyor.Count);
    }

	IEnumerator Delay()
	{
		yield return new  WaitForSeconds(0.6f);
        r.velocity = new Vector3(0f, 0f, 0f);                   //set velocity to 0
        r.useGravity = false;
        r.freezeRotation = true;
        r.velocity += conveyorVelocityVector;
        agent.enabled = false;
        isObjectOnConveyor = true;
	}

	IEnumerator Delay2()
	{
		yield return new  WaitForSeconds(0.5f);
        r.useGravity = true;
        listOfRigidbodiesOnConveyor.Remove(r);
		isObjectOnConveyor = false;
	}

	public void ConveyorOn() {                                  //turn conveyor On
		conveyorOn = true;
		//audio.Play();
	}

	public void ConveyorOff() {                                 //turn conveyor Off
		conveyorOn = false;
		//audio.Stop();
	}

	public bool getConveyorStatus() {                           //conveyor status: on or off                           
		return conveyorOn;
	}

	public void setConveyorDirectionUpLeft(string speed) {                  //conveyor forward function
        ConveyorSpeedSelet(speed);
        GetComponent<tcpServer_StartConveyorBelt>().sendBackMessage("finished");
	}

	public void setConveyorDirectionDownRight(string speed) {               //conveyor backward function
        ConveyorSpeedSelet(speed);
		conveyorDriveSpeed = -conveyorDriveSpeed;
        GetComponent<tcpServer_StartConveyorBelt>().sendBackMessage("finished");
	}

    void ConveyorSpeedSelet(string speed)
    {
        switch (speed)
        {
            case "low":
                conveyorDriveSpeed = 0.5f;
                break;
            case "norm":
                conveyorDriveSpeed = 1.5f;
                break;
            case "fast":
                conveyorDriveSpeed = 3;
                break;
        }
    }
	public bool getConveyorObjectSensorStatus() {               //is object on conveyor? return true, else false
        return isObjectOnConveyor;
	}
}
