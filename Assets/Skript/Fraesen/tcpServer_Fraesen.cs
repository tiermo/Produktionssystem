using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017

//tcpMilling controls tcp communication for milling machine
public class tcpServer_Fraesen : MonoBehaviour
{
    private int port = 6801;
    private ServerClient client;
    private TcpListener server;
    private bool serverStarted = false;

    void Start()
    {
        port = transform.parent.gameObject.GetComponent<ConstructorClient_Fraesen>().getModulPortNr();
        if (port == 0)
        {
            port = 6801;
        }
        
        try
        {
            server = new TcpListener(IPAddress.Any, port);  //listen on host adress of PC
            server.Start();
            StartListening();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.Log("socket error: " + e.Message);
        }
    }

    void Update()
    {
        if (!serverStarted)
            return;

        //is the client still connected?
        if (client != null)
        {
            if (!isConnected(client.tcp))
            {
                client.tcp.Close();
            }
            //check for message from the client
            else
            {
                NetworkStream s = client.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    if (data != null)
                    {
                        onIncoming(client, data);
                    }
                }
            }
        }
    }

    private void onIncoming(ServerClient client, string data)
    {  //process requests depending on string message received

        Debug.Log("data " + data);
        if (data.Contains("down"))
        {
            int spaceposition = data.IndexOf(' ');
            int depth = int.Parse(data.Substring(spaceposition + 1));
            GetComponent<FraesenSkript>().moveDown(depth);
        }
        if (data.Contains("up"))
        {
            int spaceposition = data.IndexOf(' ');
            int depth = int.Parse(data.Substring(spaceposition + 1));
            GetComponent<FraesenSkript>().moveUp(depth);
        }
        if (data.Contains("left"))
        {
            int spaceposition = data.IndexOf(' ');
            float distance = float.Parse(data.Substring(spaceposition + 1));
            GetComponent<FraesenSkript>().moveLeft(distance);
        }
        if (data.Contains("right"))
        {
            int spaceposition = data.IndexOf(' ');
            float distance = float.Parse(data.Substring(spaceposition + 1));
            GetComponent<FraesenSkript>().moveRight(distance);
        }
        if (data.Contains("speed"))
        {
            int spaceposition = data.IndexOf(' ');
            string speed = data.Substring(spaceposition + 1);
            GetComponent<FraesenSkript>().SpeedSelect(speed);
        }
        if (string.Compare(data, "middle") == 0)
        {
            GetComponent<FraesenSkript>().moveMiddle();
        }
        if (string.Compare(data, "trigger") == 0)
        {
            GetComponent<FraesenSkript>().callTrigger();
        }
        if (string.Compare(data, "scale") == 0)
        {
            GetComponent<FraesenSkript>().callScale();
        }
        if (string.Compare(data, "on") == 0)
        {
            GetComponent<FraesenSkript>().turnOn();
        }
        if (string.Compare(data, "off") == 0)
        {
            GetComponent<FraesenSkript>().turnOff();
        }
        if (string.Compare(data, "stop") == 0)
        {
            GetComponent<FraesenSkript>().stopMovement();
        }
        if (string.Compare(data, "limitL") == 0)
        {
            GetComponent<FraesenSkript>().callLeftPosSensor();
        }
        if (string.Compare(data, "limitR") == 0)
        {
            GetComponent<FraesenSkript>().callRightPosSensor();
        }
        if (string.Compare(data, "limitU") == 0)
        {
            GetComponent<FraesenSkript>().callLimitSensorUp();
        }
        if (string.Compare(data, "limitD") == 0)
        {
            GetComponent<FraesenSkript>().callLimitSensorDown();
        }
        if (string.Compare(data, "limitltr") == 0)
        {
            GetComponent<FraesenSkript>().callMiddleLtRSensor();
        }
        if (string.Compare(data, "limitrtl") == 0)
        {
            GetComponent<FraesenSkript>().callMiddleRtLSensor();
        }
        if (string.Compare(data, "st") == 0)
        {
            StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
            data = GetComponent<FraesenSkript>().getMachineStatus().ToString();
            writer.WriteLine(data);
            writer.Flush();
        }

    }

    public void sendBackMessage(string data)
    {                    // send service number as acknowledgement
        StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
        writer.WriteLine(data);
        writer.Flush();
    }

  /*  public void LimitSwitchesReached(string data)
    {                     // send service number as acknowledgement when limit switches are called
        if (string.Compare(data, "trigger") == 0)
        {
            sendBackMessage("reached");
            //GetComponent<FraesenSkript>().stopMovement();
        }
        if (string.Compare(data, "limitU") == 0)
        {
            //sendBackMessage("reached");
        }
        if (string.Compare(data, "limitF") == 0)
        {
            sendBackMessage("reached");
        }

    }*/

    private bool isConnected(TcpClient c)
    {      // check if client is connected
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    private void StartListening()
    {
        server.BeginAcceptSocket(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        client = new ServerClient(listener.EndAcceptTcpClient(ar));
        StartListening();
    }

}