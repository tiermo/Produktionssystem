using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class Drag_Omni : MonoBehaviour
{
    private Transform trans;          //get gameobject's transform
    private Color originalColor;      //save the original color of the gameobject
    private bool isDrag = false;      //flag if is dragging

    private Vector3 ObjScreenSpace;   //gameobject's screenposition
    private Vector3 ObjWorldSpace;    //gameobject's worldposition
    private Vector3 MouseScreenSpace; //mouse's screenposition
    private Vector3 Offset;           //offset between screen space and world space

    private Vector3 previousposition; //save the previous position of gameobject
    private int x;                    //position x of modul
    private int y;                    //position y of modul
    private string Infostring;        //information about modul name and position, will been called by ConstructorClient and sent to constructor server
    public string Positionstring;     //position of this modul, send directly to Modulserver

    private string Omniname;          //the name of Omni
    private string previousCollidername; //save previous Collider
    private string Collidername;      //the  name of colider
    private LayerMask mask;           //Calculate the number of "Omni" layermask

    RaycastHit hit;
    private int serverPort;           //server port of this modul
    private ModulServerClient msc;

    //private ConfigManager ConfigManager = new ConfigManager(); // so the Config can be updated

    void Start()
    {
        //get the name and position of gameobject
        previousCollidername = GameObject.Find("Button_OmniBelt").GetComponent<create_Omni>().SendColliderName();
        x = int.Parse(previousCollidername.Substring(4, 1));
        y = int.Parse(previousCollidername.Substring(5, 1));
        Positionstring = "Position: x=" + x.ToString() + ", y=" + y.ToString();

        Omniname = GameObject.Find("Button_OmniBelt").GetComponent<create_Omni>().SendOmniName();
        originalColor = GetComponent<MeshRenderer>().material.color;
        trans = GetComponent<Transform>();
        previousposition = trans.position;
        mask = 1 << (LayerMask.NameToLayer("Omni"));

        serverPort = GetComponent<ConstructorClient_Omni>().getServerPortNr();
        Debug.Log("serverPost_omni" + serverPort);
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
        Offset = trans.position - Camera.main.ScreenToWorldPoint(MouseScreenSpace);

        GameObject.Find(previousCollidername);
        ConfigManager.changeConfig("OLM", previousCollidername, ProductionModule.KeinModul, false); // update the current Config 
    }

    void OnMouseDrag()
    {
        isDrag = true;
        GetComponent<MeshRenderer>().material.color = Color.red;
        //the previous collider should be activated again for the other omni modul
        GameObject.Find(previousCollidername).GetComponent<BoxCollider>().enabled = true;

        //gameobject moves with mouse
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace) + Offset;
        if (ObjWorldSpace.y > 0.55f == false)
        {
            ObjWorldSpace.y = 4f;
        }
        trans.position = ObjWorldSpace;

        //delete modul 
        if (Input.GetKeyDown(KeyCode.D))
        {
            serverPort = GetComponent<ConstructorClient_Omni>().getServerPortNr();
            msc = new ModulServerClient(serverPort, "deleted");
            Destroy(trans.gameObject);
        }
        
        //show the place that gameobject can be placed
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
            Collidername = hit.collider.name;
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    void OnMouseUp()
    {
        if (GetComponent<MeshRenderer>().material.color == Color.green)
        {
            Vector3 _offset = new Vector3(0.2f, -0.55f, 0);
            transform.position = hit.collider.transform.position - _offset;

            previousposition = trans.position;
            hit.collider.GetComponent<BoxCollider>().enabled = false;
            previousCollidername = Collidername;

            ConfigManager.changeConfig("OLM", previousCollidername, ProductionModule.KeinModul, true); // Update the current Config

            x = int.Parse(previousCollidername.Substring(4, 1));
            y = int.Parse(previousCollidername.Substring(5, 1));
            Positionstring = "Position: x=" + x.ToString() + ", y=" + y.ToString();
            serverPort = GetComponent<ConstructorClient_Omni>().getServerPortNr();
            msc = new ModulServerClient(serverPort, Positionstring);
        }
        else
        {
            trans.position = previousposition;
            GameObject.Find(previousCollidername).GetComponent<BoxCollider>().enabled = false;

            ConfigManager.changeConfig("OLM", previousCollidername, ProductionModule.KeinModul, true); // Update the current Config
        }
        GetComponent<MeshRenderer>().material.color = originalColor;
        isDrag = false;
    }

    public string SendInfo()
    {
        Infostring = "%" + Omniname + "/" + x + "/" + y;
        Debug.Log(Infostring);
        return Infostring;
    }

    private class ModulServerClient
    {
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private string serverhost = "127.0.0.1";

        public ModulServerClient(int port, string data)
        {
            Debug.Log("client port" + port);
            try
            {
                socket = new TcpClient(serverhost, port);
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