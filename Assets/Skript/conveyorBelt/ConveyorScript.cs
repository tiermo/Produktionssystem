using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017
//Author: Xiang, Yu
//Date: 26.10.2018

//ConveyorScript controls conveyor belt
public class ConveyorScript : MonoBehaviour {

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

    
    private ConfigurationHelper configHelper = new ConfigurationHelper();
    private ConvertTime timeConverter = new ConvertTime();


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
        conveyorDirectionVector = transform.rotation * Vector3.down;

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
       
        r = rigidbody;
        StartCoroutine(Delay());                                //delay to move object from omni-conveyor to conveyor using nav-mesh agent
    }

	public void OnCollisionExit(Collision collision) {                 //if object is not on conveyor anymore, remove it to listOfRigidbodiesOnConveyor
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
        //listOfRigidbodiesOnConveyor.Remove(rigidbody);
        r = rigidbody;
        StartCoroutine(Delay2());                               //delay to move object from conveyor to  omni-conveyor
       
    }

   /* public void removeCube(GameObject cube)
    {
        Rigidbody rigidbody = cube.GetComponent<Rigidbody>();
        //listOfRigidbodiesOnConveyor.Remove(rigidbody);
        r = rigidbody;
        r.useGravity = true;
        listOfRigidbodiesOnConveyor.Remove(r);
        isObjectOnConveyor = false;
        Debug.Log("exit number" + listOfRigidbodiesOnConveyor.Count);
    }*/

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
	}

	public void setConveyorDirectionDownRight(string speed) {               //conveyor backward function
        ConveyorSpeedSelet(speed);
		conveyorDriveSpeed = -conveyorDriveSpeed;
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

    public void forwardInformation(string data)
    {
        string[] nameSplit;
        nameSplit = data.Split(" "[0]);
        string message;

        if (nameSplit[2] == "servicename")
        {
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName("conveyorBelt"), configHelper.getServiceName("conveyorBelt"), "10", "4");
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName("conveyorBelt"), configHelper.getServiceName("conveyorBelt"), configHelper.getDrehzahl(configHelper.getServiceName("conveyorBelt")), configHelper.getLength(configHelper.getServiceName("conveyorBelt"))));

        }
        else
        {
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName("conveyorBelt"), nameSplit[2], nameSplit[3], nameSplit[4]);
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName("conveyorBelt"), nameSplit[2], nameSplit[3], nameSplit[4]));

        }
        GetComponent<tcpServer_ConveyorBelt>().sendBackMessage(message);
    }
}
