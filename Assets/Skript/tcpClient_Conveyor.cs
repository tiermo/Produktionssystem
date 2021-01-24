using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class tcpClient_Conveyor : MonoBehaviour
{

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    //private StreamReader reader;
    private string host = "127.0.0.1";
    //public static int serverport;

    /*void Start()
    {
        serverport = GetComponent<ConstrutorClient_ConveyorBelt>().getServerPortNr();
        //Debug.Log("client serverport : " + serverport);
        GetComponent<ConstrutorClient_ConveyorBelt>().enabled = false;
    }*/

    public void SendPosition(int port,string data)
    {
        Debug.Log("client port"+port);
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            writer.WriteLine(data);
            writer.Flush();

            /*if (stream.DataAvailable)
            {
                //Debug.Log("hi");
                string info = reader.ReadLine();
                Debug.Log("info" + info);
                if ((info.CompareTo("received position")) ==0)
                {
                    Debug.Log("OK");
                    //Debug.Log("wrong info :" + info);
                }
            }*/
            socket.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }
}
    
    /*void Start() 
    {
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
            socket = new TcpClient(host, serverport);
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
        /*Debug.Log("Server says : " + data);
        if (data.Equals("changed"))
        {
            Debug.Log("position changed");
            socket.Close();
            return;
        }

        if (data.Contains("/"))
        {
            string[] array = data.Split(new char[] {'/'});
            serverport = Int32.Parse(array[0]);
            conveyorPortNr = Int32.Parse(array[1]);
            sensorPortNr = Int32.Parse(array[2]);
            Debug.Log("serverport " + serverport + "conveyorPN " + conveyorPortNr + " sensorPN " + sensorPortNr);

            if (serverport == 0 || conveyorPortNr == 0 || sensorPortNr == 0)
            {
                Debug.Log("error : port number is null");
            }
            else
            {
                GetComponent<tcpServer_ConveyorBelt>().enabled = true;
                CloseSocket();
                socketReady = false;
                //ConnectToServer(serverport);
                //ModulServerEnabled = true;
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

    public void SendPosition(int Serverport,string positionstring)
    {
        ConnectToServer(Serverport);
        Send(positionstring);
        socket.Close();
        /*newposition = GetComponent<Drag_Conveyor>().ReturnPosition();
        if (String.Compare(previousposition,newposition)==0)
        {
            //ConnectToServer(serverport);
            Send(newposition);
            previousposition = newposition;
        }
    }
}*/
