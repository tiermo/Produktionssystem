using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017

//tcpDrilling controls tcp communication for drlling machines
public class tcpServer_Stanzen_SM : MonoBehaviour
{
    private int port = 7001;
    private ServerClient client;
    private TcpListener server;
    private bool serverStarted = false;

    private string speed;

    void Start()
    {
        port = transform.parent.gameObject.GetComponent<ConstructorClient_StanzenMessen>().getModulPortNr();
        if (port == 0)
        {
            port = 7001;
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
    {
        if (data.Contains("speed"))
        {
            int spaceposition = data.IndexOf(' ');
            speed = data.Substring(spaceposition + 1);
            GetComponent<StanzenSkript_SM>().SpeedSelect(speed);
        }else{
            if (string.Compare(data, "up") == 0)
            {
                GetComponent<StanzenSkript_SM>().moveUp();
            }
            if (string.Compare(data, "down") == 0)
            {
                GetComponent<StanzenSkript_SM>().moveDown();
            }
            if (string.Compare(data, "stop") == 0)
            {
                GetComponent<StanzenSkript_SM>().stopMovement();
            }
            if (string.Compare(data, "limitU") == 0)
            {
                GetComponent<StanzenSkript_SM>().callLimitSensorUp();
            }
            if (string.Compare(data, "limitD") == 0)
            {
                GetComponent<StanzenSkript_SM>().callLimitSensorDown();
            }
            if (string.Compare(data, "st") == 0)
            {
                StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
                data = GetComponent<StanzenSkript_SM>().getMachineStatus().ToString();
                writer.WriteLine(data);
                writer.Flush();
            }
        }
    }

    public void sendBackMessage(string data)
    {                    // send service number as acknowledgement
        StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
        writer.WriteLine(data);
        writer.Flush();
    }

    public void LimitSwitchesReached(string data)
    {                     // send service number as acknowledgement when limit switches are called
        if (string.Compare(data, "limitD") == 0)
        {
            sendBackMessage("reached");
        }
        if (string.Compare(data, "limitU") == 0)
        {
            sendBackMessage("reached");
        }
    }

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
