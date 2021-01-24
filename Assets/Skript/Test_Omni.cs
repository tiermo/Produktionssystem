using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//Author: Sagar Nayak
//Date: 26.10.2017
//Author: Xiang, Yu
//Date: 26.10.2018

//OmniConveyorControl controls omni-directional conveyor belt
public class Test_Omni : MonoBehaviour
{

    private NavMeshAgent agent;                             // used for navigation
    private Vector3 pos;                                    // omni-directional conveyor belt position vector
        private Transform tr;                                   // omni-directional conveyor belt position
    private Vector3 movement;                               // movement vector
    private bool Collisionflag = true;                      // flag to enable collisions
    private bool countDownflag = false;                     // countdown to disable collisions
    private float timeRemaining = 4.0f;
    private bool isObjectOnConveyor = false;                // flag to check if is Object On Conveyor
    private bool monitorFlag = false;                        // flag to notify presence sensor
        AudioSource audio;                                      // omni-directional conveyor audio
        GameObject cube;                                        // access to testCube
    private Rigidbody rigidbody;                            // access to collision objects
        private NavMeshSurface surface;

        private bool z_pos = false;                        // flag to notify z positiv collider
    
    
    private GameObject parent;
    private Quaternion targetRotation;
    private bool middlepostrigger=false;


    // Use this for initialization
    void Start()
    {
        parent = transform.parent.gameObject;
        //parent.transform.Rotate(new Vector3(0, -90, 0));
        targetRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);

        //pos = parent.transform.Find("middleposition").transform.position;
        //Debug.Log("pos : " + pos);
        pos = parent.transform.position;


        
        //tr = transform.parent.GetComponent<Transform>();
        //pos = tr.position;
        //pos.y = 2.31f;
        //audio = GetComponent<AudioSource>();
        //cube = GameObject.Find("TestCube");
        //agent = cube.GetComponent<NavMeshAgent>();
        //agent.enabled = false;

        //surface = GetComponent<NavMeshSurface>();
        //surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
       
        //parent.transform.rotation=Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        
        
        if (countDownflag)
        {                                // start count down
            timeRemaining -= Time.deltaTime;
        }
        if (timeRemaining < 0)
        {
            countDownflag = false;
            Collisionflag = true;
            timeRemaining = 4.0f;
        }

        /*if (agent.enabled == false) {
            audio.Stop ();                                  // agent is disabled if object is not on omni-conveyor
            isObjectOnConveyor = false;
        }*/

        if (monitorFlag && isObjectOnConveyor)
        {            // send acknowledgement id presence sensor is called
            //GetComponent<tcpOmniConveyor> ().onObjectDetection ();
            monitorFlag = false;
        }
    }

    public void moveLeft()
    {                               // function to move an object to the left
        if (isObjectOnConveyor)
        {
            /*pos = tr.position;                                  // update position vector
            pos.y = 2.31f;                                      // adjust height
            //audio.Play();                                       // play audio
            movement = new Vector3(0.0f, 0.0f, -5.0f);
            pos += movement;                                    // add left location movement vector
            agent.destination = pos;                            // set destnation to move object to the left
            */
            pos = parent.transform.position;
            movement = new Vector3(0.0f, 2.2f, 10.0f);
            pos += movement;
            agent.destination = pos;
            /*StartCoroutine(Delay1());
            //Debug.Log("pos : " + pos);
            if (middlepostrigger)
            {
                movement = new Vector3(0.0f, 0.0f, -5.0f);
                pos += movement;
                Debug.Log("pos_after " + pos);
                agent.destination = pos;
            }
            middlepostrigger = false;*/
        }
    }

    public void moveRight()
    {                              // function to move an object to the right
        if (isObjectOnConveyor)
        {
            /*pos = tr.position;
            pos.y = 2.31f;
            //audio.Play();
            movement = new Vector3(0.0f, 0.0f, -10.0f);
            pos += movement;
            agent.destination = pos;*/

            pos = parent.transform.position;
            movement = new Vector3(0.0f, 2.2f, -10.0f);
            pos += movement;
            agent.destination = pos;

        }

    }

    public void moveUp()
    {                                 // function to move an object in upward direction
        if (isObjectOnConveyor)
        {
            Debug.Log("up");
            /*pos = tr.position;
            pos.y = 2.31f;
            audio.Play();
            movement = new Vector3(0.0f, 0.0f, -10.0f);
            pos += movement;
            agent.destination = pos;*/

            pos = parent.transform.position;
            Debug.Log("omni position " + pos);
            movement = new Vector3(10.0f, 2.2f, 0.0f);
            pos += movement;
            Debug.Log("cube position " + pos);
            agent.destination = pos;
        }
    }

    public void moveDown()
    {                               // function to move an object in downward direction
        if (isObjectOnConveyor)
        {
            /*pos = tr.position;
            pos.y = 2.31f;
            audio.Play();
            movement = new Vector3(0.0f, 0.0f, 10.0f);
            pos += movement;
            agent.destination = pos;*/

            pos = parent.transform.position;
            movement = new Vector3(-10.0f, 2.2f, 0.0f);
            pos += movement;
            agent.destination = pos;
        }
    }

    void OnCollisionEnter(Collision collision)              // called when object is on conveyor
    {
        //Debug.Log("oncollision enter");
        isObjectOnConveyor = true;
        //Debug.Log("collisionflag" + Collisionflag);
        if (Collisionflag) {
            agent = collision.gameObject.GetComponent<NavMeshAgent> ();
            //Debug.Log("agent.name" + agent.name);
			agent.enabled = true;                           // enable navigation
            rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            StartCoroutine (Delay ());                      // dealy to move object from omni-conveyor to conveyor using nav-mesh agent
            countDownflag = true;
			Collisionflag = false;
		}
	}

   /* void OnCollisionExit(Collision collision)
    {
        Debug.Log("exit");
    }*/

    public void setMonitorFlag()
    {                           // flag set when presence sensor is called
        monitorFlag = true;
    }

    public bool getObjectSensorStatus()
    {
        return isObjectOnConveyor;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        rigidbody.velocity = new Vector3(0f, 0f, 0f);         //set velocity to 0
        rigidbody.useGravity = true;
    }

    IEnumerator Delay1()
    {
        yield return new WaitForSeconds(2.4f);
        parent.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        middlepostrigger = true;
    }
}