using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//Author: Sagar Nayak
//Date: 26.10.2017
//Author: Xiang, Yu
//Date: 26.10.2018

//OmniConveyorControl controls omni-directional conveyor belt
public class OmniConveyorControl : MonoBehaviour {

	private NavMeshAgent agent;                             // used for navigation
    private Vector3 pos;                                    // omni-directional conveyor belt position vector
    private Vector3 movement;                               // movement vector
    private bool Collisionflag = true;                      // flag to enable collisions
	private bool countDownflag = false;                     // countdown to disable collisions
	private float timeRemaining = 4.0f;
	public static bool isObjectOnConveyor = false;                // flag to check if is Object On Conveyor
    private bool monitorFlag= false;                        // flag to notify presence sensor
	AudioSource audio;                                      // omni-directional conveyor audio
    private Rigidbody rigidbody;                            // access to collision objects
    private NavMeshSurface surface;                         // build nav mesh surface
    private float agentspeed;                               // adjust the speed of agent


    // Use this for initialization
    void Start () {
		pos = transform.position;
		audio = GetComponent<AudioSource> ();
        surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

	// Update is called once per frame
	void Update () {
		if (countDownflag) {                                // start count down
			timeRemaining -= Time.deltaTime; 
		}
		if (timeRemaining < 0) 
        {
			countDownflag = false;
			Collisionflag = true;
			timeRemaining = 4.0f;
		}

        if (isObjectOnConveyor == false)
        {
            audio.Stop();                                  // agent is disabled if object is not on omni-conveyor
            //isObjectOnConveyor = false;
        }   

		if (monitorFlag && isObjectOnConveyor) {            // send acknowledgement id presence sensor is called
			GetComponent<tcpServer_Omni> ().onObjectDetection ();
			monitorFlag= false;
		}
	}

	public void moveLeft (string speed) {                               // function to move an object to the left
        OmniSpeedSelet(speed);
        if (isObjectOnConveyor)
        {
            pos = transform.position;                                  // update position vector
            audio.Play();                                       // play audio
            movement = new Vector3(0.0f, 2.03f, 15.0f);
            pos += movement;                                    // add left location movement vector
            agent.speed = agentspeed;
            agent.destination = pos;                            // set destnation to move object to the lef
        }    
        GetComponent<tcpServer_Omni>().sendBackMessage("finished");
    }

    public void moveRight(string speed){                              // function to move an object to the right
        OmniSpeedSelet(speed);
        if (isObjectOnConveyor)
        {
            pos = transform.position;
            audio.Play();
            movement = new Vector3(0.0f, 2.03f, -15.0f);
            pos += movement;
            agent.speed = agentspeed;
            agent.destination = pos;
        }
        GetComponent<tcpServer_Omni>().sendBackMessage("finished");
	}

	public void moveUp (string speed) {                                 // function to move an object in upward direction
        OmniSpeedSelet(speed);
        if (isObjectOnConveyor)
        {
            pos = transform.position;
            audio.Play();
            movement = new Vector3(20.0f, 2.03f, 0.0f);
            pos += movement;
            agent.speed = agentspeed;
            agent.destination = pos;
        }
        GetComponent<tcpServer_Omni>().sendBackMessage("finished");
    }

    public void moveDown(string speed){                               // function to move an object in downward direction
        OmniSpeedSelet(speed);
        if (isObjectOnConveyor)
        {
            pos = transform.position;
            audio.Play();
            movement = new Vector3(-15.0f, 2.03f, 0.0f);
            pos += movement;
            agent.speed = agentspeed;
            agent.destination = pos;
        }     
        GetComponent<tcpServer_Omni>().sendBackMessage("finished");
    }

    void OnCollisionEnter(Collision collision)              // called when object is on conveyor
    {
        //isObjectOnConveyor = true;
        if (Collisionflag) {
            agent = collision.gameObject.GetComponent<NavMeshAgent> ();
			agent.enabled = true;                           // enable navigation
            rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            StartCoroutine (Delay ());                      // dealy to move object from omni-conveyor to conveyor using nav-mesh agent
            countDownflag = true;
			Collisionflag = false;
		}
	}

    void OmniSpeedSelet(string speed)
    {
        switch (speed)
        {
            case "low":
                agentspeed = 0.5f;              
                break;
            case "norm":
                agentspeed = 1.5f;
                break;
            case "fast":
                agentspeed = 5;
                break;
        }
    }

    public void setMonitorFlag(){                           // flag set when presence sensor is called
		monitorFlag = true;
	}

	public bool getObjectSensorStatus() {
		return isObjectOnConveyor;
	}

   	IEnumerator Delay()
    {
    	yield return new  WaitForSeconds(0.5f);
    	rigidbody.velocity = new Vector3(0f,0f,0f);         //set velocity to 0
        rigidbody.useGravity = true;
    }
}