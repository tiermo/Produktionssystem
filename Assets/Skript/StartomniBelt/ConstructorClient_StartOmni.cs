using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class ConstructorClient_StartOmni : MonoBehaviour
{

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string data="%omniBelt/0/0"; 
    private string host = "127.0.0.1";
    private int port = 9999;
    private int serverport;
    private int omniPortNr;

    void Start()
    {
        /*Invoke("DelayOpen", 5); //delay 5s 
    }

    void DelayOpen()
    {*/
        ConnectToServer();
        Send(data);
        //CancelInvoke("DelayOpen");
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
            omniPortNr = Int32.Parse(array[1]);
            Debug.Log("serverport " + serverport + "omniPN " + omniPortNr);

            if (serverport == 0 || omniPortNr == 0)
            {
                Debug.Log("error : port number is null");
                //show info and destroy object
            }
            else
            {
                GetComponent<tcpServer_StartOmni>().enabled = true;
                GetComponent<Start_OmniConveyorControl>().enabled = true;
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

    public int getOmniPortNr()
    {
        return omniPortNr;
    }
}
