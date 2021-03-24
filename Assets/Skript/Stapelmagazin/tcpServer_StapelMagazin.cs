using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Yu Xiang
//Date: 

public class tcpServer_StapelMagazin : MonoBehaviour
{
    private int port=6401;             // listening port

    private ServerClient client;
    private TcpListener server;
    private bool serverStarted = false;

    private string material;     // substring get from data, represent the material of workpiece
    private string height;         // substring get from data, represent the height of workpiece
    private int spaceposition;    //space position in the data

    //materialformat: red, black, metall
    //heightforamt: short, tall

    void Start()
    {
        port = transform.parent.gameObject.GetComponent<ConstructorClient_StapelMagazin>().getModulPortNr();

        try
        {
            server = new TcpListener(IPAddress.Any, port);  //listen on host adress of PC
            server.Start();
            StartListening();
            serverStarted = true;
            Debug.Log("server has been started, and waiting for the connection...");
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
                //Debug.Log("Socket closed");
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
        
        spaceposition = data.IndexOf(' ');
        material = data.Substring(0, spaceposition);
        height = data.Substring(spaceposition + 1);

        if (string.Compare(material, "red") == 0)
        {
            GetComponent<StapelMagazinSkript>().CreateRed(height);
        }
        if (string.Compare(material, "black") == 0)
        {
            GetComponent<StapelMagazinSkript>().CreateBlack(height);
        }
        if (string.Compare(material, "metall") == 0)
        {
            GetComponent<StapelMagazinSkript>().CreateMetall(height);
        }
        if (string.Compare(material, "start") == 0)
        {
            GetComponent<StapelMagazinSkript>().startMonitoring();
        }

    }

    public void sendBackMessage(string data)
    {   
        StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
        writer.WriteLine(data);
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


