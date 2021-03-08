using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class ConstructorClient_StanzenMessen : MonoBehaviour
{

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
    private int sensorPortNr;

    private GameObject t;   //son object "Arm"
    private GameObject s;   //son object "Abstandssensor"

    void Start()
    {
        t = transform.Find("Arm").gameObject;
        s = transform.Find("Abstandssensor").gameObject;
        data = GetComponent<Drag_StanzenPruefen>().SendInfo();
        ConnectToServer();
        Send(data);
    }

    void Update()
    {
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
            sensorPortNr = Int32.Parse(array[2]);

            if (serverport == 0 || modulPortNr == 0 || sensorPortNr ==0)
            {
                Debug.Log("error : port number is null");
            }
            else
            {
                t.GetComponent<tcpServer_Stanzen_SM>().enabled = true;
                t.GetComponent<StanzenSkript_SM>().enabled = true;
                s.GetComponent<tcpServer_Messen_SM>().enabled = true;
                s.GetComponent<MessenScript_SM>().enabled = true;
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

    public int getSensorPortNr()
    {
        return sensorPortNr;
    }
}
