using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class tcpServer_Senke : MonoBehaviour {

    private int port;
    private ServerClient client;
    private TcpListener server;
    private bool serverStarted = false;
    private int depth;
    private string speed;

    // Use this for initialization
    void Start () {
        
        port = transform.parent.gameObject.GetComponent<ConstructorClient_Senke>().getModulPortNr();
       
        if (port == 0)
        {
            port = 7201;
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
	
	// Update is called once per frame
	void Update () {
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
        Debug.Log(data);
        if (data.Contains("down"))
        {
            GetComponent<Senke_Script>().moveDown();
        }
        
        else if (data.Contains("service"))
        {
            GetComponent<Senke_Script>().forwardInformation(data);
        } else if (data.Contains("up"))
        {
            GetComponent<Senke_Script>().moveUp();
        }
        else if (data.Contains("trigger"))
        {
            GetComponent<Senke_Script>().callTrigger();
        }

    }

    public void LimitSwitchesReached(string data)
    {                     // send service number as acknowledgement when limit switches are called
      
        if (string.Compare(data, "limitU") == 0)
        {
            sendBackMessage("reached");
        }
    }


    public void sendBackMessage(string data)
    {                    // send service number as acknowledgement
        StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
        writer.WriteLine(data);
        Debug.Log(data);
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
