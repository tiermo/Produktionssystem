using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class Drag_Conveyor : MonoBehaviour
{
    private Transform trans;  //get gameobject's transform

    private Color originalColor; //save the original color of the gameobject
    
    private bool isDrag = false;  //flag if is dragging
    
    private Vector3 ObjScreenSpace; //gameobject's screenposition
    private Vector3 ObjWorldSpace;  //gameobject's worldposition
    private Vector3 MouseScreenSpace;  //mouse's screenposition
    private Vector3 Offset; // offset between screen space and world space

    private Vector3 previousposition; //save the previous position of gameobject
    private int x;
    private int y;
    private string Infostring;
    public string Positionstring;

    private string Conveyorname; //the name of conveyor
    private string previousCollidername;
    private string Collidername; //the name of colider
    private LayerMask mask;  //Calculate the number of "Plane" layermask
    
    private string localEulerAngles; //in order to change the rotation of conveyor belt
    RaycastHit hit;

    private int serverPort;
    private ModulServerClient msc;

    private GameObject t;

    void Start()
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
        trans = GetComponent<Transform>();
        previousposition = trans.position;
        mask = 1 << (LayerMask.NameToLayer("Plane"));

        //get the name and position of gameobject
        /*if (GameObject.Find("Button_ConveyorBelt")==null)
        {
            Debug.Log("null");
            //previousCollidername=GetComponent<
            getInfo();
            Debug.Log("finish");
        }
        else
        {*/
            previousCollidername = GameObject.Find("Button_ConveyorBelt").GetComponent<Create_ConveyorBelt>().SendColliderName();
            x = int.Parse(previousCollidername.Substring(8, 1));
            y = int.Parse(previousCollidername.Substring(9, 1));
            Positionstring = "Position: x=" + x.ToString() + ", y=" + y.ToString();
            Conveyorname = GameObject.Find("Button_ConveyorBelt").GetComponent<Create_ConveyorBelt>().SendConveyorName();
        //}
    }

    void OnMouseEnter()
    {
        if (!isDrag)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    void OnMouseExit()
    {
        if (!isDrag)
        {
            GetComponent<MeshRenderer>().material.color = originalColor;
        }
    }

    void OnMouseDown()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;

        ObjScreenSpace = Camera.main.WorldToScreenPoint(trans.position);
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        //Debug.Log("ObjScreenSpace" + ObjScreenSpace.z);
        Offset = trans.position - Camera.main.ScreenToWorldPoint(MouseScreenSpace);
        //Debug.Log("offset" + Offset);
    }

    void OnMouseDrag()
    {
        isDrag = true;
        GetComponent<MeshRenderer>().material.color = Color.red;
        GameObject.Find(previousCollidername).GetComponent<BoxCollider>().enabled = true;
        
        //gameobject moves with mouse
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        //Debug.Log("ObjScreenSpace" + ObjScreenSpace.z);
        ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace) + Offset;
        if (ObjWorldSpace.y > 0.46f == false)
        {
            ObjWorldSpace.y = 5f;
        }
        trans.position = ObjWorldSpace;

        //change the rotation of gameobject
        localEulerAngles = trans.localEulerAngles.ToString();
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)":
                    trans.rotation = Quaternion.Euler(new Vector3(0, 180, 90));
                    break;
                case "(0.0, 180.0, 90.0)":
                    trans.rotation = Quaternion.Euler(new Vector3(0, 270, 90));
                    break;
                default:
                    trans.rotation = Quaternion.Euler(new Vector3(0, 270, 90));
                    break;
            }
        }

        //delete modul 
        if (Input.GetKeyDown(KeyCode.D))
        {
            serverPort = GetComponent<ConstrutorClient_ConveyorBelt>().getServerPortNr();
            msc = new ModulServerClient(serverPort, "deleted");
            Destroy(trans.gameObject);
        }

        //show the place that gameobject can be placed
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.green);  // project green ray

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
            Collidername = hit.collider.name;
            switch(localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)": //is related to Conveyor1/3/5
                    if (int.Parse(Collidername.Substring(8,1))%2 !=0)   //Format is "Conveyor1/3/5"oder"Conveyor0/2/4/6",get the number and decided if it is odd or even number.
                    {
                        GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    break;
                case "(0.0, 180.0, 90.0)": //is related to Conveyor0/2/4/6
                    if (int.Parse(Collidername.Substring(8, 1)) % 2 == 0)   //Format is "Conveyor1/3/5"oder"Conveyor0/2/4/6",get the number and decided if it is odd or even number.
                    {
                        GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    break;
            }
        }
    }

    void OnMouseUp()
    {
        if (GetComponent<MeshRenderer>().material.color == Color.green)
        {
            switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)":
                    //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
                    Vector3 _offset_row =new Vector3(0,-0.46f,9.75f);
                    trans.position = hit.collider.transform.position-_offset_row;
                    //Debug.Log("trans.position" + trans.position);
                    //GetComponent<ConveyorScript>().enabled = true;      //script for horizontal conveyor belt
                    //GetComponent<ConveyorScript_Vertikal>().enabled = false; 
                    break;
                case "(0.0, 180.0, 90.0)":
                    //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
                    Vector3 _offset_column = new Vector3(-9.67f, -0.46f, 0);
                    trans.position = hit.collider.transform.position - _offset_column;
                    Debug.Log("trans.position" + trans.position);
                    //GetComponent<ConveyorScript>().enabled = false; 
                    //GetComponent<ConveyorScript_Vertikal>().enabled = true; // script for horizontal conveyor belt
                    break;
            }
            previousposition = trans.position;
            hit.collider.GetComponent<BoxCollider>().enabled = false;
            previousCollidername = Collidername;
            x = int.Parse(previousCollidername.Substring(8, 1));
            y = int.Parse(previousCollidername.Substring(9, 1));
            Positionstring = "Position: x=" + x.ToString() + ", y=" + y.ToString();
            //Debug.Log("position string" + Positionstring);
            serverPort = GetComponent<ConstrutorClient_ConveyorBelt>().getServerPortNr();
            msc=new ModulServerClient(serverPort, Positionstring);
            //GetComponent<tcpClient_Conveyor>().SendPosition(serverPort,Positionstring);
        }
        else
        {
            trans.position = previousposition;
            GameObject.Find(previousCollidername).GetComponent<BoxCollider>().enabled = false;
        }
        GetComponent<MeshRenderer>().material.color = originalColor;
        isDrag = false;
    }

    public string SendInfo() {
        Infostring = "%"+Conveyorname + "/"+ x + "/" + y;
        Debug.Log(Infostring);
        return Infostring;
    }

    /*public string ReturnPosition()
    {
        Debug.Log(Positionstring);
        return Positionstring;
    }*/

    private class ModulServerClient
    {
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private string host = "127.0.0.1";

        public ModulServerClient(int port, string data)
        {
            Debug.Log("client port" + port);
            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                writer.WriteLine(data);
                writer.Flush();
                socket.Close();
            }
            catch (Exception e)
            {
                Debug.Log("Socket error : " + e.Message);
            }
        }
    }

    /*private void getInfo()
    { 
        RaycastHit hit;
        Vector3 forward=transform.TransformDirection(Vector3.left);
        
        if (Physics.Raycast(transform.position, forward, out hit, Mathf.Infinity, mask.value))
        {       
            previousCollidername=hit.collider.name;
            Debug.Log("hit");     
        }
        else {
            Debug.Log("did not hit");
        }	
	}*/
}