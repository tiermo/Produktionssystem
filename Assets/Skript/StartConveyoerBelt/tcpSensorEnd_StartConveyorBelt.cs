using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017

//tcpSensor controls tcp communication for sensors
public class tcpSensorEnd_StartConveyorBelt : MonoBehaviour
{
    public static int port;
    private ServerClient client;
    private TcpListener server;

    private GameObject g;

    private bool serverStarted = false;

    void Start()
    {
        g = transform.parent.gameObject;
        port = g.GetComponent<ConstructorClient_StartConveyorBelt>().getSensorPortEndNr();
        if (port == 0)
        {
            port = 6360;
        }
        Debug.Log("end port " + port);

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
    {   //process requests depending on string message received
        Debug.Log("onincoming data end sensor " + data);
        if (string.Compare(data, "st") == 0)
        {
            GetComponent<sensorEnd_StartConveyorBelt>().setMonitorFlag();
        }

        if (string.Compare(data, "st_off") == 0)
        {
            GetComponent<sensorEnd_StartConveyorBelt>().deactiveMonitorFlag();
        }
    }

    public void onObjectDetection()
    {   // send "detected" as acknowledgement
        StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
        writer.WriteLine("detected");
        writer.Flush();
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