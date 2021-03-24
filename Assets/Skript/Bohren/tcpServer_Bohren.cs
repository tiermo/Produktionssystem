using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class tcpServer_Bohren : MonoBehaviour
{
    private int port = 6601;
    private ServerClient client;
    private TcpListener server;
    private bool serverStarted = false;
    private int depth;
    private string speed;

    void Start()
    {
        
        port = transform.parent.gameObject.GetComponent<ConstructorClient_Bohren>().getModulPortNr();
        if (port == 0)
        {
            port = 6601;
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
                        Debug.Log("oncoming data" + data);
                        onIncoming(client, data);
                    }
                }
            }
        }
    }

    private void onIncoming(ServerClient client, string data)
    {  //process requests depending on string message received
        
        if (data.Contains("down"))
        {
            int spaceposition = data.IndexOf(' ');
            depth = int.Parse(data.Substring(spaceposition + 1));
            GetComponent<BohrenScript>().moveDown(depth);
        }
        else if (data.Contains("speed"))
        {
            string[] orderSplit;
            orderSplit = data.Split(" "[0]);

            //int spaceposition = data.IndexOf(' ');
            //speed = data.Substring(spaceposition + 1);
            GetComponent<BohrenScript>().SpeedSelect(orderSplit[1]);
        }
        
        else if (data.Contains("service"))
        {
            GetComponent<BohrenScript>().forwardInformation(data);
            
        }
        else
        {
            if (string.Compare(data, "trigger") == 0)
            {
                GetComponent<BohrenScript>().callTrigger();
            }

            if (string.Compare(data, "up") == 0)
            {
                GetComponent<BohrenScript>().moveUp();
            }

            if (string.Compare(data, "on") == 0)
            {
                GetComponent<BohrenScript>().turnOn();
            }
            if (string.Compare(data, "off") == 0)
            {
                GetComponent<BohrenScript>().turnOff();
            }
            if (string.Compare(data, "stop") == 0)
            {
                GetComponent<BohrenScript>().stopMovement();
            }
            if (string.Compare(data, "limitU") == 0)
            {
                GetComponent<BohrenScript>().callLimitSensorUp();
            }
            if (string.Compare(data, "limitD") == 0)
            {
                GetComponent<BohrenScript>().callDepthSensor();
            }
            if (string.Compare(data, "st") == 0)
            {
                StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
                data = GetComponent<BohrenScript>().getMachineStatus().ToString();
                writer.WriteLine(data);
                writer.Flush();
            }
        }
    }

    public void LimitSwitchesReached(string data)
    {                     // send service number as acknowledgement when limit switches are called
        if (string.Compare(data, "Depth") == 0)
        {
            sendBackMessage("reached");
        }
        if (string.Compare(data, "limitU") == 0)
        {
            sendBackMessage("reached");
        }
    }

    public void sendBackMessage(string data)
    {                    // send service number as acknowledgement
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
