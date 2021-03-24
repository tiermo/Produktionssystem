using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class ConstrutorClient_ConveyorBelt : MonoBehaviour {

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private string data; 
    private string host = "127.0.0.1";
    private int port = 9999;
    private int serverport;
    private int conveyorPortNr;
    private int sensorPortStartNr;
    private int sensorPortMidNr;
    private int sensorPortEndNr;

    private GameObject sensorStart; //son object "sensor"
    private GameObject sensorMid; //son object "sensor"
    private GameObject sensorEnd; //son object "sensor"

    void Start()
    {
        sensorStart = transform.Find("sensorStart").gameObject;
        sensorMid = transform.Find("sensorMid").gameObject;
        sensorEnd = transform.Find("sensorEnd").gameObject;
        data = GetComponent<Drag_Conveyor>().SendInfo();
        ConnectToServer();
        Debug.Log(data);
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
            socket = new TcpClient(host,port);
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
            conveyorPortNr = Int32.Parse(array[1]);
            sensorPortStartNr = Int32.Parse(array[2]);
            sensorPortMidNr = Int32.Parse(array[3]);
            sensorPortEndNr = Int32.Parse(array[4]);
            Debug.Log("serverport " + serverport + "conveyorPN " + conveyorPortNr + " sensorstartPN " + sensorPortStartNr + "mid "+sensorPortMidNr + "end "+sensorPortEndNr);

            if (serverport == 0 || conveyorPortNr == 0 || sensorPortStartNr == 0 || sensorPortMidNr == 0 || sensorPortEndNr == 0)
            {
                Debug.Log("error : port number is null");
                //show info and destroy object
            }
            else
            {
                sensorStart.GetComponent<tcpSensorStart_ConveyorBelt>().enabled = true;
                sensorStart.GetComponent<sensorStart_ConveyorBelt>().enabled = true;

                sensorMid.GetComponent<tcpSensorMid_ConveyorBelt>().enabled = true;
                sensorMid.GetComponent<sensorMid_ConveyorBelt>().enabled = true;

                sensorEnd.GetComponent<tcpSensorEnd_ConveyorBelt>().enabled = true;
                sensorEnd.GetComponent<sensorEnd_ConveyorBelt>().enabled = true;

                GetComponent<tcpServer_ConveyorBelt>().enabled = true;
                GetComponent<ConveyorScript>().enabled = true;
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
        //Debug.Log("sending info");
        writer.WriteLine(data);
        writer.Flush();
    }

    public int getServerPortNr()
    {
        return serverport;
    }

    public int getConveyorPortNr()
    {
        //Debug.Log("conveyor Port Number " + conveyorPortNr);
        return conveyorPortNr;
    }

    public int getSensorPortStartNr()
    {
        return sensorPortStartNr;
    }

    public int getSensorPortMidNr()
    {
        return sensorPortMidNr;
    }

    public int getSensorPortEndNr()
    {
        return sensorPortEndNr;
    }
}
