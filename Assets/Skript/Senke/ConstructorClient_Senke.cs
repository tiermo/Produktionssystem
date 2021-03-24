using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class ConstructorClient_Senke : MonoBehaviour {
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private string data;
    private string host = "127.0.0.1";
    private int port = 9999;
    private int serverport;
    private int modulPortNr;

    private GameObject t;   //son object "Arm"

    // Use this for initialization
    void Start () {
        t = transform.Find("Arm").gameObject;
        //t = transform.Find("Sensoren").gameObject;
        Debug.Log(t);
        Debug.Log(t.GetComponent<tcpServer_Senke>().transform.parent);
        data = GetComponent<Drag_Senke>().SendInfo();
        Debug.Log("ConstructorClient Info :" + GetComponent<Drag_Senke>().SendInfo());
        ConnectToServer();
        Send(data);
    }
	
	// Update is called once per frame
	void Update () {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    private void ConnectToServer()
    {
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    private void OnIncomingData(string data)
    {
        if (data.Contains("/"))
        {

            string[] array = data.Split(new char[] { '/' });
            serverport = Int32.Parse(array[0]);
            modulPortNr = Int32.Parse(array[1]);

            if (serverport == 0 || modulPortNr == 0)
            {
                Debug.Log("error : port number is null");
                //Destory(transform.gameObject);
            }
            else
            {
                t.GetComponent<tcpServer_Senke>().enabled = true;
                t.GetComponent<Senke_Script>().enabled = true;
            }
        }
        else
        {
            Debug.Log("error : wrong Information from constructor server");
        }
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;
        writer.WriteLine(data);
        writer.Flush();
    }

    public int getServerPortNr()
    {
        return serverport;
    }

    public int getModulPortNr()
    {
        return modulPortNr;
    }
}
