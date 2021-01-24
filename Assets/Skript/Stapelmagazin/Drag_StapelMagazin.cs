using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class Drag_StapelMagazin : MonoBehaviour
{

    private Transform trans;             //get gameobject's transform

    private Color originalColor;         //save the original color of the gameobject

    private bool isDrag = false;         //flag if is dragging

    private Vector3 ObjScreenSpace;      //gameobject's screenposition
    private Vector3 ObjWorldSpace;       //gameobject's worldposition
    private Vector3 MouseScreenSpace;    //mouse's screenposition
    private Vector3 Offset;              //offset between screen space and world space

    private Vector3 previousposition;    //save the previous position of gameobject
    private float x;                       //position x of modul
    private float y;                       //position y of modul
    private string Infostring;
    private string Positionstring;

    private string Modulname;            //the name of modul
    private string previousCollidername;
    private string Collidername;         //the name of colider
    private LayerMask mask;              //Calculate the number of "Modul" layermask

    private string localEulerAngles;     //in order to change the rotation of conveyor belt
    RaycastHit hit;

    private int serverPort;
    private ModulServerClient msc;

    void Start()
    {
        //get the name and position of gameobject
        previousCollidername = GameObject.Find("StapelMagazin").GetComponent<Create_StapelMagazin>().SendColliderName();
        if (int.Parse(previousCollidername.Substring(6, 1)) % 2 == 0)
        {
            x = float.Parse(previousCollidername.Substring(5, 1)) / float.Parse(previousCollidername.Substring(6, 1));
            y = float.Parse(previousCollidername.Substring(7, 1));

        }
        else
        {
            x = float.Parse(previousCollidername.Substring(5, 1));
            y = float.Parse(previousCollidername.Substring(6, 1)) / float.Parse(previousCollidername.Substring(7, 1));
        }

        Modulname = GameObject.Find("StapelMagazin").GetComponent<Create_StapelMagazin>().SendModulName();
        originalColor = GetComponent<MeshRenderer>().material.color;

        trans = GetComponent<Transform>();
        previousposition = trans.position;
        mask = 1 << (LayerMask.NameToLayer("Modul"));
    }

    void OnMouseEnter()
    {
        if (!isDrag)
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
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
        GetComponent<MeshRenderer>().material.color = Color.yellow;

        ObjScreenSpace = Camera.main.WorldToScreenPoint(trans.position);
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        Offset = trans.position - Camera.main.ScreenToWorldPoint(MouseScreenSpace);
    }

    void OnMouseDrag()
    {
        isDrag = true;
        GetComponent<MeshRenderer>().material.color = Color.yellow;
        GameObject.Find(previousCollidername).GetComponent<BoxCollider>().enabled = true;

        //gameobject moves with mouse
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace) + Offset;
        if (ObjWorldSpace.y > 7.78f == false)
        {
            ObjWorldSpace.y = 9f;
        }
        trans.position = ObjWorldSpace;

        //change the rotation of gameobject
        localEulerAngles = trans.localEulerAngles.ToString();
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (localEulerAngles)
            {
                case "(270.0, 0.0, 0.0)":
                    trans.rotation = Quaternion.Euler(new Vector3(270, 270, 0));
                    break;
                case "(270.0, 270.0, 0.0)":
                    trans.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
                    break;
                default:
                    trans.rotation = Quaternion.Euler(new Vector3(270, 270, 0));
                    break;
            }
        }
        
        //delete modul 
        if (Input.GetKeyDown(KeyCode.D))
        {
            serverPort = GetComponent<ConstructorClient_StapelMagazin>().getServerPortNr();
            msc = new ModulServerClient(serverPort, "deleted");
            Destroy(trans.gameObject);
        }

        //show the place that gameobject can be placed
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
            Collidername = hit.collider.name;
            switch (localEulerAngles)
            {
                case "(270.0, 0.0, 0.0)": //should put on the side of horizontal conveyor
                    if (int.Parse(Collidername.Substring(6, 1)) % 2 == 0)   //Format is "Modul#2#",get the middle number, it should be even.
                    {
                        GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    break;
                case "(270.0, 270.0, 0.0)": //should put on the side of vertical conveyor
                    if (int.Parse(Collidername.Substring(6, 1)) % 2 != 0)   //Format is "Modul#1/3/5#",get the middle number, it should be odd number.
                    {
                        GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.color = Color.yellow;
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
                case "(270.0, 0.0, 0.0)":
                    //Debug.Log("Collider position " + hit.collider.transform.position);
                    Vector3 _offset_row = new Vector3(9.52f, -7.78f, 0.01f);
                    trans.position = hit.collider.transform.position - _offset_row;
                    //Debug.Log("Modul position " + trans.position);
                    break;
                case "(270.0, 270.0, 0.0)":
                    //Debug.Log("Collider position " + hit.collider.transform.position);
                    Vector3 _offset_column = new Vector3(0.18f, -7.78f, 9.42f);
                    trans.position = hit.collider.transform.position - _offset_column;
                    //Debug.Log("Modul position " + trans.position);
                    break;
            }
            previousposition = trans.position;
            hit.collider.GetComponent<BoxCollider>().enabled = false;
            previousCollidername = Collidername;
            if (int.Parse(previousCollidername.Substring(6, 1)) % 2 == 0)
            {
                x = float.Parse(previousCollidername.Substring(5, 1)) / float.Parse(previousCollidername.Substring(6, 1));
                y = float.Parse(previousCollidername.Substring(7, 1));

            }
            else
            {
                x = float.Parse(previousCollidername.Substring(5, 1));
                y = float.Parse(previousCollidername.Substring(6, 1)) / float.Parse(previousCollidername.Substring(7, 1));
            }

            Positionstring = "Position: x=" + x.ToString("0.0") + ", y=" + y.ToString("0.0");
            //Debug.Log("position string" + Positionstring);
            serverPort = GetComponent<ConstructorClient_StapelMagazin>().getServerPortNr();
            msc = new ModulServerClient(serverPort, Positionstring);
        }
        else
        {
            trans.position = previousposition;
            GameObject.Find(previousCollidername).GetComponent<BoxCollider>().enabled = false;
        }
        GetComponent<MeshRenderer>().material.color = originalColor;
        isDrag = false;
    }


    public string SendInfo()
    {
        Infostring = "%" + Modulname + "/" + x + "/" + y;
        Debug.Log(Infostring);
        return Infostring;
    }
    public string ReturnPosition()
    {
        Debug.Log(Positionstring);
        return Positionstring;
    }

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

}
